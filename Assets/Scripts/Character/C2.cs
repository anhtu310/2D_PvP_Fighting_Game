using UnityEngine;

public class C2 : CharacterBase
{
    [SerializeField] private GameObject C2Prefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private float damageSkill = 15f;
    [SerializeField] private float healAmount = 30f;

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(skill1Key))
        {
            //if(CheckMana(1,true))
            //{
                QueueSkill("Skill1");
            //}
        }
        if (Input.GetKeyDown(skill2Key))
        {
            //if (CheckMana(2, true))
            //{
                QueueSkill("Skill2");
            //}
        }
    }


    public void SpawnSkill1()
    {
        //if (!CheckMana(1, false))
        //{
            StartCoroutine(FireProjectileWithChargeOption(C2Prefab, firePoint, damageSkill, false, false));
        //}
    }

    public void Heal()
    {
        //if(!CheckMana(2, false))
        //{
            HealSkill(healAmount);
        //}
    }
    
}
