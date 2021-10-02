using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CharacterMovement : MonoBehaviour
{
    public float MoveSpeed;
    public float JumpHeight;
    float CurrentSpeed;
    float CurrentJumpHeight;
    float HorizontalInput;
    Rigidbody2D rb;
    MainControlls controlls;
    bool isTouchingGround;
    public Transform groundChecker;
    public float CheckerSize;
    public LayerMask GroundLayer;
    public int jumpAmount;
    public float InAirJumpForce;
    int CurrentjumpAmount;
    public bool FacingRight;
    bool CurFaceRight;
    public Transform ToFlip;
    private void Awake()
    {
        controlls = new MainControlls();
        controlls.Player.Movement.performed += ctx => HorizontalInput = ctx.ReadValue<float>();
        controlls.Player.Movement.canceled += ctx => HorizontalInput = 0;
        controlls.Player.Jump.performed += ctx => OnJumpPressd();
    }
    void OnJumpPressd()
    {
        if (CurrentjumpAmount < 1) { return; }
        Debug.Log("PressedJump");
        rb.velocity = new Vector2(rb.velocity.x, CurrentJumpHeight);
        CurrentjumpAmount--;
        isTouchingGround = false;
    }
    private void OnEnable()
    {
        controlls.Enable();
    }
    private void OnDisable()
    {
        controlls.Disable();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        CurrentjumpAmount = jumpAmount;
        CurrentSpeed = MoveSpeed;
        CurrentJumpHeight = JumpHeight;
        CurFaceRight = FacingRight;
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(HorizontalInput * CurrentSpeed * Time.deltaTime, rb.velocity.y);
        isTouchingGround = Physics2D.Raycast(groundChecker.transform.position, Vector2.down, CheckerSize, GroundLayer);
        if (isTouchingGround)
        {
            CurrentjumpAmount = jumpAmount;
        }
        if (!isTouchingGround) { CurrentJumpHeight = InAirJumpForce; } else { CurrentJumpHeight = JumpHeight; }
    }
    float LastUpdatedFlip = 0;
    private void Update()
    {
        if (transform.position.y < -10f) { transform.position = Vector3.zero; }
        
        if(CurFaceRight == false && HorizontalInput > 0)
        {
            Flip();
        }else if(CurFaceRight == true && HorizontalInput < 0)
        {
            Flip();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundChecker.transform.position, groundChecker.transform.position + Vector3.down * CheckerSize);
    }
    void Flip()
    {
        CurFaceRight = !CurFaceRight;
        Vector3 Scaler = ToFlip.localScale;
        Scaler.x *= -1;
        ToFlip.localScale = Scaler;
    }
}
