using UnityEngine;

public class AutoDisable : MonoBehaviour
{
    public float time = 2f;

    private void OnEnable()
    {
        CancelInvoke();
        Invoke("desable", time);
    }

    private void desable()
    {
        gameObject.SetActive(false);
    }
}
