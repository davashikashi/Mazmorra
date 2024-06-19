using UnityEngine;

public class FollowBone : MonoBehaviour
{
    public Transform bone; // El hueso que el collider debe seguir
    public Vector3 positionOffset; // Offset de posición
    public Vector3 rotationOffset; // Offset de rotación en grados

    void LateUpdate()
    {
        if (bone != null)
        {
            // Aplica el offset de posición y rotación
            transform.position = bone.position + bone.TransformDirection(positionOffset);

            // Aplica la rotación del hueso y el offset de rotación
            transform.rotation = bone.rotation * Quaternion.Euler(rotationOffset);
        }
    }
}
