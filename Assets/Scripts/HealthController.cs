
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public int maxHealth = 100; // Salud máxima del jugador
    public float invulnerabilityDuration = 3f; // Duración de la invulnerabilidad después de recibir daño

    private int currentHealth; // Salud actual del jugador
    private bool isInvulnerable = false; // Indica si el jugador está en estado de invulnerabilidad
    private float invulnerabilityTimer = 0f; // Temporizador para la invulnerabilidad
    public AudioClip deathSoundClip; // Clip de sonido para la muerte del enemigo
    private AudioSource HitSource;

    public Image healthBar; // Referencia a la barra de vida en la UI

    void Start()
    {
        currentHealth = maxHealth; // Inicializar la salud actual al máximo al inicio
        UpdateHealthBar();

        AudioSource[] audiosSources = GetComponents<AudioSource>();

        if (audiosSources.Length >= 4)
        {
            HitSource = audiosSources[2];
            
        }

        HitSource.clip = deathSoundClip;
        
    }

    void Update()
    {
        // Si el jugador está en estado de invulnerabilidad, reducir el tiempo restante
        if (isInvulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;

            // Si el tiempo de invulnerabilidad ha terminado, desactivar la invulnerabilidad
            if (invulnerabilityTimer <= 0)
            {
                isInvulnerable = false;
                invulnerabilityTimer = 0;
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        // Si el jugador no está en estado de invulnerabilidad, recibir daño
        if (!isInvulnerable)
        {
            currentHealth -= damageAmount;

            if (deathSoundClip != null)
            {
                HitSource.Play();
            }

            // Actualizar la barra de vida en la UI solo si está activada
            if (healthBar != null && healthBar.gameObject.activeSelf)
            {
                UpdateHealthBar();
            }

            // Si la salud actual del jugador es menor o igual a 0, puede que quieras ejecutar un método de muerte o desactivar al jugador
            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                // Activar la invulnerabilidad
                isInvulnerable = true;
                invulnerabilityTimer = invulnerabilityDuration;
            }
        }
    }

    public void Heal(int HealAmount)
    {
        // Si el jugador no está en estado de invulnerabilidad, recibir daño
        currentHealth += HealAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }


        // Actualizar la barra de vida en la UI solo si está activada
        if (healthBar != null && healthBar.gameObject.activeSelf)
        {
            UpdateHealthBar();
        }
    }

    void UpdateHealthBar()
    {
        // Asegurarse de que la barra de vida exista y que la salud actual y máxima sean válidas
        if (healthBar != null && maxHealth > 0)
        {
            // Calcular el porcentaje de salud actual respecto a la máxima
            float healthPercent = (float)currentHealth / (float)maxHealth;

            // Actualizar el tamaño de la barra de vida en la UI
            healthBar.fillAmount = healthPercent;
        }
    }

    public bool IsInvulnerable()
    {
        return isInvulnerable;
    }

    void Die()
    {
        // Aquí puedes implementar lógica de muerte del jugador, como reiniciar el nivel o mostrar una pantalla de game over
        Debug.Log("Player died!");

        // Por ejemplo:
        // SceneManager.LoadScene("GameOverScene");
        // gameObject.SetActive(false); // Desactivar al jugador
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

    }
}
