using UnityEngine;

public class Lava : MonoBehaviour
{
    public int damageAmount = 100; // Cantidad de daño que inflige al jugador

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HealthController healthController = other.gameObject.GetComponent<HealthController>();
            if (healthController != null && !healthController.IsInvulnerable())
            {
                InflictDamage(other.gameObject);
            }
        }
    }

    void InflictDamage(GameObject playerObj)
    {
        Debug.Log("Inflicting damage to player!");

        HealthController healthController = playerObj.GetComponent<HealthController>();
        if (healthController != null)
        {
            healthController.TakeDamage(damageAmount);
        }
    }
}
