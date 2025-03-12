using System.Collections;
using UnityEngine;

public class ManaSystem : MonoBehaviour
{
    private float currentMana;
    private ManaBar manaBar;
    private CharacterBase character;

    private void Start()
    {
        character = GetComponent<CharacterBase>();
        currentMana = character.maxMana;
        FindAndAssignManaBar();
        StartCoroutine(RegenerateMana());
    }

    public float CurrentMana => currentMana;

    void FindAndAssignManaBar()
    {
        GameObject barObject = GameObject.Find(CompareTag("Player1") ? "MP1" : "MP2");
        if (barObject != null)
        {
            manaBar = barObject.GetComponent<ManaBar>();
            manaBar.SetMaxMana(character.maxMana);
            manaBar.SetMana(currentMana);
        }
    }

    public void ChangeMana(float amount)
    {
        currentMana = Mathf.Clamp(currentMana + amount, 0, character.maxMana);
        if (manaBar != null)
            manaBar.SetMana(currentMana);
    }

    IEnumerator RegenerateMana()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            ChangeMana(character.manaRegenRate);
        }
    }

    public bool UseMana(float amount)
    {
        if (currentMana >= amount)
        {
            ChangeMana(-amount);
            return true;
        }
        return false;
    }
}
