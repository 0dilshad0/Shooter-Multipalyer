using UnityEngine;
using UnityEngine.UI;

public class CorssTarget : MonoBehaviour
{
    public Image CrossArrowImg;
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

        if(hitinfo.collider.gameObject.GetComponentInParent<BotHealth>() ||  hitinfo.collider.gameObject.GetComponentInParent<Health>())
        {
            CrossArrowImg.color = Color.red;
        }
        else
        {
            CrossArrowImg.color = Color.white;
        }


    }
}
