using Photon.Pun;
using UnityEngine;

public class BotRegDoll : MonoBehaviour
{
    private Rigidbody[] rigidbodies;
    private PhotonView photonView;
   

    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        photonView = GetComponent<PhotonView>();
        RegdollDesble();
    }
   

    public void Die()
    {
        photonView.RPC("RegdollEnable",RpcTarget.AllBuffered);    
        Destroy(gameObject, 1f);
        
    }

    [PunRPC]
    public void RegdollEnable()
    {
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = false;
        }
    }
    public void RegdollDesble()
    {
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = true;
        }
    }
}
