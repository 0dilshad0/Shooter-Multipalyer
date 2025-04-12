using Unity.VisualScripting;
using UnityEngine;

public class GunSwap : MonoBehaviour
{
    public GameObject[] guns = new GameObject[6];
    public GameObject[] InventoryIcon = new GameObject[6];

    public GameObject[] PickButton = new GameObject[6];

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if(other.CompareTag("M4"))
        {
         
            PickButton[0].SetActive(true); 
        }
        else if(other.CompareTag("SCAR-L"))
        {
           
            PickButton[1].SetActive(true);
        }
        else if(other.CompareTag("XM"))
        {
            
            PickButton[2].SetActive(true);
        }
        else if(other.CompareTag("MachineGun"))
        {
            
            PickButton[3].SetActive(true);
        }
        else if(other.CompareTag("ShotGun"))
        {
            
            PickButton[4].SetActive(true);
        }
        else if(other.CompareTag("Sniper"))
        {
           
            PickButton[5].SetActive(true);
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        PickButtonUI();
    }

    private void PickButtonUI()
    {
        foreach(GameObject ui in PickButton)
        {
            ui.SetActive(false);
        }
    }

    public void GunPick(int i)
    {
        foreach(GameObject gun in guns)
        {
            gun.SetActive(false);
        }
        guns[i].SetActive(true);
        foreach (GameObject gunIcon in InventoryIcon)
        {
            gunIcon.SetActive(false);
        }
        InventoryIcon[i].SetActive(true);

        PickButtonUI();
    }
    
}
