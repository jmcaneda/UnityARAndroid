using UnityEngine;

public class MessageController : MonoBehaviour
{
    [SerializeField] private InfoDisplay infoDisplay;

    private void Awake()
    {
        if (infoDisplay == null)
            infoDisplay = Object.FindAnyObjectByType<InfoDisplay>();
    }

    public void ShowStartMessage()
    {
        infoDisplay?.ShowInfo("Toca una superficie para colocar el objeto.");
    }

    public void ShowInfoMessage(string message)
    {
        infoDisplay?.ShowInfo(message);
    }


    public void ShowPlacementSuccess()
    {
        infoDisplay?.ShowInfo("Objeto colocado correctamente.");
    }

    public void ShowPlacementWarning()
    {
        infoDisplay?.ShowWarning("No se detectó una superficie válida.");
    }

    public void ShowPlacementBlocked()
    {
        infoDisplay?.ShowWarning("Ya hay un objeto en ese lugar.");
    }
    public void ShowPlacementPosition(Vector2 pos)
    {
        infoDisplay?.ShowInfo($"Objeto colocado en: {pos}");
    }

    public void ShowSelection(GameObject obj)
    {
        string name = obj.name;
        infoDisplay?.ShowInfo($"Objeto seleccionado: {name}");
    }

    public void ShowNoSelection()
    {
        infoDisplay?.ShowWarning("No hay objeto seleccionado.");
    }

    public void ShowDeletion()
    {
        infoDisplay?.ShowInfo("Objeto eliminado.");
    }

    public void ShowError(string message)
    {
        infoDisplay?.ShowError($"Error: {message}");
    }

    public void Clear()
    {
        infoDisplay?.ClearMessage();
    }
}
