using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class CorssTarget : MonoBehaviour
{
    public Image CrossArrowImg;
    public Camera PlayerCamera;
    private Camera M_Camera;
    private Ray ray;
    private RaycastHit hitinfo;
    public PhotonView photonView;
    void Start()
    {
        //photonView = GetComponentInParent<PhotonView>();
        if (!photonView.IsMine) return;
        M_Camera = PlayerCamera;
       
    }

   
    void Update()
    {
        if (!photonView.IsMine) return;
        ray.origin = M_Camera.transform.position;
        ray.direction = M_Camera.transform.forward;
        Physics.Raycast(ray,out hitinfo);
        transform.position = hitinfo.point;

        //if(hitinfo.collider.gameObject.GetComponentInParent<BotHealth>() ||  hitinfo.collider.gameObject.GetComponentInParent<Health>())
        //{
        //    CrossArrowImg.color = Color.red;
        //}
        //else
        //{
        //    CrossArrowImg.color = Color.white;
        //}


    }
}
