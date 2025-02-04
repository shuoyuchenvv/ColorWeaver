using UnityEngine;

public class InstructionTutorial : MonoBehaviour
{
 
    public GameObject yellowInstructionFire;  // 
    public GameObject redInstructionFire;
    public GameObject greenInstructionFire;
    public GameObject blueInstructionFire;
    public GameObject colorInstructionFire;
    public GameObject jumpInstructionFire;

    public GameObject yellowUIPanel;  //  
    public GameObject redUIPanel;  //  
    public GameObject greenUIPanel;  //  
    public GameObject blueUIPanel;  //
    public GameObject colorUIPanel;
    public GameObject jumpUIPanel;
    // 
    //public GameObject ColorPanelonscreen;  //  
    public GameObject redOnScreen;
    public GameObject blueOnScreen;
    public GameObject greenOnScreen;
    public GameObject yellowOnScreen;
    public GameObject jumpPanelonscreen;
    
    public GameObject tutorialCheck;

    public bool yellowPeriodActivated = false;
    public bool redPeriodActivated = false;
    public bool greenPeriodActivated = false;
    public bool bluePeriodActivated = false;
    public bool jumpActivated = false;

    private bool isTutorial = true;

    private void OnTriggerEnter(Collider other)
    {
        // 
        Debug.Log("collide");
        if (other.gameObject == yellowInstructionFire)
        {
            yellowPeriodActivated = true;
            yellowUIPanel.SetActive(true);  // 
            
        }
        if (other.gameObject == redInstructionFire)
        {
            redPeriodActivated = true;
            redUIPanel.SetActive(true);  // 

        }
        if (other.gameObject == greenInstructionFire)
        {
            greenPeriodActivated = true;
            greenUIPanel.SetActive(true);  // 

        }
        if (other.gameObject == blueInstructionFire)
        {
            bluePeriodActivated = true;
            blueUIPanel.SetActive(true);  // 
        }
        if (other.gameObject == colorInstructionFire)
        {
           // ColorPanelonscreen.SetActive(true);
            colorUIPanel .SetActive(true);  //
                                            
        }
        if (other.gameObject == jumpInstructionFire)
        {
            jumpPanelonscreen.SetActive(true);
            jumpUIPanel.SetActive(true);  // 
            jumpActivated = true;
        }
        if (other.gameObject == tutorialCheck)
        {
            isTutorial = false;
            //ColorPanelonscreen.SetActive(true);
            yellowOnScreen.SetActive(true);
            redOnScreen.SetActive(true);
            blueOnScreen.SetActive(true);
            greenOnScreen.SetActive(true);
            jumpPanelonscreen.SetActive(true);
            jumpInstructionFire.SetActive(false);
            colorInstructionFire.SetActive(false);
            colorUIPanel.SetActive(false);
            jumpUIPanel.SetActive(false);

        }

    }

    private void Update()
    {
        if (isTutorial)
        {// 
            if (Input.GetKeyDown(KeyCode.Alpha4) && yellowPeriodActivated)
            {
                Destroy(yellowUIPanel);
                yellowOnScreen.SetActive(true);
                yellowUIPanel = new GameObject("Placeholder");  // 
                redInstructionFire.SetActive(true);       // 
                Destroy(yellowInstructionFire);
                yellowInstructionFire = new GameObject("Placeholder");
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) && redPeriodActivated)
            {
                Destroy(redUIPanel);
                redOnScreen.SetActive(true);
                redUIPanel = new GameObject("Placeholder");  // 
                greenInstructionFire.SetActive(true);       // 
                Destroy(redInstructionFire);
                redInstructionFire = new GameObject("Placeholder");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && greenPeriodActivated)
            {
                Destroy(greenUIPanel);
                greenOnScreen.SetActive(true);
                greenUIPanel = new GameObject("Placeholder");  // 
                blueInstructionFire.SetActive(true);       // 
                Destroy(greenInstructionFire);
                greenInstructionFire = new GameObject("Placeholder");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && bluePeriodActivated)
            {
                Destroy(blueUIPanel);
                blueOnScreen.SetActive(true);
                blueUIPanel = new GameObject("Placeholder");  // 
                colorInstructionFire.SetActive(true);       // 
                jumpInstructionFire.SetActive(true);
                Destroy(blueInstructionFire);
                blueInstructionFire = new GameObject("Placeholder");
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                colorUIPanel.SetActive(false);
                jumpUIPanel.SetActive(false); 
            }
        }
    }

  



}
