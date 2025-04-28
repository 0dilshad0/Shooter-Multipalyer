using UnityEngine;
using Photon.Pun;

public class BotMovementSync : MonoBehaviour, IPunObservable
{
    private PhotonView photonView;
    private Vector3 networkPosition;
    private Quaternion networkRotation;

    private float lerpSpeed = 10f;

    private Animator animator;
    private float syncX, syncZ;

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
            // Smooth position sync
            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * lerpSpeed);

            // Safe rotation sync
            if (IsValidQuaternion(networkRotation))
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, Time.deltaTime * lerpSpeed);
            }

            // Sync animation movement
            if (animator != null)
            {
                animator.SetFloat("x", syncX);
                animator.SetFloat("z", syncZ);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send position, rotation, and animation floats
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

            if (animator != null)
            {
                stream.SendNext(animator.GetFloat("x"));
                stream.SendNext(animator.GetFloat("z"));
            }
        }
        else
        {
            // Receive position and rotation
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();

            if (animator != null)
            {
                syncX = (float)stream.ReceiveNext();
                syncZ = (float)stream.ReceiveNext();
            }
        }
    }

    // Utility to avoid invalid Quaternion data
    private bool IsValidQuaternion(Quaternion q)
    {
        return !(float.IsNaN(q.x) || float.IsNaN(q.y) || float.IsNaN(q.z) || float.IsNaN(q.w));
    }
}
