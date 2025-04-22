using UnityEngine;

public class Throw : MonoBehaviour
{
    public GameObject Grenade;
    public GameObject Smoke;
    public float force = 20f;
    public Transform ThrowPoint;
    public Transform CamaraPoint;
    public void BompThrew()
    {
        throwing(Grenade);
    } public void SmokThrew()
    {
        throwing(Smoke); 
    }

    private void throwing(GameObject type)
    {
        
        ThrowPoint.LookAt(CamaraPoint);
        GameObject Bomp = Instantiate(type, ThrowPoint.position, Quaternion.identity);
        Rigidbody rb = Bomp.GetComponent<Rigidbody>();
        rb.AddForce(ThrowPoint.forward * force, ForceMode.Impulse);
    }


}
