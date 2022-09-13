using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    bool facingLeft = true;
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

    IEnumerator HealthDepletionDelay()
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = (int)backgroundHealthBar.slider.value; i >= slider.value; i--)
        {
            backgroundHealthBar.slider.value -= 1f;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void Flip()
    {
        facingLeft = !facingLeft;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
