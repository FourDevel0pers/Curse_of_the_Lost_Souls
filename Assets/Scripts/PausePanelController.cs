using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanelController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Animator crossFadeAnimator;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject centerPoint;
    [HideInInspector] public bool canPause = true;

    private void Update()
    {
        if (!canPause) return;
        if(Input.GetKeyDown(KeyCode.Escape)) ChangePauseState();
    }

    private void ChangePauseState()
    {
        pausePanel.SetActive(!pausePanel.activeSelf);
        Time.timeScale = pausePanel.activeSelf ? 0f : 1f;
        Cursor.lockState = pausePanel.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = pausePanel.activeSelf;
        crosshair.SetActive(!crosshair.activeSelf);
        centerPoint.SetActive(!centerPoint.activeSelf);
    }

    public void OnResumeButtonClick()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    public void OnBackToMenuButtonClick()
    {
        crossFadeAnimator.SetBool("Fade", true);
        canPause = false;
        StartCoroutine(LoadMainMenu());
    }

    private IEnumerator LoadMainMenu()
    {
        yield return new WaitForSecondsRealtime(.5f);
        SceneManager.LoadScene(0);
    }
}
