using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class Inventory : MonoBehaviour
{
    
    public TMP_Text TotalAmmoText;
    public TMP_Text GrenadeCountText;
    public TMP_Text SmokeCountText;
    public TMP_Text MedicCountText;
    public Slider WeightLevel;
    public GameObject InventoryPanal;
    public int MaxSize = 500;
    public Transform DropPont;
    public Dictionary<string, int> InvetaryItem = new Dictionary<string, int>();

    private int CurrentWeigth;
    private bool IsEnable;
    private ReloadAll reloadAll;
    private Dictionary<string, int> ItemWeights = new Dictionary<string, int>
    {
        {"ammo",1},
        {"medic",20},
        {"grenade",20},
        {"smoke",20},
        {"helmet1",30},
        {"helmet2",50}

    };

    void Start()
    {
        WeightLevel.maxValue = MaxSize;

        reloadAll = GetComponent<ReloadAll>();
        InventoryPanal.SetActive(false);
        IsEnable = false;
        AddItemsStart();
    }

    
    void Update()
    {
        WeightLevel.value = CurrentWeigth;
        TotalAmmoText.text = reloadAll.TotalAmmo.ToString();
        GrenadeCountText.text = reloadAll.GrenadeCound.ToString();
        SmokeCountText.text = reloadAll.SmokeCound.ToString();
        MedicCountText.text = reloadAll.MedicCound.ToString();

       
    }

    private void AddItemsStart()
    {
        AddItem("ammo", reloadAll.TotalAmmo);
        AddItem("medic", reloadAll.MedicCound);
        AddItem("grenade", reloadAll.GrenadeCound);
        AddItem("smoke", reloadAll.SmokeCound);
        AddItem("helmet1", 1);
      
    }


    public void InventoryONOFF()
    {
        if(IsEnable)
        {
            IsEnable = false;
            InventoryPanal.SetActive(false);
        }
        else
        {
            IsEnable = true;
            InventoryPanal.SetActive(true);
        }
    }   
    
    public bool AddItem(string Item, int quantity =1)
    {
        if(ItemWeights.ContainsKey(Item))
        {
            int ItemWeight = ItemWeights[Item] * quantity;
            if(CurrentWeigth + ItemWeight <= MaxSize)
            {
                CurrentWeigth += ItemWeight;
         
                return true;

              
            }
            else
            {
                return false;
            }

        }
       
            return false;
    }

    public void Drop(string Item)
    {
        if(ItemWeights.ContainsKey(Item))
        {

            if (Item == "ammo" && reloadAll.TotalAmmo - 100 >= 0)
            {
                RemoveItem(Item, false, 100);
                reloadAll.TotalAmmo -= 100;
            }
            else if (Item == "grenade" && reloadAll.GrenadeCound != 0)
            {
                RemoveItem(Item, false, 1);
                reloadAll.GrenadeCound--;
            }
            else if (Item == "medic" && reloadAll.MedicCound != 0)
            {
                RemoveItem(Item, false, 1);
                reloadAll.MedicCound--;
            }
            else if (Item == "smoke" && reloadAll.SmokeCound !=0)
            {
                RemoveItem(Item, false, 1);
                reloadAll.SmokeCound--;
            }
        }
        
       
      
    }
    public void RemoveItem(string Item , bool use , int quantity = 1)
    {
        if(ItemWeights.ContainsKey(Item))
        {
            int ItemWeight = ItemWeights[Item] * quantity;
            CurrentWeigth -= ItemWeight;
            InvetaryItem.Remove(Item);

            if(!use)
            {
                DropItem(Item);
            }
        }
    }

    public void  DropItem(string Item)
    {
        GameObject DropedItem = PhotonNetwork.Instantiate("picUp/" + Item, DropPont.position, Quaternion.identity);
    }
}
