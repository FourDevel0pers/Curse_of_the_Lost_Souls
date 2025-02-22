using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool locked = false;
    [HideInInspector] public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeDoorState()
    {
        if (locked) return;
        animator.SetBool("Open", !animator.GetBool("Open"));
    }

    public void UnlockDoor()
    {
        locked = false;
    }
}
