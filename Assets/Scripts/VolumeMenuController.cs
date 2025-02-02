using UnityEngine;

public class VolumeMenuController : MonoBehaviour
{
    public GameObject volumePanel;

    public void ToggleVolumePanel()
    {
        volumePanel.SetActive(!volumePanel.activeSelf);
    }
}

