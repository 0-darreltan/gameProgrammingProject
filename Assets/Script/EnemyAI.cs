using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("Deteksi Player")]
    public Transform player;
    public float detectRange = 5f;
    public float attackRange = 1.5f;
    public float moveSpeed = 4f;

    [Header("Roaming")]
    public float roamRadius = 3f;
    public float minWaitTime = 1.5f;
    public float maxWaitTime = 4f;

    private Rigidbody2D rb;
    private Animator anim;

    private Vector2 startPosition;
    private Vector2 roamDestination;

    private float waitTimer;
    private bool isWaiting = true;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        startPosition = rb.position;
        waitTimer = Random.Range(minWaitTime, maxWaitTime);

        // Agar enemy tidak berputar saat bertabrakan
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (player == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null)
                player = obj.transform;
        }

        if (player == null)
        {
            RoamBehavior();
            return;
        }

        float distance = Vector2.Distance(rb.position, player.position);

        if (distance <= attackRange)
        {
            Attack();
        }
        else if (distance <= detectRange)
        {
            ChasePlayer();
        }
        else
        {
            RoamBehavior();
        }
    }

    void ChasePlayer()
    {
        
        anim.SetBool("isMoving", true);

        Vector2 nextPos = Vector2.MoveTowards(
            rb.position,
            player.position,
            moveSpeed * Time.deltaTime);

        rb.MovePosition(nextPos);

        FlipSprite(player.position);

        isWaiting = true;
    }

    void Attack()
    {
        
        anim.SetBool("isMoving", false);
        anim.SetTrigger("attackTrigger");
    }

    void RoamBehavior()
    {
        if (isWaiting)
        {
            
            anim.SetBool("isMoving", false);

            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0)
            {
                isWaiting = false;
                PickRandomDestination();
            }
        }
        else
        {
            
            anim.SetBool("isMoving", true);

            Vector2 nextPos = Vector2.MoveTowards(
                rb.position,
                roamDestination,
                moveSpeed * Time.deltaTime);

            rb.MovePosition(nextPos);

            FlipSprite(roamDestination);

            if (Vector2.Distance(rb.position, roamDestination) < 0.1f)
            {
                isWaiting = true;
                waitTimer = Random.Range(minWaitTime, maxWaitTime);
            }
        }
    }

    void PickRandomDestination()
    {
        float randomX = Random.Range(-roamRadius, roamRadius);

        roamDestination = startPosition + new Vector2(randomX, 0);
    }

    void FlipSprite(Vector2 target)
    {
        if (target.x > rb.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else if (target.x < rb.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Enemy menabrak : " + collision.gameObject.name);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green;

        if (Application.isPlaying)
            Gizmos.DrawWireSphere(startPosition, roamRadius);
        else
            Gizmos.DrawWireSphere(transform.position, roamRadius);
    }
}