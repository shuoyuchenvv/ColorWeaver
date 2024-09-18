using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrink : MonoBehaviour
{
    public float energyAmount = 20f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnergyBar energyBar = other.GetComponent<EnergyBar>();
            if (energyBar != null)
            {
                energyBar.RefillEnergy(energyAmount);
                Destroy(gameObject);
            }
        }
    }
}
