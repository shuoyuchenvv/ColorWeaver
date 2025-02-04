using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Slider energySlider;
    public float maxEnergy = 100f;
    public float regenerationRate = 5f; // Amount to regenerate per second
    public float regenerationDelay = 10f; // Delay before regeneration starts after inactivity
    public GameObject player; // Reference to the player for transparency control


    private float currentEnergy;
    public bool isRegenerating = false;
    private float lastEnergyUseTime = 0f; // Tracks the time of the last energy usage

    void Start()
    {
        currentEnergy = maxEnergy;
        UpdateEnergyBar();
        UpdatePlayerTransparency();
    }

    public void RefillEnergy(float amount)
    {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
        UpdateEnergyBar();
        UpdatePlayerTransparency();
    }

    public bool UseEnergy(float amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            UpdateEnergyBar();
            UpdatePlayerTransparency();

            // Reset the timer when energy is used
            lastEnergyUseTime = Time.time;

            // Stop regeneration if energy is used again
            if (isRegenerating)
            {
                StopCoroutine("RegenerateEnergy");
                isRegenerating = false;
            }

            return true;
        }
        Debug.Log("Energy insufficient for action.");
        return false;
    }

    private void UpdateEnergyBar()
    {
        energySlider.value = currentEnergy / maxEnergy;
    }

    private void UpdatePlayerTransparency()
    {
        float transparency = currentEnergy / maxEnergy;

        TransparencyController transparencyController = player.GetComponent<TransparencyController>();

        if (transparencyController != null)
        {
            transparencyController.ApplyEnergyTransparency(transparency);
        }
    }


    void Update()
    {
        // Check if the regeneration delay has passed since the last energy use
        if (!isRegenerating && Time.time - lastEnergyUseTime >= regenerationDelay && currentEnergy < maxEnergy)
        {
            StartCoroutine(RegenerateEnergy());
        }
    }

    IEnumerator RegenerateEnergy()
    {
        isRegenerating = true;

        // Regenerate energy slowly over time
        while (currentEnergy < maxEnergy)
        {
            currentEnergy = Mathf.Min(currentEnergy + regenerationRate * Time.deltaTime, maxEnergy);
            UpdateEnergyBar();
            UpdatePlayerTransparency();
            yield return null; // Wait for the next frame
        }

        isRegenerating = false;
    }

    public bool HasEnoughEnergy(float amount)
    {
        return currentEnergy >= amount;
    }

    public float CurrentEnergy
    {
        get { return currentEnergy; }
    }
}
