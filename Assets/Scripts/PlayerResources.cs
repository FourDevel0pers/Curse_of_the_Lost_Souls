using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    public static PlayerResources Instance;

    public int stones;
    public int planks;
    public int iron;
    public int nails;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(this);
    }
}