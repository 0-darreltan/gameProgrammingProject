using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings Gerakan")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    
    private float moveInput;
    private bool isGrounded;

    void Start()
    {
        // Mengambil semua komponen yang dibutuhkan dari karakter
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Ambil input kiri/kanan (A/D atau Panah)
        moveInput = Input.GetAxisRaw("Horizontal");

        // 2. Kontrol Animasi
        // Jika moveInput tidak 0 (artinya kita tekan tombol), maka isWalking jadi true
        if (moveInput != 0) {
            anim.SetBool("isWalking", true);
        } else {
            anim.SetBool("isWalking", false);
        }

        // 3. Membalik arah karakter (Flip)
        if (moveInput > 0) {
            spriteRenderer.flipX = false; // Hadap Kanan
        } else if (moveInput < 0) {
            spriteRenderer.flipX = true; // Hadap Kiri
        }

        // 4. Melompat (Tombol Spasi)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Unity 6 menggunakan linearVelocity
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        // Menjalankan gerakan fisik karakter
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    // Deteksi apakah kaki menyentuh lantai (agar tidak bisa lompat di udara)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}