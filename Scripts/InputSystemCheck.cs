using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemCheck : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Input System instalado correctamente");
        Debug.Log("Versión: " + InputSystem.version);
    }
}
