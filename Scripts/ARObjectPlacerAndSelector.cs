using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARObjectPlacerAndSelector : MonoBehaviour
{
    [Header("AR Managers")]
    [SerializeField] private GameObject objectToPlace;
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private ARAnchorManager anchorManager;
    [SerializeField] private ARGestureDetector gestureDetector;

    [Header("UI & Messaging")]
    [SerializeField] private MessageController messageController;
    [SerializeField] private RawImage crosshairImage;
    [SerializeField] private TextMeshProUGUI crosshairButtonText;

    [Header("Visual Settings")]
    [SerializeField] private GameObject shotVFX;

    [Header("Placement Settings")]
    [SerializeField] private float placementThreshold = 0.1f;

    private List<GameObject> placedObjects = new List<GameObject>();
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private ARInputActions inputActions;
    private ARObjectManipulator currentSelectedObject;
    private bool crosshairVisible = false;


    void Awake()
    {
        inputActions = new ARInputActions();
    }

    void OnEnable()
    {
        inputActions.ARPlayer.Enable();
        inputActions.ARPlayer.Tap.performed += OnTouchPerformed;
        inputActions.ARPlayer.DoubleTap.performed += ctx => OnDoubleTap();
    }

    void OnDisable()
    {
        inputActions.ARPlayer.Tap.performed -= OnTouchPerformed;
        inputActions.ARPlayer.DoubleTap.performed -= ctx => OnDoubleTap();
        inputActions.ARPlayer.Disable();
    }
    private void OnDoubleTap()
    {
        Debug.Log("Double Tap detectado");
        messageController?.ShowInfoMessage("[INFO] Double Tap.");

        Vector2 touchPosition = inputActions.ARPlayer.TouchPosition.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject targetObject = hit.collider.gameObject;

            // Validar que el objeto tiene el tag correcto y está seleccionado
            if (hit.collider.CompareTag("Interactable") && targetObject == currentSelectedObject?.gameObject)
            {
                if (shotVFX != null)
                    Instantiate(shotVFX, hit.point, Quaternion.identity);

                StartCoroutine(DestroyWithVFX(targetObject));
                messageController?.ShowDeletion();
            }
            else
            {
                messageController?.ShowInfoMessage("[INFO] Doble toque sobre objeto no seleccionado.");
            }
        }
    }
    private void OnTouchPerformed(InputAction.CallbackContext context)
    {
        // Leer posición del toque
        Vector2 touchPosition = inputActions.ARPlayer.TouchPosition.ReadValue<Vector2>();

        // Intentar seleccionar un objeto primero
        if (TrySelectObject(touchPosition))
            return;

        // Validaciones defensivas
        if (raycastManager == null || hits == null)
        {
            Debug.LogWarning("RaycastManager o lista de hits no está disponible.");
            return;
        }

        if (objectToPlace == null)
        {
            Debug.LogWarning("Prefab no asignado: objectToPlace es null.");
            return;
        }

        // Intentar colocar objeto si no se seleccionó ninguno
        if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon) && hits.Count > 0)
        {
            Pose hitPose = hits[0].pose;

            // Limpiar referencias nulas en la lista
            placedObjects.RemoveAll(obj => obj == null);

            // Verificar proximidad con objetos existentes
            foreach (GameObject obj in placedObjects)
            {
                if (Vector3.Distance(obj.transform.position, hitPose.position) < placementThreshold)
                {
                    messageController?.ShowPlacementBlocked();
                    Debug.Log("Ya existe un objeto cerca. No se colocará otro.");
                    return;
                }
            }

            // Instanciar y anclar el nuevo objeto
            GameObject placedObject = Instantiate(objectToPlace, hitPose.position, hitPose.rotation);
            placedObjects.Add(placedObject);
            AnchorObject(placedObject, hitPose);

            messageController?.ShowPlacementPosition(hitPose.position);
            Debug.Log($"Objeto colocado en {hitPose.position}");
        }
    }

    private bool TrySelectObject(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            ARObjectManipulator manipulator = hit.collider.GetComponent<ARObjectManipulator>();
            if (manipulator != null)
            {
                if (currentSelectedObject != null)
                {
                    currentSelectedObject.Deselect();
                    gestureDetector.selectedObject = null;
                }

                currentSelectedObject = manipulator;
                currentSelectedObject.Select();
                gestureDetector.selectedObject = currentSelectedObject;

                return true;
            }
        }

        if (currentSelectedObject != null)
        {
            currentSelectedObject.Deselect();
            currentSelectedObject = null;
            gestureDetector.selectedObject = null;
        }

        return false;
    }

    private void AnchorObject(GameObject obj, Pose pose)
    {
        // Crear un nuevo Anchor en la posición detectada
        GameObject anchorGO = new GameObject("ARAnchor");
        anchorGO.transform.position = pose.position;

        // ⚠️ Importante: no tocar la rotación del anchor
        anchorGO.transform.rotation = Quaternion.identity;

        // Añadir el componente ARAnchor
        anchorGO.AddComponent<ARAnchor>();

        // Reparentar el objeto conservando su transform local
        obj.transform.SetParent(anchorGO.transform, true);
    }

    public void DeleteAllPrefabs()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Interactable"))
        {
            StartCoroutine(DestroyWithVFX(obj));
        }

        currentSelectedObject = null;
        gestureDetector.selectedObject = null;

        messageController?.ShowInfoMessage("[DEL] Todos los objetos eliminados.");
    }
    // Método para activar/desactivar la mirilla
    public void ToggleCrosshair()
    {
        crosshairVisible = !crosshairVisible;
        crosshairImage.gameObject.SetActive(crosshairVisible);

        if (crosshairVisible)
        {
            messageController?.ShowInfoMessage("[VER] Mirilla activada.");
            crosshairButtonText.text = "Desactivar Mirilla";
        }
        else
        {
            messageController?.ShowInfoMessage("[VER] Mirilla desactivada.");
            crosshairButtonText.text = "Activar Mirilla";
        }
    }

    // Método para disparar y destruir solo si la mirilla está activa
    public void FireAndDestroyCenterObject()
    {
        if (!crosshairVisible)
        {
            messageController?.ShowInfoMessage("[INF] La mirilla no está activa. Actívala primero.");
            return;
        }

        Vector2 screenCenter = new(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Interactable"))
        {
            GameObject targetObject = hit.collider.gameObject;

            // Instanciar efectos de disparo
            if (shotVFX != null)
                Instantiate(shotVFX, hit.point, Quaternion.identity);

            StartCoroutine(DestroyWithVFX(targetObject));
            messageController?.ShowDeletion();
        }
        else
        {
            messageController?.ShowInfoMessage("[INF] No hay objeto en el centro.");
        }
    }
    private IEnumerator DestroyWithVFX(GameObject targetObject)
    {
        if (targetObject == null) yield break;

        // Obtener el componente SelectableObject
        ARObjectManipulator selectable = targetObject.GetComponent<ARObjectManipulator>();
        ParticleSystem vfx = null;

        if (selectable != null)
        {
            Transform vfxTransform = selectable.transform.Find("DeleteVFX");
            if (vfxTransform != null)
            {
                GameObject vfxGO = vfxTransform.gameObject;
                vfxGO.SetActive(true);
                vfx = vfxGO.GetComponent<ParticleSystem>();

                if (vfx != null)
                {
                    vfxGO.transform.SetParent(null);
                    vfxGO.transform.position = targetObject.transform.position;
                    vfxGO.transform.rotation = targetObject.transform.rotation;

                    yield return null; // Esperar un frame

                    vfx.Play();

                    float delay = vfx.main.duration + vfx.main.startLifetime.constant;
                    //Destroy(vfxGO, delay);
                }
            }
        }

        yield return new WaitForSeconds(0.1f); // Pequeña pausa para asegurar visibilidad

        placedObjects.Remove(targetObject);

        Transform anchorTransform = targetObject.transform.parent;
        if (anchorTransform != null && anchorTransform.GetComponent<ARAnchor>() != null)
        {
            Destroy(anchorTransform.gameObject);
        }
        else
        {
            Destroy(targetObject);
        }

        if (vfx != null)
        {
            float delay = vfx.main.duration + vfx.main.startLifetime.constantMax;
            yield return new WaitForSeconds(delay);
        }
    }

}
