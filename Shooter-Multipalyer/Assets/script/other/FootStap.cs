using UnityEngine;

public class FootStap : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip FootstepSfx;
    public void playFootstep()
    {
        audioSource.PlayOneShot(FootstepSfx);
    }
}
