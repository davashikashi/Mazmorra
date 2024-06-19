using UnityEngine;

public class FollowBone : MonoBehaviour
{
    public Transform bone; // El hueso que el collider debe seguir
    public Vector3 positionOffset; // Offset de posici�n
    public Vector3 rotationOffset; // Offset de rotaci�n en grados

    void LateUpdate()
    {
        if (bone != null)
        {
            // Aplica el offset de posici�n y rotaci�n
            transform.position = bone.position + bone.TransformDirection(positionOffset);

            // Aplica la rotaci�n del hueso y el offset de rotaci�n
            transform.rotation = bone.rotation * Quaternion.Euler(rotationOffset);
        }
    }
}
