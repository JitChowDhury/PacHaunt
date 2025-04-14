using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] private AudioSource[] footsteps;
    [SerializeField] private AudioSource jumpAudio;
    [SerializeField] private AudioSource landAudio;

    private bool isJumping = false;

   
    // Plays a random footstep sound if not jumping.
   
    public void PlayFootStepAudio()
    {
        if (footsteps.Length == 0 || isJumping) return;

        int n = Random.Range(0, footsteps.Length);
        if (footsteps[n] && !footsteps[n].isPlaying)
        {
            footsteps[n].PlayOneShot(footsteps[n].clip);
        }
    }

    
    // Plays the jump sound and sets jumping state.
   
    public void PlayJumpAudio()
    {
        isJumping = true;
        if (jumpAudio && jumpAudio.clip)
        {
            jumpAudio.PlayOneShot(jumpAudio.clip);
        }
    }

    
    // Plays the landing sound and resets jumping state.
   
    public void PlayLandAudio()
    {
        isJumping = false;
        if (landAudio && landAudio.clip)
        {
            landAudio.PlayOneShot(landAudio.clip);
        }
    }
}
