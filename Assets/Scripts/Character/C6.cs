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

        if (Input.GetKeyDown(skill1Key)) TryUseSkill(1, "Skill1");
        if (Input.GetKeyDown(skill2Key)) TryUseSkill(2, "Skill2");
    }
    public void SpawnSkill1() => StartCoroutine(FireProjectileWithChargeOption(C6_1Prefab, firePoint1, damageSkill1, false,true));
    public void SpawnSkill2() => StartCoroutine(FireProjectileWithChargeOption(C6_2Prefab, firePoint2, damageSkill2, true,false));
}
