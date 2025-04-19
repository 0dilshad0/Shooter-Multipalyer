
using UnityEngine;
using UnityEngine.Events;

public class BotHealth : MonoBehaviour
{
    public GameObject Helmet1;

    public int MaxSheald = 50;
    public int MaxHealth = 100;
    private int CurrentSheald;
    private int CurrentHealth;
    public UnityEvent OnDie;

    void Start()
    {
        CurrentSheald = MaxSheald / 2;
        CurrentHealth = MaxHealth;
    }

    
    void Update()
    {
        if(CurrentSheald <= 0)
        {
            Helmet1.SetActive(false);
            
        }

        if(CurrentHealth <= 0)
        {

            OnDie?.Invoke();

        }
    }

    public void Damage(int damage)
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

    }
}
