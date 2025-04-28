using Photon.Pun;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public float TurnSpeed = 4f;
    public GameObject FirstPersonCamara;
    private Camera M_camera;
    public Camera PlayerCamera;
    private PhotonView photonView;
    void Start()
    {
        photonView = GetComponentInParent<PhotonView>();
        if (!photonView.IsMine) return;
        M_camera = PlayerCamera;
    }

    
    void Update()
    {
        if (!photonView.IsMine) return;

        float Mycamara = M_camera.transform.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, Mycamara, 0), TurnSpeed * Time.deltaTime);
    }

    public void SwitchCamara()
    {
        if(FirstPersonCamara.activeInHierarchy)
        {
            FirstPersonCamara.SetActive(false);
        }
        else
        {
            FirstPersonCamara.SetActive(true);
        }
    }
}
