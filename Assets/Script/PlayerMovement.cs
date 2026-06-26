using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Gerakan & Lompat")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    public float jumpStartDelay = 0.1f;
    public float landRecoveryTime = 0.15f;
    public bool hasGun = false;

    [Header("Shooting")]
    public GameObject bulletPrefab; // Tarik prefab peluru ke sini
    public Transform firePoint;     // Tarik objek FirePoint ke sini

    private Rigidbody2D rb;
    private Animator anim;
    private float moveInput;
    private bool isGrounded;
    private bool canMove = true;

    [HideInInspector] public Vector2 lastSafePosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        lastSafePosition = transform.position; // Set posisi awal sebagai tempat aman
    }

    void Update()
    {

        if (Time.timeScale == 0) return;

        if (!canMove) { 
            moveInput = 0; 
            anim.SetBool("isWalking", false);
            return; 
        }

        // 1. Input Gerakan
        moveInput = Input.GetAxisRaw("Horizontal");
        anim.SetBool("isWalking", moveInput != 0);

        // 2. Membalik Arah (Flip)
        // Kita pakai localScale supaya FirePoint ikut berputar arahnya
        if (moveInput > 0) transform.localScale = new Vector3(0.1533f, 0.1557f, 1);
        else if (moveInput < 0) transform.localScale = new Vector3(-0.1533f, 0.1557f, 1);

        // 3. Input Lompat
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            StartCoroutine(JumpSequence());
        }

        // 4. Input Menembak
        if (Input.GetKeyDown(KeyCode.O) && hasGun)
        {
            anim.SetTrigger("shootTrigger"); // Pastikan nama trigger di Animator sama
            Shoot();
            StartCoroutine(LockMovementForShooting()); 
        }

        // 5. Update Animasi Fisika
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    IEnumerator LockMovementForShooting()
    {
        canMove = false;
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(0.3f); // Sesuaikan dengan durasi animasi shooting
        canMove = true;
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    void Shoot()
    {
        // Munculkan peluru di posisi FirePoint
        if(bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    IEnumerator JumpSequence()
    {
        canMove = false;
        rb.linearVelocity = Vector2.zero;
        anim.SetTrigger("startJump");
        yield return new WaitForSeconds(jumpStartDelay);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        canMove = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetTrigger("endJump");
            StartCoroutine(LandSequence());
            lastSafePosition = transform.position; // Simpan posisi aman terakhir
        }
    }

    IEnumerator LandSequence()
    {
        canMove = false;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        yield return new WaitForSeconds(landRecoveryTime);
        canMove = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}