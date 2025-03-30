using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Plank, Stone, Iron, Nail
}
public class ResourceController : MonoBehaviour
{
    public ResourceType resourceType;
    [SerializeField] private int minAmount = 1;
    [SerializeField] private int maxAmount = 3;

    public int SelectAmount()
    {
        return Random.Range(minAmount, maxAmount);
    }
}
