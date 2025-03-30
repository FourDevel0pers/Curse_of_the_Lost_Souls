using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public LayerMask interactionLayer;
    public float interactionDistance;

    [Header("UI")]
    public TextMeshProUGUI plankText;
    public TextMeshProUGUI stoneText;
    public TextMeshProUGUI ironText;
    public TextMeshProUGUI nailText;

    private GameObject interactionObject;
    private Transform mainCamera;
    private ResourceManager resourceManager;

    private void Start()
    {
        mainCamera = Camera.main.transform;
        resourceManager = ResourceManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) Interact();
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out RaycastHit hit, interactionDistance))
        {
            if (!hit.transform.gameObject.Equals(interactionObject))
            {
                interactionObject = hit.transform.gameObject;
            }
        }
        else
        {
            interactionObject = null;
        }
    }

    private void Interact()
    {
        if (!interactionObject) return;

        if(interactionObject.TryGetComponent(out ResourceController resource))
        {
            PickupItem(resource);
        }
    }

    private void PickupItem(ResourceController resource)
    {
        int amount = resource.SelectAmount();
        switch (resource.resourceType)
        {
            case ResourceType.Plank:
                resourceManager.planks += amount;
                plankText.text = resourceManager.planks.ToString();
                break;
            case ResourceType.Stone:
                resourceManager.stones += amount;
                stoneText.text = resourceManager.stones.ToString();
                break;
            case ResourceType.Iron:
                resourceManager.iron += amount;
                ironText.text = resourceManager.iron.ToString();
                break;
            case ResourceType.Nail:
                resourceManager.nails += amount;
                nailText.text = resourceManager.nails.ToString();
                break;
            default:
                return;
        } 
        Destroy (resource.gameObject);
    }
}
