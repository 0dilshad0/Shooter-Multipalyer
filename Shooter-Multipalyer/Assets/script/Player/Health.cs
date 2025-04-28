using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class Health : MonoBehaviourPun
{
    public Slider HealthBar;
    public Slider ShealdBar;
    public GameObject Helmet1;
    public GameObject Helmet1Img;
    public RagDoll ragDollAndDed;
    public GameObject Helmet2;
    public GameObject Helmet2Img;
   

    public int MaxSheald = 50;
    public int MaxHealth = 100;
    private int CurrentSheald;
    private int CurrentHealth;
    private PhotonView photonView;
    private bool IsDied;
    void Start()
    {
       
        photonView = GetComponentInParent<PhotonView>();
        if (!photonView.IsMine) return;

        HealthBar.maxValue = MaxHealth;
        ShealdBar.maxValue = MaxSheald;

        CurrentSheald = MaxSheald / 2;
        CurrentHealth = MaxHealth;

        IsDied = false;
    }


    void Update()
    {
        if (!photonView.IsMine) return;
        HealthBar.value = CurrentHealth;
        ShealdBar.value = CurrentSheald;

        if (CurrentSheald <= 0)
        {
            photonView.RPC("DisableHemet",RpcTarget.AllBuffered);
        }


       
    }

    public void Damage(int damage,Player attacker = null)
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) return;
        photonView.RPC("ApplyDamage", RpcTarget.AllBuffered, damage, attacker);

    }
    [PunRPC]
    private void ApplyDamage(int damage,Player attacker)
    {
        if (CurrentSheald > 0)
        {
            int ReminingDamage = damage - CurrentSheald;
            CurrentSheald = Mathf.Max(0, CurrentSheald - damage);

            if (ReminingDamage > 0)
            {
                CurrentHealth = Mathf.Max(0, CurrentHealth - ReminingDamage);
            }
        }
        else
        {
            CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
        }

        if (CurrentHealth <= 0 && IsDied==false)
        {
            IsDied = true;
            ragDollAndDed.Die(attacker);
        }
    }
    public void Heal()
    {
        CurrentHealth = MaxHealth;
    }

    public bool HealthIsFull()
    {
        return CurrentHealth == MaxHealth;
    }
    public void HelmetPic(int value, string type)
    {
        CurrentSheald += value;

        photonView.RPC("UpdateHemetUI",RpcTarget.AllBuffered,type);
    }

    [PunRPC]
    private void DisableHemet()
    {
        Helmet1.SetActive(false);
        Helmet1Img.SetActive(false);

        Helmet2.SetActive(false);
        Helmet2Img.SetActive(false);
    }

    [PunRPC]
    private void UpdateHemetUI(string type)
    {
        if (type == "Level1")
        {
            Helmet1.SetActive(true);
            Helmet1Img.SetActive(true);

            Helmet2.SetActive(false);
            Helmet2Img.SetActive(false);
        }
        else if (type == "Level2")
        {

            Helmet1.SetActive(false);
            Helmet1Img.SetActive(false);

            Helmet2.SetActive(true);
            Helmet2Img.SetActive(true);

        }
    }

    public bool PickLevel1()
    {
        return CurrentSheald < 25;
    } public bool PickLevel2()
    {
        return CurrentSheald < 50;
    }
}
