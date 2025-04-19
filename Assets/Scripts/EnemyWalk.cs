using UnityEngine;

public class OrcFootsteps : MonoBehaviour
{
    public AudioClip[] footstepSounds; 
    private AudioSource audioSource;
    private Animator animator;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    void PlayFootstep()
    {
        if (footstepSounds.Length > 0)
        {
            // Вибір випадкового звуку
            int index = Random.Range(0, footstepSounds.Length);
            audioSource.PlayOneShot(footstepSounds[index]);
        }
    }

    // Викликається через анімацію
    void OnFootstep()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Walking"))
        {
            PlayFootstep();
        }
    }
}
