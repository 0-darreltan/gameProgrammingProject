using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Panggil fungsi ini untuk menyerang
    public void Attack()
    {
        anim.SetTrigger("attackTrigger");
    }

    // Panggil fungsi ini untuk mengubah status gerak
    public void SetMoving(bool isMoving)
    {
        anim.SetBool("isMoving", isMoving);
    }
}