using UnityEngine;

public class BotRegDoll : MonoBehaviour
{
    private Rigidbody[] rigidbodies;
    
   

    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
       
        RegdollDesble();
    }
   

    public void Die()
    {
        RegdollEnable();      
        Destroy(gameObject, 1f);
        
    }

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
