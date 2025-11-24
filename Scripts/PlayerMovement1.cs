using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoviment1 : MonoBehaviour
{
    [Header("Components")]
    private CharacterController controller2;
    private Transform myCamera;
    private Animator animator2;
    [SerializeField] private Transform foot;
    [SerializeField] private LayerMask colisionLayer;

    [Header("Variables")]
    public float velocity = 5f;
    private bool isGround;
    private float yForce;

    void Start()
    {

        controller2 = GetComponent<CharacterController>();
        myCamera = Camera.main.transform;
        animator2 = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();

    }

    public void Move()
    {
        Debug.Log("Executando o movimento do personagem...");

        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontal -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontal += 1f;

        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) vertical -= 1f;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) vertical += 1f;

        Vector3 movimento2 = new Vector3(horizontal, 0, vertical);

        movimento2 = Vector3.ClampMagnitude(movimento2, 1f); // Normaliza a velocidade diagonal de movimento
        movimento2 = myCamera.TransformDirection(movimento2);
        movimento2.y = 0; // Mantém o movimento no plano horizontal

        controller2.Move(movimento2 * velocity * Time.deltaTime); // Aplica o movimento ao CharacterController 
        if (movimento2 != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation, Quaternion.LookRotation(movimento2),
                Time.deltaTime * 10f
             );
        }

        animator2.SetBool("Mover", movimento2 != Vector3.zero);
        isGround = Physics.CheckSphere(foot.position, 0.3f, colisionLayer);
        //Botar parametro isGround no animator
        animator2.SetBool("isGround", isGround);
    }

    public void Jump()
    {
        Debug.Log("Estou no chão?" + isGround);

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGround)
        {
            yForce = 5f;
            animator2.SetTrigger("Jump");

        }

        if (yForce > -9.81f)
        {
            yForce += -9 * Time.deltaTime;
        }

        controller2.Move(new Vector3(0, yForce, 0) * Time.deltaTime); // Aplica a gravidade ao CharacterController

    }

}
