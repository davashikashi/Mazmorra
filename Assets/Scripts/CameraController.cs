using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Referencia al transform del personaje
    public float rotationSpeed = 1f; // Velocidad de rotación de la cámara con el mouse
    public Vector3 offsetPosition = new Vector3(0f, 2f, -4f); // Offset de posición de la cámara respecto al personaje
    public Vector3 lookOffset = new Vector3(0f, 1f, 0f); // Offset de dirección donde mira la cámara respecto al personaje
    public float minYAngle = -30f; // Ángulo mínimo en Y de la cámara
    public float maxYAngle = 80f; // Ángulo máximo en Y de la cámara

    private float currentXAngle = 0f; // Ángulo actual en X de la cámara
    private float currentYAngle = 0f; // Ángulo actual en Y de la cámara

    void LateUpdate()
    {
        // Obtener la posición del mouse en la pantalla
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // Actualizar los ángulos actuales según la entrada del mouse
        currentXAngle += mouseX;
        currentYAngle -= mouseY;
        currentYAngle = Mathf.Clamp(currentYAngle, minYAngle, maxYAngle);

        // Rotar la cámara alrededor del personaje
        Quaternion rotation = Quaternion.Euler(currentYAngle, currentXAngle, 0);
        Vector3 newPosition = rotation * offsetPosition + target.position;
        transform.position = newPosition;

        // Aplicar el offset de dirección donde mira la cámara
        Vector3 lookPosition = target.position + lookOffset;

        // Aplicar la rotación y mirar hacia la posición del lookOffset
        transform.rotation = rotation;
        transform.LookAt(lookPosition);
    }
}
