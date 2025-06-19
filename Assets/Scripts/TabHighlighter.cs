using UnityEngine;
using UnityEngine.UI;

public class TabHighlighter : MonoBehaviour
{
    public Button[] tabButtons; 
    public Color activeColor = Color.yellow;
    public Color normalColor = Color.white;
    public float activeScale = 0.9f; 
    public float normalScale = 1f;

    public void OnTabClicked(Button clickedButton)
    {
        foreach (Button btn in tabButtons)
        {
            btn.GetComponent<Image>().color = normalColor;
            btn.transform.localScale = Vector3.one * normalScale;
        }

        clickedButton.GetComponent<Image>().color = activeColor;
        clickedButton.transform.localScale = Vector3.one * activeScale;
    }
}
