using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public KeyCode skill1Key;
    public KeyCode skill2Key;

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectile;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement.isPlayer1)
        {
            skill1Key = KeyCode.K;
            skill2Key = KeyCode.L;
        }
        else
        {
            skill1Key = KeyCode.Alpha2;
            skill2Key = KeyCode.Alpha3;
        }
    }

    void Update()
    {
        HandleSkills();
    }

    void HandleSkills()
    {
        if (Input.GetKeyDown(skill1Key))
        {
            animator.SetTrigger("Skill1");
            projectile.transform.position = firePoint.position;
            projectile.GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));

        }
        else if (Input.GetKeyDown(skill2Key)) animator.SetTrigger("Skill2");
    }
}
