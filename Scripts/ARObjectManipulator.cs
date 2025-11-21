using UnityEngine;

public class ARObjectManipulator : MonoBehaviour
{
    private ARGestureDetector gestureDetector;
    private MessageController messageController;
    private Renderer cachedRenderer;
    private Color originalColor;
    private bool isSelected = false;
    public bool IsSelected => isSelected;


    void Awake()
    {
        FindMessageController();
        CacheRenderer();
        FindGestureDetector();
    }

    private void FindMessageController()
    {
        var reportObj = GameObject.Find("ARReport");
        if (reportObj != null)
        {
            messageController = reportObj.GetComponent<MessageController>();
        }
        else
        {
            Debug.LogWarning("[SelectableObject] No se encontró 'ARReport' en la escena.");
        }
    }

    private void CacheRenderer()
    {
        cachedRenderer = GetComponent<Renderer>();
        if (cachedRenderer != null)
        {
            originalColor = cachedRenderer.material.color;
        }
        else
        {
            Debug.LogWarning($"[SelectableObject] Renderer no encontrado en '{gameObject.name}'.");
        }
    }

    private void FindGestureDetector()
    {
        gestureDetector = FindFirstObjectByType<ARGestureDetector>(); 
        if (gestureDetector == null)
        {
            Debug.LogWarning("[gestureDetector] No se encontró en la escena.");
        }

    }

    // Llamar cuando el usuario selecciona este objeto
    public void Select()
    {
        isSelected = true;
        // Cambiar apariencia para mostrar selección
        if (cachedRenderer != null)
        {
            cachedRenderer.material.color = Color.yellow;
        }
        Debug.Log($"{gameObject.name} seleccionado");
        messageController?.ShowInfoMessage($"{gameObject.name} seleccionado");
    }

    public void Deselect()
    {
        isSelected = false;
        // Restaurar apariencia normal
        if (cachedRenderer != null)
        {
            cachedRenderer.material.color = originalColor;
        }
        Debug.Log($"{gameObject.name} deseleccionado");
        messageController?.ShowInfoMessage($"{gameObject.name} deseleccionado");

    }
}
