using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;
    private Rigidbody rb;
    private Vector3 movePlayer;
    private bool isGrounded;

    public Animator animator;
    public float jumpForce = 10f;

    private int puntaje;
    public TextMeshProUGUI puntajeText;

    public Camera mainCamera; // Referencia a la cámara principal

    public AudioClip soundClip; // Clip de sonido de caminar
    public AudioClip PointsSoundClip; // Clip de sonido de salto
    public AudioClip SongClip;
    private AudioSource audioSource; // Referencia al componente AudioSource
    private AudioSource PointSource;
    private AudioSource SongSource;

    private bool isWalking; // Variable para determinar si el jugador está caminando

    private HealthController playerHealthController; // Referencia al HealthController del jugador

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        puntaje = 0;
        UpdateScoreText();
        AudioSource[] audioSources = GetComponents<AudioSource>();

        if (audioSources.Length >= 4)
        {
            audioSource = audioSources[0];
            PointSource = audioSources[1];
            SongSource = audioSources[3];
        }

        audioSource.clip = soundClip;
        PointSource.clip = PointsSoundClip;
        SongSource.clip = SongClip;
        SongSource.loop = true;

        if (SongSource != null)
        {
            SongSource.Play();
        }

        audioSource.loop = true; // Reproducir en loop mientras camina

        // Obtener la referencia al HealthController del jugador
        playerHealthController = GetComponent<HealthController>();
    }

    void Update()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        Vector3 camForward = mainCamera.transform.forward;
        Vector3 camRight = mainCamera.transform.right;
        camForward.y = 0; // Mantener el movimiento en el plano horizontal
        camRight.y = 0;
        camForward = camForward.normalized;
        camRight = camRight.normalized;

        Vector3 playerInput = horizontalMove * camRight + verticalMove * camForward;
        playerInput = Vector3.ClampMagnitude(playerInput, 1);

        movePlayer = playerInput;

        isWalking = playerInput.magnitude > 0;

        if (movePlayer != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(movePlayer);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * 10f);
        }

        animator.SetBool("isWalking", isWalking);

        if (isWalking && isGrounded)
        {
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource != null)
            {
                audioSource.Stop();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movePlayer * playerSpeed * Time.fixedDeltaTime);
    }

    void Jump()
    {
        animator.SetTrigger("JumpTrigger");
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
    }

    void Heal(int healAmount)
    {
        // Curar al jugador usando la referencia al HealthController del jugador
        if (playerHealthController != null)
        {
            playerHealthController.Heal(healAmount);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Soul"))
        {
            puntaje++; 
            Heal(10); // Curar al jugador por 30 puntos

            if (PointSource != null)
            {
                PointSource.Play();
            }

            UpdateScoreText();
            Destroy(collision.gameObject);
        }
    }

    void UpdateScoreText()
    {
        if (puntajeText != null)
        {
            puntajeText.text = puntaje.ToString();
        }
    }
}
