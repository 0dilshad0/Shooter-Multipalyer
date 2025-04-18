using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public float TurnSpeed = 4f;
    public GameObject FirstPersonCamara;
    private Camera M_canara;

    void Start()
    {
        M_canara = Camera.main;
    }

    
    void Update()
    {
        float Mycamara = M_canara.transform.eulerAngles.y;
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
