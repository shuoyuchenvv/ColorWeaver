using UnityEngine;

public class TransparencyController : MonoBehaviour
{
    public GameObject innerLayerPlayer; // Reference to the inner layer player
    [Range(0f, 1f)]
    public float transparencyLevel = 0.1f; // Transparency level adjustable in Inspector

    private Material innerLayerMaterial;
    private float originalTransparency;
    private bool isCameraOverride = false; // Flag to control transparency override

    void Start()
    {
        if (innerLayerPlayer != null)
        {
            Renderer innerRenderer = innerLayerPlayer.GetComponent<Renderer>();
            if (innerRenderer != null)
            {
                innerLayerMaterial = innerRenderer.material;
                originalTransparency = innerLayerMaterial.color.a;
            }
        }
    }

    public void SetTemporaryTransparency()
    {
        if (innerLayerMaterial != null)
        {
            isCameraOverride = true;
            Color color = innerLayerMaterial.color;
            color.a = transparencyLevel; // Set to inspector-defined transparency
            innerLayerMaterial.color = color;
        }
    }

    public void ResetTransparency()
    {
        isCameraOverride = false;
        ForceEnergyTransparency();
    }

    // Method to force apply energy-based transparency immediately
    public void ForceEnergyTransparency()
    {
        if (innerLayerMaterial != null && !isCameraOverride) // Only update if camera override is off
        {
            EnergyBar energyBar = GetComponent<EnergyBar>();
            if (energyBar != null)
            {
                float transparency = energyBar.CurrentEnergy / energyBar.maxEnergy;
                Color color = innerLayerMaterial.color;
                color.a = transparency;
                innerLayerMaterial.color = color;
            }
        }
    }

    // Method to update transparency based on energy bar
    public void ApplyEnergyTransparency(float transparency)
    {
        if (!isCameraOverride && innerLayerMaterial != null) // Only update if camera override is off
        {
            Color color = innerLayerMaterial.color;
            color.a = transparency;
            innerLayerMaterial.color = color;
        }
    }
}
