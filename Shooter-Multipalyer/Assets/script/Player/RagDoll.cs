
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class RagDoll : MonoBehaviourPun
{

    private Rigidbody[] rigidbodies;
    private PhotonView photonView;

    
    public GameObject weapon;
    public GameObject Player;
    public SpriteRenderer marker;
    public BasicMove basicMove;
    public Animator animator;
    public PlayerRotation playerRotation;
    

    


    void Start()
    {
        photonView = GetComponent<PhotonView>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        RegdollDesble();
    }

    public void Die(Player attacker)
    {
        Debug.Log($"{attacker.NickName} killed {photonView.Owner.NickName}");
        if(attacker!=null)
        {
            int currentKills = 0;
            if (attacker.CustomProperties.TryGetValue("Kills", out object kills))
            {
                currentKills = (int)kills;
            }

            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
            {
                { "Kills", currentKills + 1 }
            };
            attacker.SetCustomProperties(props);
        }



        photonView.RPC("RegdollEnable", RpcTarget.AllBuffered);
        photonView.RPC("HandleDeath", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void HandleDeath()
    {
       
        weapon.SetActive(false);

        marker.enabled = false;
        basicMove.enabled = false;
        animator.enabled = false;
        playerRotation.enabled = false;

        if(photonView.IsMine)
        {
            Invoke("Respown", 2f);
        }
    }


    private void Respown()
    {
        PlaayerSpawner spawner = FindAnyObjectByType<PlaayerSpawner>();
        spawner.Spawn();

        PhotonNetwork.Destroy(Player);
        

    }



    [PunRPC]
    public void RegdollEnable()
    {
       
       foreach(var rb in rigidbodies)
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
