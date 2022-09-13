using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JetpackResourceBar : MonoBehaviour
{
    public Slider slider;
    public float regenerationDelay;

    public float maximumJetpackFuel = 100;
    public float currentJetpackFuel;
    public BackgroundHealthBar backgroundHealthBar;

    private void Awake()
    {
        InvokeRepeating(nameof(StaminaRegeneration), 0f, 0.35f);
    }

    void Start()
    {
        currentJetpackFuel = maximumJetpackFuel;
        SetMaximumJetpackFuel(maximumJetpackFuel);
        backgroundHealthBar.slider.maxValue = maximumJetpackFuel;
        backgroundHealthBar.slider.value = maximumJetpackFuel;
    }

    private void Update()
    {
        SetStamina(currentJetpackFuel);
    }

    void StaminaRegeneration()
    {
        if (Time.time >= regenerationDelay && slider.value <= slider.maxValue)
        {
            if (currentJetpackFuel + 5 > maximumJetpackFuel)
            {
                currentJetpackFuel = maximumJetpackFuel;
            }
            else
            {
                currentJetpackFuel += 5;
            }
        }

        slider.value = currentJetpackFuel;
        backgroundHealthBar.slider.value = currentJetpackFuel;
    }

    public void SetMaximumJetpackFuel(float fuel)
    {
        slider.maxValue = fuel;
        slider.value = fuel;
    }

    public void SetStamina(float fuel)
    {
        if (slider.value > fuel)
        {
            StartCoroutine(StaminaDepletionDelay());
        }
        if (slider.value < fuel)
        {
            backgroundHealthBar.slider.value = fuel;
        }
        slider.value = fuel;
    }

    IEnumerator StaminaDepletionDelay()
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = (int)backgroundHealthBar.slider.value; i >= slider.value; i--)
        {
            backgroundHealthBar.slider.value -= 1f;
            yield return new WaitForSeconds(0.02f);
        }
    }

}
