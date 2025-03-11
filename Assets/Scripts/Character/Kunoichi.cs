//using UnityEngine;

//public class Kunoichi : CharacterBase
//{
//    [SerializeField] private GameObject shurikenPrefab;
//    [SerializeField] private Transform firePoint;
//    [SerializeField] private int healAmount = 20;

//    protected override void Start()
//    {
//        base.Start();
//    }

//    public override void UseSkill1()
//    {
//        animator.SetTrigger("Skill1");
//        GameObject shuriken = Instantiate(shurikenPrefab, firePoint.position, Quaternion.identity);
//        shuriken.GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
//    }

//    public override void UseSkill2()
//    {
//        animator.SetTrigger("Skill2");
//        Heal(healAmount);
//    }

//    private void Heal(int amount)
//    {
//        if (isDead) return;
//        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
//    }
//}