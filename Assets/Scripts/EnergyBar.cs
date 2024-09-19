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

    // Add a public property to access the current energy
    public float CurrentEnergy
    {
        get { return currentEnergy; }
    }

    // Optional: Check if enough energy is available for a certain cost
    public bool HasEnoughEnergy(float amount)
    {
        return currentEnergy >= amount;
    }
}
