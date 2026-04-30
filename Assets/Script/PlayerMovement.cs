using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings Gerakan")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Timing (Anticipation & Recovery)")]
    public float jumpStartDelay = 0.15f; // Jeda jumpStart (jongkok)
    public float landRecoveryTime = 0.2f; // Jeda jumpEnd (mendarat)

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    
    private float moveInput;
    private bool isGrounded;
    private bool canMove = true; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Jika sedang ancang-ancang atau mendarat, kunci gerakan
        if (!canMove) 
        {
            moveInput = 0;
            anim.SetBool("isWalking", false);
            return; 
        }

        // 2. Ambil Input Jalan
        moveInput = Input.GetAxisRaw("Horizontal");

        // 3. Atur Animasi Jalan & Flip
        anim.SetBool("isWalking", moveInput != 0);
        
        if (moveInput > 0) spriteRenderer.flipX = false;
        else if (moveInput < 0) spriteRenderer.flipX = true;

        // 4. Input Lompat
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            StartCoroutine(JumpSequence());
        }

        // 5. Update Parameter Animator
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    void FixedUpdate()
    {
        // Gerakkan karakter hanya jika canMove = true
        if (canMove)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    // --- PROSES LOMPAT (Antisipasi) ---
    IEnumerator JumpSequence()
    {
        canMove = false; 
        rb.linearVelocity = Vector2.zero; // Berhenti total saat jongkok
        
        anim.SetTrigger("startJump"); // Mainkan jumpStart
        
        yield return new WaitForSeconds(jumpStartDelay);
        
        // Meluncur ke atas
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        canMove = true; 
    }

    // --- DETEKSI MENDARAT ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Pastikan objek lantai di-tag "Ground"
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (!isGrounded) // Hanya panggil saat baru saja mendarat
            {
                isGrounded = true;
                anim.ResetTrigger("startJump"); // Bersihkan antrean trigger lama
                StartCoroutine(LandSequence());
            }
        }
    }

    IEnumerator LandSequence()
    {
        anim.SetTrigger("endJump"); // Mainkan jumpEnd
        
        canMove = false; // Kunci lari saat mendarat
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        yield return new WaitForSeconds(landRecoveryTime);
        
        canMove = true; // Bisa jalan lagi
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}