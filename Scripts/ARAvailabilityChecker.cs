using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections;

public class ARAvailabilityChecker : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(CheckARAvailability());
    }

    IEnumerator CheckARAvailability()
    {
        // Verificar soporte AR
        if ((ARSession.state == ARSessionState.None) ||
            (ARSession.state == ARSessionState.CheckingAvailability))
        {
            yield return ARSession.CheckAvailability();
        }

        if (ARSession.state == ARSessionState.Unsupported)
        {
            // AR no soportado en este dispositivo
            Debug.LogError("AR no disponible en este dispositivo");
            ShowErrorMessage("Este dispositivo no soporta ARCore");
        }
        else if (ARSession.state == ARSessionState.NeedsInstall)
        {
            // ARCore necesita instalarse
            Debug.Log("ARCore requiere instalación");
            yield return ARSession.Install();

            if (ARSession.state == ARSessionState.Installing)
            {
                Debug.Log("Instalando ARCore...");
            }
        }
        else
        {
            // AR disponible, iniciar sesión
            Debug.Log("AR disponible y listo");
            InitializeAR();
        }
    }

    private void ShowErrorMessage(string message)
    {
        // Mostrar UI de error al usuario
    }

    private void InitializeAR()
    {
        // Iniciar experiencia AR
    }
}
