using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource WalkSource;
   

    public void AudioPlay(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void FootStepSoundPlay()
    {
        if (!WalkSource.isPlaying)
        {
            WalkSource.Play();
        }
     
    }public void FootStepSoundPause()
    {
        if (WalkSource.isPlaying)
        {
            WalkSource.Pause();
        }
     
    }
    
}

   
