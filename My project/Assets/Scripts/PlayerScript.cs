using UnityEngine;
using UnityEngine.Events;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript Instance;

    [System.Serializable]
    public class PlayersMovementEvents
    {
        public UnityEvent onPlayerMove;
        public UnityEvent onPlayerFlip;
    }
    [SerializeField]
    private AudioClip lossSound;

    [SerializeField]
    private AudioClip winSound;

    [SerializeField]
    private bool isButtonPressed = false;

    [HideInInspector] public bool facingRight = true;
    public float moveSpeed; // Скорость движения.
    public float accelerationMultiplier = 2f; // Множитель ускорения
    public GameObject sight;
    public PlayersMovementEvents playerMovementEvents;
    public AudioClip footstepSound; // Звук шагов
    public float footstepDelay = 0.5f; // Задержка между шагами
    public float footstepDelayNormal = 0.02f; // Задержка между шагами при обычной скорости
    public float footstepDelayFast = 0f;
    private Rigidbody2D rig;
    private Animator anim;
    private SpriteRenderer sprRend;
    private UltimateJoystick joystick;
    private bool playerControlEnabled = true;

    private Vector2 moveDirection; // Направление движения
    private bool isShiftPressed = false; // Переменная для отслеживания состояния клавиши Shift

    private AudioSource audioSource; // Источник звука

    void Awake()
    {
        PlayerScript.Instance = this;
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        sprRend = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>(); // Получаем компонент AudioSource
    }

    void Update()
    {
        if (!playerControlEnabled) return;

        float horizontalInputJoystick = UltimateJoystick.GetHorizontalAxis("Movement");
        float verticalInputJoystick = UltimateJoystick.GetVerticalAxis("Movement");

        float horizontalInputKeyboard = 0f;
        float verticalInputKeyboard = 0f;

        // Обработка ввода с клавиатуры (WASD)
        if (Input.GetKey(KeyCode.W))
        {
            verticalInputKeyboard += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            verticalInputKeyboard -= 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            horizontalInputKeyboard -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            horizontalInputKeyboard += 1f;
        }

        // Проверка нажатия клавиши Shift для ускорения
        isShiftPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Объединение ввода с джойстика и клавиатуры
        moveDirection = new Vector2(horizontalInputJoystick + horizontalInputKeyboard, verticalInputJoystick + verticalInputKeyboard).normalized;

        if (moveDirection.x < 0)
        {
            sprRend.flipX = true;
            playerMovementEvents.onPlayerFlip.Invoke();
        }
        else if (moveDirection.x > 0)
        {
            sprRend.flipX = false;
            playerMovementEvents.onPlayerFlip.Invoke();
        }

        Animating(moveDirection.x, moveDirection.y);
    }

    public void OnButtonDown()
    {
        isButtonPressed = true;
    }

    public void OnButtonUp()
    {
        isButtonPressed = false;
    }

    void FixedUpdate()
    {
        if (!playerControlEnabled) return;

        float currentSpeed = moveSpeed;
        float currentFootstepDelay = footstepDelayNormal; // Инициализация текущей задержки между шагами

        if (isButtonPressed != isShiftPressed) // Ускорение применяется только если одно из условий истинно, но не оба
        {
            currentSpeed *= accelerationMultiplier;
            currentFootstepDelay = footstepDelayFast; // Устанавливаем нулевую задержку при ускорении
        }

        rig.MovePosition(rig.position + moveDirection * currentSpeed * Time.fixedDeltaTime);

        playerMovementEvents.onPlayerMove.Invoke();

        // Воспроизведение звуков шагов
        if (moveDirection.magnitude > 0 && audioSource != null && audioSource.enabled && footstepSound != null && !audioSource.isPlaying)
        {
            audioSource.clip = footstepSound;
            audioSource.PlayDelayed(currentFootstepDelay); // Используем текущую задержку между шагами
        }
    }

    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }
    void PlayLossSound()
    {
        if (lossSound != null && audioSource != null && audioSource.enabled)
        {
            audioSource.clip = lossSound;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Loss sound or audio source is not set or enabled!");
        }
    }

    void PlayWinSound()
    {
        if (winSound != null && audioSource != null && audioSource.enabled)
        {
            audioSource.clip = winSound;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Loss sound or audio source is not set or enabled!");
        }
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Enemy")
        {
            // Воспроизводим звук проигрыша
            PlayLossSound();

            Health.instance.PlayerDied();
        }

        if (target.tag == "Goal")
        {
            PlayWinSound();
            GameManager.instance.PlayerReachedGoal();
        }
    }

    public void DisablePlayerControl()
    {
        playerControlEnabled = false;
    }

    public void EnablePlayerControl()
    {
        playerControlEnabled = true;
    }
}