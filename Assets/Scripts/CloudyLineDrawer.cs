using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudyLineDrawer : MonoBehaviour
{
    [Header("Component References")]
    public LineRenderer lineRenderer;
    public GameObject linePrefab;
    public InstructionTutorial instructionTutorial;
    public PlayerMovement playerMovement;
    public EnergyBar energyBar;

    [Header("Materials")]
    public Material redMaterial;
    public Material yellowMaterial;
    public Material blueMaterial;
    public Material greenMaterial;

    [Header("Particle Prefabs")]
    public GameObject redFire;
    public GameObject yellowFire;
    public GameObject blueFire;
    public GameObject greenFire;

    [Header("Settings")]
    public float energyCostPerSet = 10f;
    public Vector3 offset = new Vector3(0, -1, 0);
    public int maxTotalColliders = 50;
    public int existTime = 10;
    public Vector3 fartInterval = new Vector3(0, 3, 0);

    private GameObject particlePrefab;
    private List<Vector3> ribbonPositions = new List<Vector3>();
    private List<Material> segmentMaterials = new List<Material>();
    private Material currentMaterial;
    private List<LineRenderer> lineRenderers = new List<LineRenderer>();
    private List<Vector3> colliderPositions = new List<Vector3>();
    private List<GameObject> collidersList = new List<GameObject>();

    private bool fireIsRecord = false;
    private bool collidersActive = false;
    private bool hasChangedColor = false;
    private bool colored = false;
    private bool isUsingController;

    private Vector3 velocity;
    private CharacterController controller;


    void Start()
    {
        energyBar = GetComponent<EnergyBar>();
        controller = GetComponent<CharacterController>();
        colored = false;
        isUsingController = PlayerPrefs.GetInt("ControlType", 0) == 1;
    }

    void Update()
    {
        HandleColorChange();
        DrawRibbon();
    }

    private void HandleColorChange()
    {
        if (isUsingController)
        {
            //test
            float dpadH = Input.GetAxisRaw("DPad_Horizontal");
            float dpadV = Input.GetAxisRaw("DPad_Vertical");
            Debug.Log($"DPad Input - H: {dpadH}, V: {dpadV}");

            if (dpadV > 0.5f && instructionTutorial.redPeriodActivated)
            {
                Debug.Log("Trying to change to Red");
                hasChangedColor = true;
                ChangeMaterial(redMaterial);
                particlePrefab = redFire;
                colored = true;
            }
            else if (dpadH > 0.5f && instructionTutorial.bluePeriodActivated)
            {
                Debug.Log("Trying to change to Blue");
                hasChangedColor = true;
                ChangeMaterial(blueMaterial);
                particlePrefab = blueFire;
                colored = true;
            }


            //
            if (Input.GetButtonDown("Joy_Jump") && colored && instructionTutorial.jumpActivated)
            {
                if (energyBar.HasEnoughEnergy(energyCostPerSet) && !energyBar.isRegenerating)
                {
                    JustFart();
                    Debug.Log("Controller Jump, y velocity = " + controller.velocity.y);
                }
            }
            // D-pad controls for controller
            if (Input.GetAxisRaw("DPad_Vertical") > 0.5f && instructionTutorial.redPeriodActivated)  // 上
            {
                ChangeColor(redMaterial, redFire);
            }
            else if (Input.GetAxisRaw("DPad_Horizontal") > 0.5f && instructionTutorial.bluePeriodActivated)  // 右
            {
                ChangeColor(blueMaterial, blueFire);
            }
            else if (Input.GetAxisRaw("DPad_Vertical") < -0.5f && instructionTutorial.greenPeriodActivated)  // 下
            {
                ChangeColor(greenMaterial, greenFire);
            }
            else if (Input.GetAxisRaw("DPad_Horizontal") < -0.5f && instructionTutorial.yellowPeriodActivated)  // 左
            {
                ChangeColor(yellowMaterial, yellowFire);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && colored && instructionTutorial.jumpActivated)
            {
                if (energyBar.HasEnoughEnergy(energyCostPerSet) && !energyBar.isRegenerating)
                {
                    JustFart();
                    Debug.Log("Keyboard Jump, y velocity = " + controller.velocity.y);
                }
            }
            // Keyboard number controls
            if (Input.GetKeyDown(KeyCode.Alpha1) && instructionTutorial.redPeriodActivated)
            {
                ChangeColor(redMaterial, redFire);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && instructionTutorial.bluePeriodActivated)
            {
                ChangeColor(blueMaterial, blueFire);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && instructionTutorial.greenPeriodActivated)
            {
                ChangeColor(greenMaterial, greenFire);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) && instructionTutorial.yellowPeriodActivated)
            {
                ChangeColor(yellowMaterial, yellowFire);
            }
        }
    }

    private void ChangeColor(Material material, GameObject fireParticle)
    {
        hasChangedColor = true;
        ChangeMaterial(material);
        particlePrefab = fireParticle;
        colored = true;
    }


    //old update
    /*
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && instructionTutorial.redPeriodActivated)
        {
            hasChangedColor = true;
            ChangeMaterial(redMaterial);
            particlePrefab = redFire;
            colored = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && instructionTutorial.bluePeriodActivated)
        {
            hasChangedColor = true;
            ChangeMaterial(blueMaterial);
            particlePrefab = blueFire;
            colored = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && instructionTutorial.greenPeriodActivated)
        {
            hasChangedColor = true;
            ChangeMaterial(greenMaterial);
            particlePrefab = greenFire;
            colored = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && instructionTutorial.yellowPeriodActivated)
        {
            hasChangedColor = true;
            ChangeMaterial(yellowMaterial);
            particlePrefab = yellowFire;
            colored = true;
        }

        // Select the correct particle prefab based on the current material
        //if (currentMaterial == redMaterial) { particlePrefab = redFire; }
        //if (currentMaterial == yellowMaterial) { particlePrefab = yellowFire; }
        //if (currentMaterial == blueMaterial) { particlePrefab = blueFire; }
        //if (currentMaterial == greenMaterial) { particlePrefab = greenFire; }


        // Always draw the ribbon as the player moves
        DrawRibbon();


        // Force end if energy depletes during collider generation
        //if (collidersActive && !energyBar.HasEnoughEnergy(energyCostPerSet))
        //{
        //    EndColliderGeneration();
        //}


        // 
    //    if (Input.GetKeyDown(KeyCode.Space) && colored && instructionTutorial.jumpActivated)
    //    {

    //        // Check if enough energy exists before starting collider generation
    //        if (energyBar.HasEnoughEnergy(energyCostPerSet))
    //        {

    //            JustFart();


    //            Debug.Log("StartRecording, y velocity = " + controller.velocity.y);
    //        }
    //        else
    //        {
    //            Debug.Log("Not enough energy to start collider generation.");
    //        }

    //    }

    //    if (fireIsRecord && controller.velocity.y <= 0)
    //    {
    //        EndColliderGeneration();
    //    }

    //    if (fireIsRecord)
    //    {
    //        RecordColliderPath();
    //    }

    }    */

    public void JustFart()
    {
        Vector3 currentPosition = transform.position + offset;
        Vector3 Position2 = currentPosition + fartInterval;
        Vector3 Position3 = Position2 + fartInterval;

        

        GameObject colliderObject = Instantiate(particlePrefab, currentPosition, Quaternion.identity);
        colliderObject.layer = LayerMask.NameToLayer("Default");
        colliderObject.GetComponent<Collider>().enabled = true;
        collidersList.Add(colliderObject);
        energyBar.UseEnergy(energyCostPerSet);

    //    GameObject colliderObject2 = Instantiate(particlePrefab, Position2 , Quaternion.identity);
    //    colliderObject2.layer = LayerMask.NameToLayer("Default");
    //    colliderObject2.GetComponent<Collider>().enabled = true;
    //    collidersList.Add(colliderObject2);
    //    energyBar.UseEnergy(energyCostPerSet);

    //    Collider collider2 = colliderObject2.GetComponent<Collider>();
    //    Vector3 collider2Top = collider2.bounds.center + Vector3.up * collider2.bounds.extents.y;
    //    Transform playerTransform = gameObject.transform;
    //    playerTransform.position = new Vector3(
    //    playerTransform.position.x, // 保持玩家的 X 位置不变
    //    collider2Top.y + 5f,       // 顶部稍微上移 0.1 单位
    //    playerTransform.position.z  // 保持玩家的 Z 位置不变
    //);

        //GameObject colliderObject3 = Instantiate(particlePrefab, Position3, Quaternion.identity);
        //colliderObject2.layer = LayerMask.NameToLayer("Default");
        //colliderObject2.GetComponent<Collider>().enabled = true;
        //collidersList.Add(colliderObject2);

        // Destroy collider after 10 seconds
        Destroy(colliderObject, existTime);
       // Destroy(colliderObject2, existTime);
        //Destroy(colliderObject3, existTime);
    }

    void ChangeMaterial(Material newMaterial)
    {
        if (hasChangedColor)
        {
            SaveLine();
            currentMaterial = newMaterial;
            lineRenderer.material = currentMaterial;
            ribbonPositions.Clear();
            lineRenderer.positionCount = 0;
            hasChangedColor = false;
        }
    }

    void DrawRibbon()
    {
        Vector3 currentPosition = transform.position + offset;

        if (ribbonPositions.Count == 0 || Vector3.Distance(ribbonPositions[ribbonPositions.Count - 1], currentPosition) > 0.1f)
        {
            ribbonPositions.Add(currentPosition);
            lineRenderer.positionCount = ribbonPositions.Count;
            lineRenderer.SetPositions(ribbonPositions.ToArray());
        }
    }

    void SaveLine()
    {
        if (ribbonPositions.Count == 0) return;
        GameObject newLineObject = Instantiate(linePrefab, transform.position, Quaternion.identity);
        newLineObject.tag = "Line";
        LineRenderer newLineRenderer = newLineObject.GetComponent<LineRenderer>();

        newLineRenderer.positionCount = lineRenderer.positionCount;
        newLineRenderer.SetPositions(ribbonPositions.ToArray());
        newLineRenderer.material = lineRenderer.material;
        newLineRenderer.startWidth = lineRenderer.startWidth;
        newLineRenderer.endWidth = lineRenderer.endWidth;

        lineRenderer.positionCount = 0;
        ribbonPositions.Clear();
    }

    void StartRecording()
    {
        fireIsRecord = true;
        colliderPositions.Clear();
    }

    void RecordColliderPath()
    {
        if (!playerMovement.isLifting)
        {
            Vector3 currentPosition = transform.position + offset;
            if (colliderPositions.Count == 0 || Vector3.Distance(colliderPositions[colliderPositions.Count - 1], currentPosition) > 0.5f)
            {
                colliderPositions.Add(currentPosition);
            }
        }
    }

    void EndColliderGeneration()
    {
        collidersActive = true;

       
        // Instantiate colliders along the recorded path
        foreach (Vector3 position in colliderPositions)
        {
            if (Vector3.Distance(transform.position, position) > 1.0f)
            {
                GameObject colliderObject = Instantiate(particlePrefab, position, Quaternion.identity);
                colliderObject.layer = LayerMask.NameToLayer("Default");
                colliderObject.GetComponent<Collider>().enabled = true;
                collidersList.Add(colliderObject);

                // Destroy collider after 10 seconds
                Destroy(colliderObject, existTime );


                if (collidersList.Count > maxTotalColliders)
                {
                    Destroy(collidersList[0]);
                    collidersList.RemoveAt(0);
                }
            }
        }

        fireIsRecord = false;

        // Deduct energy after the colliders are created
        //energyBar.UseEnergy(energyCostPerSet);
        colliderPositions.Clear();
    }
}
