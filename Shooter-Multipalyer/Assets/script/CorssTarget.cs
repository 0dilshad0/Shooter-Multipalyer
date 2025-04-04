using UnityEngine;

public class CorssTarget : MonoBehaviour
{
    private Camera M_Camara;
    private Ray ray;
    private RaycastHit hitinfo;
    void Start()
    {
        M_Camara = Camera.main;
    }

   
    void Update()
    {
        ray.origin = M_Camara.transform.position;
        ray.direction = M_Camara.transform.forward;
        Physics.Raycast(ray,out hitinfo);
        transform.position = hitinfo.point;
    }
}
