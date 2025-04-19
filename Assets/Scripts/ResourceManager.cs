using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    public int planks;
    public int stones;
    public int iron;
    public int nails;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }
}
