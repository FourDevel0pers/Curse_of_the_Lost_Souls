using UnityEngine;

public class HeadTurnTrigger : MonoBehaviour
{
    public int lookCount = 0;
    public ParticleSystem smokeEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LookZone"))
        {
            lookCount++;

            if (lookCount == 2 && smokeEffect != null)
            {
                smokeEffect.Play();
            }
        }
    }
}
