using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement instance;
    [HideInInspector] public bool PlayerIsInteracting = false; 
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
    public Transform RespawnPos;
    [Header("Animations")]
    public string IdleName;
    public string RunName;
    public string JumpStartName;
    public string JustLandedName;
    public Animator anim;
    [Header("SFX")]
    public AudioClip JumpSFX;
    public AudioClip WalkSFX;
    public AudioClip PlayerFallSFX;
    public AudioClip HitGroundSFX;
    private void Awake()
    {
        instance = this;
        controlls = new MainControlls();
        controlls.Player.Movement.performed += ctx => HorizontalInput = ctx.ReadValue<float>();
        controlls.Player.Movement.canceled += ctx => HorizontalInput = 0;
        controlls.Player.Jump.performed += ctx => OnJumpPressd();
    }
    void OnJumpPressd()
    {
        if (CurrentjumpAmount < 1) { return; }
        SfxCreator.instance.PlaySound(JumpSFX, .35f);
        fallingAfterJump = true;
        anim.Play(JumpStartName);
        rb.velocity = new Vector2(rb.velocity.x, CurrentJumpHeight);
        CurrentjumpAmount--;
        isTouchingGround = false;
    }
    bool fallingAfterJump = false;
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
        canPlayHitOnGroundSFX = true;
    }
    bool canPlayHitOnGroundSFX = true;
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(HorizontalInput * CurrentSpeed * Time.deltaTime, rb.velocity.y);
        isTouchingGround = Physics2D.Raycast(groundChecker.transform.position, Vector2.down, CheckerSize, GroundLayer);
        if (isTouchingGround)
        {
            CurrentjumpAmount = jumpAmount;
            if (canPlayHitOnGroundSFX)
            {
                SfxCreator.instance.PlaySound(HitGroundSFX, .25f);
                canPlayHitOnGroundSFX = false;
            }
            if (fallingAfterJump && rb.velocity.y < .5f)
            {
                anim.Play(JustLandedName);
            }
        }
        if (!isTouchingGround)
        {
            canPlayHitOnGroundSFX = true;
        }
        if (!isTouchingGround) { CurrentJumpHeight = InAirJumpForce; } else { CurrentJumpHeight = JumpHeight; }
    }
    float LastUpdatedFlip = 0;
    public void OnEndLandingAnimation()
    {
        fallingAfterJump = false;
    }
    private void Update()
    {
        if (!fallingAfterJump) { if (HorizontalInput != 0) { anim.Play(RunName); } else { anim.Play(IdleName); } }
        if (transform.position.y < -10f) { transform.position = RespawnPos.position; SfxCreator.instance.PlaySound(PlayerFallSFX); }
        
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
