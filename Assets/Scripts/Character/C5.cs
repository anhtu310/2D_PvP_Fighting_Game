using UnityEngine;

public class C5 : CharacterBase
{
	[SerializeField] private GameObject C5Prefab;
	[SerializeField] private Transform firePoint;

	[SerializeField] private float damageSkill = 20f;
    [SerializeField] private float duration =2f;

    protected override void Update()
	{
		base.Update();

		if (Input.GetKeyDown(skill1Key))
		{
			if (CheckMana(1, true))
			{
				QueueSkill("Skill1");
		}
	}
		if (Input.GetKeyDown(skill2Key))
		{
			if (CheckMana(2, true))
			{
				QueueSkill("Skill2");
                ActivateSkill2(duration);
		}
	}
	}


	public void SpawnSkill1() => StartCoroutine(FireProjectileWithChargeOption(C5Prefab, firePoint, damageSkill, false, false));

    public void ActivateSkill2(float duration)
    {

        StartCoroutine(ActivateInvincibility(duration));
    }

}
