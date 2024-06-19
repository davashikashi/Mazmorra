using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damageAmount = 10; // Cantidad de daño que inflige el jugador
    public string enemyTag = "Enemy"; // Tag para identificar a los enemigos

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            // Si el objeto con el que colisionamos es un enemigo
            Debug.Log("Enemigo golpeado");
            InflictDamage(other.gameObject);
        }
    }

    void InflictDamage(GameObject enemy)
    {
        // Encontrar el componente HealthController en el enemigo
        HealthEnemyController enemyHealth = enemy.GetComponent<HealthEnemyController>();
        if (enemyHealth != null)
        {
            // Reducir la salud del enemigo
            enemyHealth.TakeDamage(damageAmount);
        }
    }
}
