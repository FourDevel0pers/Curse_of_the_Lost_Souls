using UnityEngine;
using TMPro;
using System.Collections;

public class BridgeRepair : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    public GameObject brokenBridge;
    public GameObject fixedBridge;
    public GameObject blackScreen;
    public GameObject endScreenCanvas; // ← новий Canvas з написом "To be continued..."
    public float screenDuration = 2f;

    public int requiredStone = 5;
    public int requiredPlanks = 5;
    public int requiredIron = 5;
    public int requiredNails = 5;

    public TextMeshProUGUI interactText;

    private bool playerInRange = false;
    private PlayerResources playerResources;

    private void Start()
    {
        playerResources = PlayerResources.Instance;
        if (interactText != null)
            interactText.gameObject.SetActive(false);

        if (endScreenCanvas != null)
            endScreenCanvas.SetActive(false); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactText != null)
                interactText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactText != null)
                interactText.gameObject.SetActive(false);
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
        Debug.Log("sdawdasdaawdasda");
        if (playerResources == null) return;

        bool hasResources =
            playerResources.stones >= requiredStone &&
            playerResources.planks >= requiredPlanks &&
            playerResources.iron >= requiredIron &&
            playerResources.nails >= requiredNails;

        if (hasResources)
        {
            playerResources.stones -= requiredStone;
            playerResources.planks -= requiredPlanks;
            playerResources.iron -= requiredIron;
            playerResources.nails -= requiredNails;

            if (interactText != null)
                interactText.gameObject.SetActive(false);

            StartCoroutine(RepairSequence());
        }
        else
        {
            Debug.Log("Not enough resources to repair the bridge!");
        }
    }

    IEnumerator RepairSequence()
    {
        blackScreen.SetActive(true);
        yield return new WaitForSeconds(screenDuration);

        brokenBridge.SetActive(false);
        fixedBridge.SetActive(true);
        blackScreen.SetActive(false);

        if (endScreenCanvas != null)
        {
            endScreenCanvas.SetActive(true);
            Time.timeScale = 0f; // зупиняє гру (опціонально)
        }

        Destroy(gameObject); // прибираємо тригер ремонту
    }
}
