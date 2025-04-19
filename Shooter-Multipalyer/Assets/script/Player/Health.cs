using UnityEngine;
using UnityEngine.UI;
using System;

public class Health : MonoBehaviour
{
    public Slider HealthBar;
    public Slider ShealdBar;
    public GameObject Helmet1;
    public GameObject Helmet1Img;

    public GameObject Helmet2;
    public GameObject Helmet2Img;
    public event Action IsZero;

    public int MaxSheald = 50;
    public int MaxHealth = 100;
    private int CurrentSheald;
    private int CurrentHealth;
    

    void Start()
    {
        HealthBar.maxValue = MaxHealth;
        ShealdBar.maxValue = MaxSheald;

        CurrentSheald = MaxSheald / 2;
        CurrentHealth = MaxHealth;
    }

   
    void Update()
    {
        HealthBar.value = CurrentHealth;
        ShealdBar.value = CurrentSheald;
 
        if(CurrentSheald <=0)
        {
            Helmet1.SetActive(false);
            Helmet2.SetActive(false);
        }

        if(CurrentHealth ==0)
        {
            IsZero?.Invoke();
        }
    }

    public void Damage(int damage)
    {
        if(CurrentSheald>0)
        {
            int ReminingDamage = damage - CurrentSheald;
            CurrentSheald = Mathf.Max(0, CurrentSheald - damage);

            if(ReminingDamage>0)
            {
                CurrentHealth = Mathf.Max(0, CurrentHealth - ReminingDamage);
            }
        }
        else
        {
            CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
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
    public void HelmetPic(int value , string type)
    {
        CurrentSheald += value;

        if(type == "Level1")
        {
            Helmet1.SetActive(true);
            Helmet1Img.SetActive(true);

            Helmet2.SetActive(false);
            Helmet2Img.SetActive(false);
        }
        else if(type == "Level2")
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
