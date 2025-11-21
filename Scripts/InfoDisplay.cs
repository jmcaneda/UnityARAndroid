using TMPro;
using UnityEngine;

public class InfoDisplay : MonoBehaviour
{
    public TextMeshProUGUI infoText;

    [Header("Colores de mensaje")]
    public Color infoColor = Color.white;
    public Color warningColor = new Color(1f, 0.65f, 0f); // naranja
    public Color errorColor = Color.red;

    public void ShowInfo(string message)
    {
        SetMessage(message, infoColor);
    }

    public void ShowWarning(string message)
    {
        SetMessage(message, warningColor);
    }

    public void ShowError(string message)
    {
        SetMessage(message, errorColor);
    }

    public void ClearMessage()
    {
        infoText.text = "";
    }

    private void SetMessage(string message, Color color)
    {
        if (infoText != null)
        {
            infoText.text = message;
            infoText.color = color;
        }
    }
}
