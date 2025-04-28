using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class ReloadAll : MonoBehaviour
{
    public TMP_Text CurrentAmmoText;
    public TMP_Text TotalAmmoText;
    public TMP_Text GrenadeCountText;
    public TMP_Text SmokeCountText;
    public TMP_Text MedicCountText;
   

    public int TotalAmmo=280;
    public int MaxAmmo=45;
    public int CurrentAmmo=0;
    public int GrenadeCound=2;
    public int SmokeCound=1;
    public int MedicCound=2;
    public int ReloadTime = 2;
    public bool IsReload;
    public AudioClip ReloadSfx;
    public AudioClip ThrowSfx;
    public AudioClip HealingSfx;

    private Animator animator;
    private PlayAudio playAudio;
    private Health health;
    private Throw throwing;
    private Inventory inventory;
    private PhotonView photonView;
    void Start()
    {
        IsReload = false;
        photonView = GetComponentInParent<PhotonView>();
        playAudio = GetComponent<PlayAudio>();
        animator = GetComponent<Animator>();
        CurrentAmmo = MaxAmmo;
        inventory = GetComponent<Inventory>();
        health = GetComponent<Health>();
        throwing = GetComponent<Throw>();
    }

  
    void Update()
    {
        if (!photonView.IsMine) return;
        CurrentAmmoText.text = CurrentAmmo.ToString();
        TotalAmmoText.text = TotalAmmo.ToString();
        MedicCountText.text = MedicCound.ToString();
        GrenadeCountText.text = GrenadeCound.ToString();
        SmokeCountText.text = SmokeCound.ToString();
        
        if(CurrentAmmo == 0)
        {
            Reloading();
        }
    }

    public void Reloading()
    {
        if(TotalAmmo!=0 && !IsReload)
        {
            animator.SetTrigger("reload");
            StartCoroutine(reload());
        }
       
    }
    IEnumerator reload()
    {
        IsReload = true;
        playAudio.AudioPlay(ReloadSfx);
        yield return new WaitForSecondsRealtime(ReloadTime);

        int NeadAmmo = MaxAmmo - CurrentAmmo;
        int ReloadAmmo = Mathf.Min(TotalAmmo, NeadAmmo);
        CurrentAmmo += ReloadAmmo;
        TotalAmmo -=  ReloadAmmo;
        inventory.RemoveItem("ammo",true,ReloadAmmo);
        IsReload = false; 
    }

    public bool CanShoot()
    {
        return CurrentAmmo != 0 && !IsReload;
    }
    public void Healing()
    {
      if(MedicCound!=0 && !health.HealthIsFull())
        {
            animator.SetTrigger("heal");
            StartCoroutine(heal());
           
        }
       
    }
    IEnumerator heal()
    {
        playAudio.AudioPlay(HealingSfx);
        IsReload = true;
        MedicCound--;
        inventory.RemoveItem("medic",true);
        yield return new WaitForSecondsRealtime(ReloadTime);
        health.Heal();
        IsReload = false;
    }

    public void GrenadeThrow()
    {
        if(GrenadeCound!=0)
        {
            playAudio.AudioPlay(ThrowSfx);
            animator.SetTrigger("grenade");
            throwing.BompThrew();
            inventory.RemoveItem("grenade", true);
            GrenadeCound--;
        }
        
    }public void Smoke_Throw()
    {
        if (SmokeCound != 0)
        {
            playAudio.AudioPlay(ThrowSfx);
            animator.SetTrigger("grenade");
            throwing.SmokThrew();
            inventory.RemoveItem("smoke", true);
            SmokeCound--;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("medic") && inventory.AddItem("medic"))
        {
            MedicCound++;
            PhotonView itemView = other.GetComponent<PhotonView>();
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(other.gameObject);
            }
            else if (itemView != null)
            {
                photonView.RPC("RequestDestroyObject", RpcTarget.MasterClient, itemView.ViewID);
            }

        }

        if(other.CompareTag("ammo") && inventory.AddItem("ammo",100))
        {
            TotalAmmo += 100;
            PhotonView itemView = other.GetComponent<PhotonView>();
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(other.gameObject);
            }
            else if (itemView != null)
            {
                photonView.RPC("RequestDestroyObject", RpcTarget.MasterClient, itemView.ViewID);
            }
        }
        if(other.CompareTag("grenade") && inventory.AddItem("grenade"))
        {
            GrenadeCound++;
            PhotonView itemView = other.GetComponent<PhotonView>();
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(other.gameObject);
            }
            else if (itemView != null)
            {
                photonView.RPC("RequestDestroyObject", RpcTarget.MasterClient, itemView.ViewID);
            }
        }
        if(other.CompareTag("smoke") && inventory.AddItem("smoke"))
        {
            SmokeCound++;
            PhotonView itemView = other.GetComponent<PhotonView>();
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(other.gameObject);
            }
            else if (itemView != null)
            {
                photonView.RPC("RequestDestroyObject", RpcTarget.MasterClient, itemView.ViewID);
            }
        }


        if(other.CompareTag("helmet1") && health.PickLevel1() && inventory.AddItem("helmet1"))
        {
            if(health.Helmet1.activeInHierarchy)
            {
                inventory.RemoveItem("helmet1", true);
            }
            else if(health.Helmet2.activeInHierarchy)
            {
                inventory.RemoveItem("helmet2", true);
            }
            health.HelmetPic(25 , "Level1");
            PhotonView itemView = other.GetComponent<PhotonView>();
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(other.gameObject);
            }
            else if (itemView != null)
            {
                photonView.RPC("RequestDestroyObject", RpcTarget.MasterClient, itemView.ViewID);
            }
        }
        if(other.CompareTag("helmet2") && health.PickLevel2() && inventory.AddItem("helmet2"))
        {
            if (health.Helmet1.activeInHierarchy)
            {
                inventory.RemoveItem("helmet1", true);
            }
            else if (health.Helmet2.activeInHierarchy)
            {
                inventory.RemoveItem("helmet2", true);
            }
            health.HelmetPic(50, "Level2");
            PhotonView itemView = other.GetComponent<PhotonView>();
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(other.gameObject);
            }
            else if (itemView != null)
            {
                photonView.RPC("RequestDestroyObject", RpcTarget.MasterClient, itemView.ViewID);
            }
        }

       
    }

    [PunRPC]
    void RequestDestroyObject(int viewID)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView view = PhotonView.Find(viewID);
            if (view != null)
            {
                PhotonNetwork.Destroy(view.gameObject);
            }
        }
    }
}
