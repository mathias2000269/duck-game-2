using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlJugador : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("Movimiento")]
    [SerializeField] float playerSpeed;
    [HideInInspector] float horizontalInput;
    [SerializeField] bool isGrounded;

    [SerializeField][Range(0, 1)] float horizontalDampingWhenStopping;
    [SerializeField][Range(0, 1)] float horizontalDampingWhenTurning;
    [SerializeField][Range(0, 1)] float horizontalDampingWhenInAir;
    [SerializeField][Range(0, 1)] float horizontalDampingBasic;

    Vector2 movement;

    [Header("JUMP")]
    public float jumpForce;
    [SerializeField]
    [Range(0, 1)]
    float jumpCut;
    public float jumpFallMultiplier;

    [SerializeField] float jumpBufferLength;
    float jumpBufferCounter;
    [SerializeField] float coyoteTime;
    float coyoteTimeCounter;

    [SerializeField] bool canJump;

    private bool isJumping;

    [SerializeField] int extraJumps = 1;
    public int extraJumpsValue;
    [SerializeField] float airLinearDrag = 2.5f;

    [Header("Raycasts")]
    [SerializeField] Vector3 groundRaycastOffset;
    [SerializeField] float groundRaycastLength;

    [Header("LayerMasks")]
    [SerializeField] LayerMask groundLayer;




    #region EVENTS SUBS

    private void OnEnable()
    {
        InputManager.playerControls.Player.Saltar.performed += GetSaltoInput;
        InputManager.playerControls.Player.Saltar.canceled += JumpCut;
    }

    private void OnDisable()
    {
        InputManager.playerControls.Player.Saltar.performed -= GetSaltoInput;
        InputManager.playerControls.Player.Saltar.canceled -= JumpCut;
    }
    #endregion

    #region GETEO INPUTS
    void GetSaltoInput(InputAction.CallbackContext context)
    {
        if (context.performed && extraJumpsValue > 0)
        {
            extraJumpsValue--;
            Jump(Vector2.up);
            jumpBufferCounter = jumpBufferLength;
        }
    }
    void JumpCut(InputAction.CallbackContext context) // salto corto
    {
        /*if (rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCut);
        }*/
    }
    #endregion
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

    }

    private void GetMoveInput()
    {
        movement = InputManager.playerControls.Player.Movement.ReadValue<Vector2>();

        if (movement.x > 0.1f || movement.x < -0.1f)
        {
            horizontalInput = movement.x * playerSpeed;
        }
        else
        {
            horizontalInput = 0;
        }
    }

    private void FixedUpdate()
    {
        GetMoveInput();

        CheckCollisions();
        MoveCharacter();

        if (jumpBufferCounter > 0f && (coyoteTimeCounter > 0f || extraJumpsValue > 0))
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }




        if (isGrounded)
        {
            if (rb.velocity.y < 0)
            {
                extraJumpsValue = extraJumps;
                coyoteTimeCounter = coyoteTime;

            }
        }
        else
        {
            //ApplyAirLinearDrag();
            Fall();
            coyoteTimeCounter -= Time.fixedDeltaTime;
            if (rb.velocity.y < 0f) isJumping = false;
        }
    }

    private void MoveCharacter()
    {
        float velocidadHorizontal = rb.velocity.x;
        velocidadHorizontal += horizontalInput;

        if (Mathf.Abs(horizontalInput) < 0.01f && isGrounded) // si paramos
            velocidadHorizontal *= Mathf.Pow(1f - horizontalDampingWhenStopping, Time.fixedDeltaTime * 10f);
        else if (Mathf.Sign(horizontalInput) != Mathf.Sign(velocidadHorizontal) && isGrounded) // si cambiamos de direcci�n
            velocidadHorizontal *= Mathf.Pow(1f - horizontalDampingWhenTurning, Time.fixedDeltaTime * 10f);
        else if (!isGrounded) // si estoy en el aire
            velocidadHorizontal *= Mathf.Pow(1f - horizontalDampingWhenInAir, Time.fixedDeltaTime * 10f);
        else
            velocidadHorizontal *= Mathf.Pow(1f - horizontalDampingBasic, Time.fixedDeltaTime * 10f);

        rb.velocity = new Vector2(velocidadHorizontal, rb.velocity.y);
    }

    private void Jump(Vector2 direction)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(direction * jumpForce, ForceMode2D.Impulse);


        jumpBufferCounter = 0f;
        coyoteTimeCounter = 0f;
        isJumping = true;


    }

    void Fall() // mejoras en la caida
    {

        if (rb.velocity.y < 0) // Si estamos cayendo del salto, a�adimos multiplicador de gravedad
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (jumpFallMultiplier) * Time.fixedDeltaTime;

        }
        else if (rb.velocity.y > 0 && InputManager.playerControls.Player.Saltar.phase == InputActionPhase.Canceled) // si estamos a�n en subida del salto y ya hemos dejado de pulsar el bot�n, a�adimos multiplicador peque�o
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (jumpFallMultiplier / 2) * Time.fixedDeltaTime;

        }

    }

    #region COLLISIONS
    private void CheckCollisions()
    {
        //Ground Collisions
        isGrounded = Physics2D.Raycast(transform.position + groundRaycastOffset, Vector2.down, groundRaycastLength, groundLayer) ||
                     Physics2D.Raycast(transform.position - groundRaycastOffset, Vector2.down, groundRaycastLength, groundLayer);

        /*//Corner Collisions
        canCornerCorrect = Physics2D.Raycast(transform.position + edgeRaycastOffset, Vector2.up, topRaycastLength, cornerCorrectLayer) &&
                           !Physics2D.Raycast(transform.position + innerRaycastOffset, Vector2.up, topRaycastLength, cornerCorrectLayer) ||
                           Physics2D.Raycast(transform.position - edgeRaycastOffset, Vector2.up, topRaycastLength, cornerCorrectLayer) &&
                           !Physics2D.Raycast(transform.position - innerRaycastOffset, Vector2.up, topRaycastLength, cornerCorrectLayer);

        //Wall Collisions
        onWall = Physics2D.Raycast(transform.position, Vector2.right, wallRaycastLength, wallLayer) ||
                 Physics2D.Raycast(transform.position, Vector2.left, wallRaycastLength, wallLayer);
        onRightWall = Physics2D.Raycast(transform.position, Vector2.right, wallRaycastLength, wallLayer);*/
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (isGrounded)
        {
            Gizmos.color = Color.green;
        }
        //Ground Check
        Gizmos.DrawLine(transform.position + groundRaycastOffset, transform.position + groundRaycastOffset + Vector3.down * groundRaycastLength);
        Gizmos.DrawLine(transform.position - groundRaycastOffset, transform.position - groundRaycastOffset + Vector3.down * groundRaycastLength);

        /*//Corner Check
        Gizmos.DrawLine(transform.position + edgeRaycastOffset, transform.position + edgeRaycastOffset + Vector3.up * topRaycastLength);
        Gizmos.DrawLine(transform.position - edgeRaycastOffset, transform.position - edgeRaycastOffset + Vector3.up * topRaycastLength);
        Gizmos.DrawLine(transform.position + innerRaycastOffset, transform.position + innerRaycastOffset + Vector3.up * topRaycastLength);
        Gizmos.DrawLine(transform.position - innerRaycastOffset, transform.position - innerRaycastOffset + Vector3.up * topRaycastLength);

        //Corner Distance Check
        Gizmos.DrawLine(transform.position - innerRaycastOffset + Vector3.up * topRaycastLength,
                        transform.position - innerRaycastOffset + Vector3.up * topRaycastLength + Vector3.left * topRaycastLength);
        Gizmos.DrawLine(transform.position + innerRaycastOffset + Vector3.up * topRaycastLength,
                        transform.position + innerRaycastOffset + Vector3.up * topRaycastLength + Vector3.right * topRaycastLength);

        //Wall Check
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * wallRaycastLength);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * wallRaycastLength);*/


    }
    #endregion

}
