using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public BackgroundHealthBar backgroundHealthBar;

    public void SetMaximumHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        backgroundHealthBar.slider.maxValue = health;
        backgroundHealthBar.slider.value = health;
    }

    public void SetHealth(float health)
    {
        if (slider.value > health)
        {
            StartCoroutine(HealthDepletionDelay());
        }
        if (slider.value < health)
        {
            backgroundHealthBar.slider.value = health;
        }
        slider.value = health;
    }

    //After taking damage, the health slowly decreases instead of instantly, giving a nice effect
    IEnumerator HealthDepletionDelay()
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = (int)backgroundHealthBar.slider.value; i >= slider.value; i--)
        {
            backgroundHealthBar.slider.value -= 1f;
            yield return new WaitForSeconds(0.02f);
        }
    }
}
