using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Referencia al transform del personaje
    public float rotationSpeed = 1f; // Velocidad de rotaci�n de la c�mara con el mouse
    public Vector3 offsetPosition = new Vector3(0f, 2f, -4f); // Offset de posici�n de la c�mara respecto al personaje
    public Vector3 lookOffset = new Vector3(0f, 1f, 0f); // Offset de direcci�n donde mira la c�mara respecto al personaje
    public float minYAngle = -30f; // �ngulo m�nimo en Y de la c�mara
    public float maxYAngle = 80f; // �ngulo m�ximo en Y de la c�mara

    private float currentXAngle = 0f; // �ngulo actual en X de la c�mara
    private float currentYAngle = 0f; // �ngulo actual en Y de la c�mara

    void LateUpdate()
    {
        // Obtener la posici�n del mouse en la pantalla
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // Actualizar los �ngulos actuales seg�n la entrada del mouse
        currentXAngle += mouseX;
        currentYAngle -= mouseY;
        currentYAngle = Mathf.Clamp(currentYAngle, minYAngle, maxYAngle);

        // Rotar la c�mara alrededor del personaje
        Quaternion rotation = Quaternion.Euler(currentYAngle, currentXAngle, 0);
        Vector3 newPosition = rotation * offsetPosition + target.position;
        transform.position = newPosition;

        // Aplicar el offset de direcci�n donde mira la c�mara
        Vector3 lookPosition = target.position + lookOffset;

        // Aplicar la rotaci�n y mirar hacia la posici�n del lookOffset
        transform.rotation = rotation;
        transform.LookAt(lookPosition);
    }
}
