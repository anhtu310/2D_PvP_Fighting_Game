using UnityEngine;

public class C5 : CharacterBase
{
	[SerializeField] private GameObject C5Prefab;
	[SerializeField] private Transform firePoint;

	[SerializeField] private float damageSkill = 20f;
	[SerializeField] private float healAmount = 30f;

	protected override void Update()
	{
		base.Update();

		if (Input.GetKeyDown(skill1Key)) TryUseSkill(1, "Skill1");
		if (Input.GetKeyDown(skill2Key)) TryUseSkill(2, "Skill2");
	}


	public void SpawnSkill1() => StartCoroutine(FireProjectileWithChargeOption(C5Prefab, firePoint, damageSkill, false, false));

	public void Heal()
	{
		HealSkill(healAmount);
	}
}
