using UnityEngine;

public class C6 : CharacterBase
{
    [SerializeField] private GameObject C6_1Prefab;
    [SerializeField] private GameObject C6_2Prefab;
    [SerializeField] private Transform firePoint1;
    [SerializeField] private Transform firePoint2;

    [SerializeField] private float damageSkill1 = 15f;
    [SerializeField] private float damageSkill2 = 35f;

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(skill1Key))
        {
            //if (CheckMana(1, true))
            //{
                QueueSkill("Skill1", 1);
            //}
        }
        if (Input.GetKeyDown(skill2Key))
        {
            //if (!CheckMana(2, true))
            //{
                QueueSkill("Skill2", 2);
            //}
        }
    }
    public void SpawnSkill1()
    {
        //if (CheckMana(1,false))
        //{
            StartCoroutine(FireProjectileWithChargeOption(C6_1Prefab, firePoint1, damageSkill1, false, true));
        //}
    }
    public void SpawnSkill2()
    {
        //if (CheckMana(2, false))
        //{
            StartCoroutine(FireProjectileWithChargeOption(C6_2Prefab, firePoint2, damageSkill2, true, false));
        //}
    }
}
