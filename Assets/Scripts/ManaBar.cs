using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    [SerializeField] private Slider manaSlider;

    public void SetMaxMana(float maxMana)
    {
        manaSlider.maxValue = maxMana;
        manaSlider.value = maxMana;
    }

    public void SetMana(float mana)
    {
        manaSlider.value = mana;
    }
}
