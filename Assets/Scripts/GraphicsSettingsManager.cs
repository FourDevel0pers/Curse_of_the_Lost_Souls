using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettingsManager : MonoBehaviour
{
    public TMP_Dropdown qualityDropdown; // Твой Dropdown для выбора качества

    private int selectedQualityIndex; // Сюда будем сохранять выбор

    void Start()
    {
        // Установить текущий уровень качества в Dropdown
        selectedQualityIndex = QualitySettings.GetQualityLevel();
        qualityDropdown.value = selectedQualityIndex;
        qualityDropdown.RefreshShownValue();

        // Когда пользователь выбирает что-то, вызываем OnQualityChanged
        qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
    }

    void OnDestroy()
    {
        // Очень важно отписаться от событий чтобы избежать ошибок
        qualityDropdown.onValueChanged.RemoveListener(OnQualityChanged);
    }

    public void OnQualityChanged(int index)
    {
        selectedQualityIndex = index; // Просто запоминаем выбранный уровень
    }

    public void ApplyGraphicsSettings()
    {
        QualitySettings.SetQualityLevel(selectedQualityIndex, true);
        Debug.Log("Качество установлено на уровень: " + QualitySettings.names[selectedQualityIndex]);
    }
}
