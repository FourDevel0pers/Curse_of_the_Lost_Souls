using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettingsManager : MonoBehaviour
{
    public TMP_Dropdown qualityDropdown; // ���� Dropdown ��� ������ ��������

    private int selectedQualityIndex; // ���� ����� ��������� �����

    void Start()
    {
        // ���������� ������� ������� �������� � Dropdown
        selectedQualityIndex = QualitySettings.GetQualityLevel();
        qualityDropdown.value = selectedQualityIndex;
        qualityDropdown.RefreshShownValue();

        // ����� ������������ �������� ���-��, �������� OnQualityChanged
        qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
    }

    void OnDestroy()
    {
        // ����� ����� ���������� �� ������� ����� �������� ������
        qualityDropdown.onValueChanged.RemoveListener(OnQualityChanged);
    }

    public void OnQualityChanged(int index)
    {
        selectedQualityIndex = index; // ������ ���������� ��������� �������
    }

    public void ApplyGraphicsSettings()
    {
        QualitySettings.SetQualityLevel(selectedQualityIndex, true);
        Debug.Log("�������� ����������� �� �������: " + QualitySettings.names[selectedQualityIndex]);
    }
}
