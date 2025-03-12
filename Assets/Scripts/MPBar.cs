using UnityEngine;
using UnityEngine.UI;

public class MPBar : MonoBehaviour
{

    [SerializeField] private Slider mpSlide;

    public void SetMaxHealth(float maxMP)
    {
        mpSlide.maxValue = maxMP;
        mpSlide.value = maxMP;
    }

    public void SetHealth(float mp)
    {
        mpSlide.value = mp;
    }
}
