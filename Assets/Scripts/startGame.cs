using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Загружаем следующую сцену (например, сцену игры)
        SceneManager.LoadScene("Main"); // Убедитесь, что сцена "GameScene" добавлена в Build Settings
    }
}
