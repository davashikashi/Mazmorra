using UnityEngine;

public class HeadController : MonoBehaviour
{
    public int damageAmount = 10; // Cantidad de daño que inflige al jugador

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Verificar si el jugador está en estado de invulnerabilidad
            HealthController healthController = other.GetComponent<HealthController>();
            if (healthController != null && !healthController.IsInvulnerable())
            {
                // El enemigo ha colisionado como trigger con el jugador y puede infligir daño
                InflictDamage(other.gameObject);
            }
        }
    }

    void InflictDamage(GameObject playerObj)
    {
        // Aquí puedes implementar la lógica para infligir daño al jugador
        // Por ejemplo, puedes reducir la salud del jugador
        Debug.Log("Inflicting damage to player!");

        // Ejemplo básico: Reducir la salud del jugador si tiene un script de salud
        HealthController healthController = playerObj.GetComponent<HealthController>();
        if (healthController != null)
        {
            healthController.TakeDamage(damageAmount);
        }
    }
}
