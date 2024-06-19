
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public int maxHealth = 100; // Salud m�xima del jugador
    public float invulnerabilityDuration = 3f; // Duraci�n de la invulnerabilidad despu�s de recibir da�o

    private int currentHealth; // Salud actual del jugador
    private bool isInvulnerable = false; // Indica si el jugador est� en estado de invulnerabilidad
    private float invulnerabilityTimer = 0f; // Temporizador para la invulnerabilidad
    public AudioClip deathSoundClip; // Clip de sonido para la muerte del enemigo
    private AudioSource HitSource;

    public Image healthBar; // Referencia a la barra de vida en la UI

    void Start()
    {
        currentHealth = maxHealth; // Inicializar la salud actual al m�ximo al inicio
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
        // Si el jugador est� en estado de invulnerabilidad, reducir el tiempo restante
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
        // Si el jugador no est� en estado de invulnerabilidad, recibir da�o
        if (!isInvulnerable)
        {
            currentHealth -= damageAmount;

            if (deathSoundClip != null)
            {
                HitSource.Play();
            }

            // Actualizar la barra de vida en la UI solo si est� activada
            if (healthBar != null && healthBar.gameObject.activeSelf)
            {
                UpdateHealthBar();
            }

            // Si la salud actual del jugador es menor o igual a 0, puede que quieras ejecutar un m�todo de muerte o desactivar al jugador
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
        // Si el jugador no est� en estado de invulnerabilidad, recibir da�o
        currentHealth += HealAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }


        // Actualizar la barra de vida en la UI solo si est� activada
        if (healthBar != null && healthBar.gameObject.activeSelf)
        {
            UpdateHealthBar();
        }
    }

    void UpdateHealthBar()
    {
        // Asegurarse de que la barra de vida exista y que la salud actual y m�xima sean v�lidas
        if (healthBar != null && maxHealth > 0)
        {
            // Calcular el porcentaje de salud actual respecto a la m�xima
            float healthPercent = (float)currentHealth / (float)maxHealth;

            // Actualizar el tama�o de la barra de vida en la UI
            healthBar.fillAmount = healthPercent;
        }
    }

    public bool IsInvulnerable()
    {
        return isInvulnerable;
    }

    void Die()
    {
        // Aqu� puedes implementar l�gica de muerte del jugador, como reiniciar el nivel o mostrar una pantalla de game over
        Debug.Log("Player died!");

        // Por ejemplo:
        // SceneManager.LoadScene("GameOverScene");
        // gameObject.SetActive(false); // Desactivar al jugador
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

    }
}
