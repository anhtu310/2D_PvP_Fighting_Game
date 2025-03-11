using System.Collections;
using UnityEngine;

public class C6 : MonoBehaviour
{
    public KeyCode skill1Key = KeyCode.K;
    public KeyCode skill2Key = KeyCode.L;
    public bool isPlayer1 = false;

    [SerializeField] private Transform firePoint1;
    [SerializeField] private Transform firePoint2;
    [SerializeField] private GameObject C6_1Prefab;
    [SerializeField] private GameObject C6_2Prefab;

    private Animator animator;
    private bool isSkill1Queued;
    private bool isSkill2Queued;

    void Start()
    {
        animator = GetComponent<Animator>();
        InitializeKeys();
    }

    void InitializeKeys()
    {
        if (CompareTag("Player1"))
        {
            isPlayer1 = true;
            skill1Key = KeyCode.K;
            skill2Key = KeyCode.L;

        }
        else if (CompareTag("Player2"))
        {
            isPlayer1 = false;
            skill1Key = KeyCode.Keypad2;
            skill2Key = KeyCode.Keypad3;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(skill1Key))
        {
            QueueSkill("Skill1", ref isSkill1Queued);
        }
        else if (Input.GetKeyDown(skill2Key))
        {
            QueueSkill("Skill2", ref isSkill2Queued);
        }
    }

    void QueueSkill(string skillTrigger, ref bool skillQueued)
    {
        animator.SetTrigger(skillTrigger);
        skillQueued = true;
    }

    public void SpawnSkill1() => HandleSkill(C6_1Prefab, firePoint1, ref isSkill1Queued, true);
    public void SpawnSkill2() => HandleSkill(C6_2Prefab, firePoint2, ref isSkill2Queued, false);

    void HandleSkill(GameObject prefab, Transform firePoint, ref bool skillQueued, bool chargeAndFire)
    {
        if (!skillQueued) return;
        skillQueued = false;

        if (chargeAndFire)
            StartCoroutine(ChargeAndFireSkill(prefab, firePoint));
        else
            SpawnProjectile(prefab, firePoint, true);
    }

    IEnumerator ChargeAndFireSkill(GameObject prefab, Transform firePoint)
    {
        GameObject projectile = Instantiate(prefab, firePoint.position, Quaternion.identity);
        projectile.SetActive(true);
        projectile.tag = isPlayer1 ? "Projectile_P1" : "Projectile_P2";

        Animator projAnimator = projectile.GetComponent<Animator>();
        if (projAnimator != null) projAnimator.enabled = false;

        yield return new WaitForSeconds(0.2f);

        if (projAnimator != null)
        {
            projAnimator.enabled = true;

        }

        float direction = transform.localScale.x > 0 ? 1 : -1;
        projectile.GetComponent<Projectile>().SetDirection(direction);
    }

    void SpawnProjectile(GameObject prefab, Transform firePoint, bool shouldMove)
    {
        GameObject projectile = Instantiate(prefab, firePoint.position, Quaternion.identity);
        projectile.SetActive(true);
        projectile.tag = isPlayer1 ? "Projectile_P1" : "Projectile_P2";

        if (shouldMove)
        {
            float direction = transform.localScale.x > 0 ? 1 : -1;
            projectile.GetComponent<Projectile>().SetDirection(direction);
        }
    }
}