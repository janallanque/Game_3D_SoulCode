using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private CharacterController controller;
    private Transform myCamera;
    private Animator animator;
    [SerializeField] private Transform foot;
    [SerializeField] private LayerMask colisionLayer;


    [Header("Movement Variables")]
    public float velocity = 5f;
    public float rotationSpeed = 10f;


    [Header("Physics & Jump")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.5f; // Altura do pulo
    [SerializeField] private float groundCheckRadius = 0.3f;


    private Vector3 playerVelocity; // Unifica a força vertical (gravidade e pulo)
    private bool isGround;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        myCamera = Camera.main.transform;
        animator = GetComponent<Animator>();
        
    }

    void Update()
    {
        // A ordem aqui é importante:
        // 1. Checar se está no chão
        isGround = Physics.CheckSphere(foot.position, groundCheckRadius, colisionLayer);

        // 2. Aplicar gravidade e pulo (lógica vertical)
        HandleGravityAndJump();

        // 3. Mover o personagem (lógica horizontal)
        Move();
    }

    void HandleGravityAndJump()
    {
        // Se o personagem está no chão e sua velocidade vertical é negativa...
        if (isGround && playerVelocity.y < 0)
        {
            // Força o personagem a ficar no chão.
            playerVelocity.y = -2f;
        }

        // Checa por input de pulo
        if (isGround && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // Calcula a força necessária para atingir a altura de pulo desejada
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Aplica a aceleração da gravidade ao longo do tempo
        playerVelocity.y += gravity * Time.deltaTime;

        // Move o personagem na vertical usando a velocidade calculada
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Move()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontal -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontal += 1f;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) vertical += 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) vertical -= 1f;

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Atualiza a animação
        animator.SetBool("Mover", direction.magnitude >= 0.1f);

        // Só executa a lógica de movimento se houver input do jogador
        if (direction.magnitude >= 0.1f)
        {
            // Calcula o ângulo alvo baseado na direção da câmera
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + myCamera.eulerAngles.y;

            // Cria a direção de movimento a partir do ângulo
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Move o personagem na direção correta
            controller.Move(moveDir.normalized * velocity * Time.deltaTime);

            // Gira o personagem suavemente para a direção em que está se movendo
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(moveDir.normalized),
                Time.deltaTime * rotationSpeed
                );
        }
        animator.SetBool("Mover", movimento!= Vector3.zero);
        isGround = Physics.CheckSphere(foot.position, 0.3f, colisionLayer);
        animator.SetBool("isGround", isGround);
    }



    public void Jump()
    {
        Debug.Log("Estou no chão: " + isGround);)
        if (keyboard.current.spaceKey.wasPressedThisFrame && isGround))
        {
            yForce = 5f;
            animator.SetTrigger("Jump");
        }

        if (yForce > -9.81f)
        {
            yForce += gravity * Time.deltaTime;
        }
    
    }
}