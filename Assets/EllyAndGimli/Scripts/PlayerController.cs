using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isJumpPressed;
    private bool isGrounded;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputActions = new InputSystem_Actions();
    }
    
    private void OnEnable()
    {
        inputActions.Enable(); // Включаем систему ввода
    }

    private void OnDisable()
    {
        inputActions.Disable(); // Отключаем систему ввода при выключении объекта
    }

    private void Update()
    {
        isJumpPressed = inputActions.Player.Jump.IsPressed();
        Debug.Log(isJumpPressed);
        // Проверка нажатия прыжка
        if (isJumpPressed && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumpPressed = false;
        }
    }

    private void FixedUpdate()
    {
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        Debug.Log(moveInput);
        // Движение
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        // Проверка, на земле ли игрок
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}