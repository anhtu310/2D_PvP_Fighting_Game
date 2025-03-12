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

        if (Input.GetKeyDown(skill1Key)) TryUseSkill(1, "Skill1");
        if (Input.GetKeyDown(skill2Key)) TryUseSkill(2, "Skill2");
    }


    public void SpawnSkill1() => StartCoroutine(FireProjectileWithChargeOption(C2Prefab, firePoint, damageSkill, false, false));

    public void Heal()
    {
        HealSkill(healAmount);
    }
    
}
