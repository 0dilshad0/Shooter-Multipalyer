using UnityEngine;

public class RagDoll : MonoBehaviour
{
    private Rigidbody[] rigidbodies;
    public Health health;

    private void OnEnable()
    {
        health.IsZero += RegdollEnable;
    }
    private void OnDisable()
    {
        health.IsZero -= RegdollEnable;
    }

    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        RegdollDesble();
    }

    
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
