using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using InputTouchPhase = UnityEngine.InputSystem.TouchPhase;

public class ARGestureDetector : MonoBehaviour
{
    [Header("AR Managers")]
    public ARObjectManipulator selectedObject;
    [SerializeField] private ARRaycastManager raycastManager;

    [Header("UI & Messaging")]
    [SerializeField] private MessageController messageController;

    private float initialDistance;
    private Vector3 initialScale;
    private const float movementThreshold = 5f;
    private readonly List<ARRaycastHit> rayHits = new();

    private void OnEnable() => EnhancedTouchSupport.Enable();

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        initialDistance = 0f;
    }

    private void Update()
    {
        if (selectedObject == null || !selectedObject.IsSelected || !EnhancedTouchSupport.enabled)
            return;

        var touches = Touch.activeTouches;

        switch (touches.Count)
        {
            case 1:
                HandleSingleTouch(touches[0]);
                break;
            case 2:
                HandlePinchToScale(touches[0], touches[1]);
                HandleTwistToRotate(touches[0], touches[1]);
                break;
            default:
                initialDistance = 0f;
                break;
        }
    }

    private void HandleSingleTouch(Touch touch)
    {
        bool isDragging = touch.phase == InputTouchPhase.Moved && touch.delta.magnitude > movementThreshold;

        if (isDragging)
            MoveSelectedObject(touch.screenPosition);
    }

    private void HandlePinchToScale(Touch touch0, Touch touch1)
    {
        float currentDistance = Vector2.Distance(touch0.screenPosition, touch1.screenPosition);

        if (initialDistance == 0f)
        {
            initialDistance = currentDistance;
            initialScale = selectedObject.transform.localScale;
            messageController?.ShowInfoMessage("[aA] Escalando...");
            return;
        }

        float scaleFactor = Mathf.Clamp(currentDistance / initialDistance, 0.5f, 2f);
        selectedObject.transform.localScale = initialScale * scaleFactor;
    }

    private void HandleTwistToRotate(Touch touch0, Touch touch1)
    {
        float rawAngle = Vector2.SignedAngle(touch0.delta, touch1.delta);
        float smoothAngle = Mathf.Lerp(0f, rawAngle, 0.1f);

        selectedObject.transform.Rotate(Vector3.up, smoothAngle);
        messageController?.ShowInfoMessage($"[ROT] {selectedObject.name} rotado ({smoothAngle:F1}°)");
    }

    private void MoveSelectedObject(Vector2 screenPosition)
    {
        if (raycastManager == null) return;

        rayHits.Clear();
        if (raycastManager.Raycast(screenPosition, rayHits, TrackableType.PlaneWithinPolygon) && rayHits.Count > 0)
        {
            Pose hitPose = rayHits[0].pose;

            // Solo actualizar posición, conservar rotación actual
            selectedObject.transform.SetPositionAndRotation(hitPose.position, selectedObject.transform.rotation);

            messageController?.ShowInfoMessage($"[MOV] {selectedObject.name} movido.");
        }
    }
}
