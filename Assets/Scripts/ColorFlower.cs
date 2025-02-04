using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFlower : MonoBehaviour
{
    public enum FlowerColor { Red, Green, Blue, Yellow }
    public FlowerColor flowerColorType;
    public ColorChangerKeyBoard colorChangerKeyBoard;
    private string currentColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentColor = colorChangerKeyBoard.GetCurrentColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 
        if (other.CompareTag("Player"))
        {
            // 
            if (currentColor == flowerColorType.ToString())
            {
                // 
                gameObject.SetActive(false);
            }
        }
    }
}
