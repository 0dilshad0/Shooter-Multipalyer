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

    private Animator animator;
    private Health health;
    private Throw throwing;
    private Inventory inventory;
    void Start()
    {
        IsReload = false;
        animator = GetComponent<Animator>();
        CurrentAmmo = MaxAmmo;
        inventory = GetComponent<Inventory>();
        health = GetComponent<Health>();
        throwing = GetComponent<Throw>();
    }

  
    void Update()
    {
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
            animator.SetTrigger("grenade");
            throwing.BompThrew();
            inventory.RemoveItem("grenade", true);
            GrenadeCound--;
        }
        
    }public void Smoke_Throw()
    {
        if (SmokeCound != 0)
        {
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
            Destroy(other.gameObject);
        }

        if(other.CompareTag("ammo") && inventory.AddItem("ammo",100))
        {
            TotalAmmo += 100;
            Destroy(other.gameObject);
        }
        if(other.CompareTag("grenade") && inventory.AddItem("grenade"))
        {
            GrenadeCound++;
            Destroy(other.gameObject);
        }if(other.CompareTag("smoke") && inventory.AddItem("smoke"))
        {
            SmokeCound++;
            Destroy(other.gameObject);
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
            Destroy(other.gameObject);
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
            Destroy(other.gameObject);
        }

       
    }
}
