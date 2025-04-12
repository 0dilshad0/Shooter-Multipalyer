using UnityEngine;

public class Smoke : MonoBehaviour
{
    public ParticleSystem ExplosionFx;
    public float ExplosionTime = 4f;
   

    void Start()
    {
        ExplosionFx.Pause();
        Invoke("Explosion", ExplosionTime);
    }


    void Update()
    {

    }

    private void Explosion()
    {
        ExplosionFx.Play();
        Destroy(gameObject, 10f);
    }
}
