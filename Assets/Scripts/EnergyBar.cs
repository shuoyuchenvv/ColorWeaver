using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Slider energySlider;
    public float maxEnergy = 100f;
    private float currentEnergy;

    void Start()
    {
        currentEnergy = maxEnergy;
        UpdateEnergyBar();
    }

    public void RefillEnergy(float amount)
    {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
        UpdateEnergyBar();
    }

    public bool UseEnergy(float amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            UpdateEnergyBar();
            return true;
        }
        return false;
    }

    private void UpdateEnergyBar()
    {
        energySlider.value = currentEnergy / maxEnergy;
    }
}
