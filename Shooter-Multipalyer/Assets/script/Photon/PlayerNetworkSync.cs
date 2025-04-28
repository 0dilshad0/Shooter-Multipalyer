using UnityEngine;
using Photon.Pun;
using UnityEngine.Animations.Rigging;

public class PlayerNetworkSync : MonoBehaviour, IPunObservable
{
    private PhotonView photonView;
    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private float lerpSpeed = 10f;

    private Animator animator;
    private float syncX, syncZ;

    private bool triggerReload, triggerHeal, triggerGrenade;

    private float syncRigWeight;
    public Rig fireRig; 

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        networkPosition = transform.position;
        networkRotation = transform.rotation;
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            // Smooth transform sync
            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * lerpSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, Time.deltaTime * lerpSpeed);

            // Movement animations
            animator.SetFloat("x", syncX);
            animator.SetFloat("z", syncZ);

            // Trigger animations
            if (triggerReload)
            {
                animator.SetTrigger("reload");
                triggerReload = false;
            }
            if (triggerHeal)
            {
                animator.SetTrigger("heal");
                triggerHeal = false;
            }
            if (triggerGrenade)
            {
                animator.SetTrigger("grenade");
                triggerGrenade = false;
            }

            // Rig weight sync
            if (fireRig != null)
            {
                fireRig.weight = Mathf.Lerp(fireRig.weight, syncRigWeight, Time.deltaTime * 10f);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Sync transform
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

            // Sync movement params
            stream.SendNext(animator.GetFloat("x"));
            stream.SendNext(animator.GetFloat("z"));

            // Sync triggers
            stream.SendNext(animator.GetBool("reload"));
            stream.SendNext(animator.GetBool("heal"));
            stream.SendNext(animator.GetBool("grenade"));

            // Sync rig weight
            if (fireRig != null)
                stream.SendNext(fireRig.weight);
        }
        else
        {
            // Receive transform
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();

            // Receive movement params
            syncX = (float)stream.ReceiveNext();
            syncZ = (float)stream.ReceiveNext();

            // Receive triggers
            triggerReload = (bool)stream.ReceiveNext();
            triggerHeal = (bool)stream.ReceiveNext();
            triggerGrenade = (bool)stream.ReceiveNext();

            // Receive rig weight
            syncRigWeight = (float)stream.ReceiveNext();
        }
    }

    // Call this from your local player input script to trigger animations
    public void SetTrigger(string triggerName)
    {
        if (photonView.IsMine)
        {
            animator.SetTrigger(triggerName);
        }
    }
}
