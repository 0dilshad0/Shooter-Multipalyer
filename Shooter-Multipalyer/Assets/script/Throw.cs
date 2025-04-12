using UnityEngine;

public class Throw : MonoBehaviour
{
    public GameObject Grenade;
    public GameObject Smoke;
    public float force = 20f;
    public Transform ThrowPoint;
    public void BompThrew()
    {
        throwing(Grenade);
    } public void SmokThrew()
    {
        throwing(Smoke); 
    }

    private void throwing(GameObject type)
    {
        GameObject Bomp = Instantiate(type, ThrowPoint.position, Quaternion.identity);
        Rigidbody rb = Bomp.GetComponent<Rigidbody>();
        rb.AddForce(ThrowPoint.forward * force, ForceMode.Impulse);
    }


}
