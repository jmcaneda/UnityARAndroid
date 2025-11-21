using UnityEngine;

public class WheelRotator : MonoBehaviour
{
    public float rotationSpeed = 360f; // grados por segundo
    public bool autoRotate = true;     // si debe girar automáticamente

    void Update()
    {
        if (autoRotate)
        {
            transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
        }
    }

    // Si quieres controlar la rotación manualmente desde otro script:
    public void RotateBy(float degrees)
    {
        transform.Rotate(Vector3.right, degrees);
    }
}
