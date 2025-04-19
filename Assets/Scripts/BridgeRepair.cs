using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BridgeRepair : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    public GameObject brokenBridge;
    public GameObject fixedBridge;
    public GameObject blackScreen;
    public float screenDuration = 2f;

    public int requiredWood = 0;
    public int requiredRope = 0;

    private bool playerInRange = false;
    private PlayerResources playerResources;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerResources = other.GetComponent<PlayerResources>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerResources = null;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            TryRepairBridge();
        }
    }

    void TryRepairBridge()
    {
        if (playerResources == null) return;

        if (playerResources.wood >= requiredWood && playerResources.rope >= requiredRope)
        {
            playerResources.wood -= requiredWood;
            playerResources.rope -= requiredRope;
            StartCoroutine(RepairSequence());
        }
        else
        {
            Debug.Log("Not enough resources!");
        }
    }

    IEnumerator RepairSequence()
    {
        blackScreen.SetActive(true);
        yield return new WaitForSeconds(screenDuration);

        brokenBridge.SetActive(false);
        fixedBridge.SetActive(true);
        blackScreen.SetActive(false);
    }

}

