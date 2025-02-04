using UnityEngine;
using System.Collections.Generic;

public class Ignitable : MonoBehaviour
{
    public string marbleStandTag = "MarbleStand";
    public string marbleTag = "Marble";
    private bool playerCarryRed;

    // Ranges and Effects for Yellow Effect
    public float ignitionRange = 8f;       // Range for ignition
    public float effectRange = 3f;         // Range for the wind effect
   public float maxHeight = 140f;         // Maximum height for wind effect

    // Push Parameters for Red Effect
    //public float pushSpeed = 20f; // 
    public float pushDistance = 0.6f; // 
    public GameObject player;
    //private bool isPlayerInPushRange = false; //
    //private Vector3 lastPlayerPosition; // 
    //public float pushAngleThreshold = 60f;

    //Actived or not
    public static bool yellowActived = false ;
    public static bool redActived = false;
    public static bool greenActived = false;
    public static bool blueActived = false;

    // Ignition Status Flags
    //private bool isPushing = false;
    private Transform playerTransform;
    public bool isYellowIgnited = false;
    public bool isBlueIgnited = false;
    public bool isRedIgnited = false;
    public bool isGreenIgnited = false;
    public bool isIgnited = false;         // Indicates if the object is ignited

    // Game Objects for Effects
    public GameObject initialFire;
    public GameObject innerMarble;
    public GameObject yellowWindPrefab;    // Yellow wind effect prefab
    public GameObject redMovementPrefab;

    // Material and Physic Material References
    public PhysicMaterial redPhysicMaterial;
    public Material initialMaterial;
    public Material redMaterial;
    public Material greenMaterial;
    public Material blueMaterial;
    public Material yellowMaterial;

    // External References
    public GameObject colorKeyboardController;           // GameObject containing the color controller
    public ColorChangerKeyBoard colorChangerKeyBoard;    // Script managing color changes

    // Wind Lift Parameters
    public float liftForce = 10f;          // Force applied to lift the player
    public float maxUpwardVelocity = 8f;   // Max upward speed for player

    // Internal State
    private GameObject currentIgnitionMagic;
    private string currentEffectColor;     // Currently active color effect
    private Renderer innerRenderer;
    private SphereCollider marbleCollider;
    private Rigidbody marbleRigidbody;
    
    private HashSet<CharacterController> playersInRange = new HashSet<CharacterController>();
    private CharacterController characterController;

    void Start()
    {
        // Initialize Components
        innerRenderer = innerMarble.GetComponent<Renderer>();
        marbleCollider = GetComponent<SphereCollider>();
        marbleRigidbody = GetComponent<Rigidbody>();

        // Validate and Fetch ColorChangerKeyBoard
        if (colorKeyboardController == null)
        {
            Debug.LogError("colorKeyboardController is not assigned in the Inspector.");
        }
        else
        {
            colorChangerKeyBoard = colorKeyboardController.GetComponent<ColorChangerKeyBoard>();
            if (colorChangerKeyBoard == null)
            {
                Debug.LogError("ColorChangerKeyBoard component is missing on the colorKeyboardController GameObject.");
            }
        }

        //marbleRigidbody.isKinematic = true;
    }

   

    // Handles Ignition Based on Current Color Effect
    public void Ignite()
    {
        if (colorChangerKeyBoard != null)
        {
            currentEffectColor = colorChangerKeyBoard.GetCurrentColor();
            Debug.Log("Current color: " + currentEffectColor);

            // Extinguish previous effects before applying new ignition
            Extinguish();

            // Apply effect based on the current color
            switch (currentEffectColor)
            {
                case "Red":
                    if (redActived)
                    {
                        ApplyRedEffect();
                    }
                    break;
                case "Yellow":
                    if (yellowActived)
                    {
                        ApplyYellowEffect();
                    }
                    break;
                case "Green":
                    if (greenActived)
                    {
                        ApplyGreenEffect();
                    }
                    break;
                case "Blue":
                    if (blueActived)
                    {
                        ApplyBlueEffect();
                    }
                    break;
                default:
                    Debug.LogWarning("No effect for this color.");
                    break;
            }

            isIgnited = true;
            Debug.Log($"{gameObject.name} has been ignited!");
        }
        else
        {
            Debug.LogError("colorChangerKeyBoard is null, ensure it's properly initialized.");
        }
    }

    private void ApplyRedEffect()
    {
        playerTransform = player.transform;
        foreach (Transform child in playerTransform)
        {
            if (child.CompareTag(marbleTag))
            {
                playerCarryRed = true;
            }
            else
            {
                playerCarryRed = false;
            }

        }


        if (!playerCarryRed)
        {
            innerRenderer.material = redMaterial;
             initialFire.SetActive(false);
             currentIgnitionMagic = Instantiate(redMovementPrefab, transform.position, Quaternion.Euler(0f, 0f, 90f));
             currentIgnitionMagic.transform.SetParent(transform);
              ResetOtherIgnitions();
             isRedIgnited = true;
            transform.SetParent(playerTransform);
            transform.localPosition = new Vector3(0, 0, pushDistance);
            transform.localRotation = Quaternion.identity;
            if (marbleRigidbody != null)
            {
                marbleRigidbody.isKinematic = true;
                marbleRigidbody.useGravity = false;
            }
        }


    }

    private void ApplyYellowEffect()
    {
        innerRenderer.material = yellowMaterial;
        initialFire.SetActive(false);
        currentIgnitionMagic = Instantiate(yellowWindPrefab, transform.position, Quaternion.Euler(0f, 0f, 90f));
        ResetOtherIgnitions();
        isYellowIgnited = true;
        marbleRigidbody.isKinematic = true;
    }

    private void ApplyGreenEffect()
    {
        innerRenderer.material = greenMaterial;
        initialFire.SetActive(false);
        ResetOtherIgnitions();
        isGreenIgnited = true;
        marbleRigidbody.isKinematic = true;
    }

    private void ApplyBlueEffect()
    {
        innerRenderer.material = blueMaterial;
        initialFire.SetActive(false);
        ResetOtherIgnitions(); 
        isBlueIgnited = true;
        marbleRigidbody.isKinematic = true;

    }

    private void ResetOtherIgnitions()
    {
        isYellowIgnited = false;
        isBlueIgnited = false;
        isRedIgnited = false;
        isGreenIgnited = false;
    }

    //Detects Player Trigger Entry
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player") && !isPlayerInPushRange && isRedIgnited)
    //    {
    //        playerTransform = other.transform;
    //        transform.SetParent(playerTransform);
    //        transform.localPosition = new Vector3(0, 0, pushDistance);
    //        transform.localRotation = Quaternion.identity;
    //        if (marbleRigidbody != null)
    //        {
    //            marbleRigidbody.isKinematic = true;
    //            marbleRigidbody.useGravity = false;
    //        }
    //    }
    //}

    // Detects Player Trigger Exit
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        //marbleRigidbody.useGravity = true;
    //        isPlayerInPushRange = false;
    //        playerTransform = null;
    //        Debug.Log("Player exited trigger. Setting isPushing to false.");
    //    }
    //}

    // Updates Wind Effect for Yellow Ignition
    void Update()
    {
        if (isYellowIgnited)
        {
            UpdateWindEffect();
        }
        //if (isRedIgnited)
        //{
        //    UpdatePushEffect();
        //}
    }

    //Yellow
    private void UpdateWindEffect()
    {

        //Vector3 boxSize = new Vector3(effectRange, maxHeight, effectRange);
        //Collider[] colliders = Physics.OverlapBox(transform.position, boxSize / 2);
        //HashSet<CharacterController> currentPlayers = new HashSet<CharacterController>();
        Vector3 boxCenter = transform.position + new Vector3(0, maxHeight / 2, 0); // 将中心向上偏移
        Vector3 boxSize = new Vector3(effectRange, maxHeight, effectRange);

        // 检测区域内的所有碰撞体
        Collider[] colliders = Physics.OverlapBox(boxCenter, boxSize / 2);

        // 创建一个 HashSet 来存储检测到的 CharacterController
        HashSet<CharacterController> currentPlayers = new HashSet<CharacterController>();

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                CharacterController characterController = collider.GetComponent<CharacterController>();
                
                if (characterController != null )
                {
                    currentPlayers.Add(characterController);                   
                    ApplyLiftForce(characterController); // 应用提升力
                }
            }
        }

        // 停止范围外玩家的提升力
        foreach (CharacterController player in playersInRange)
        {
            if (!currentPlayers.Contains(player))
            {
                StopLiftForPlayer(player); // 停止提升力
            }
        }

        // 更新范围内的玩家
        playersInRange = currentPlayers;
    }


    //Red
    //private void UpdatePushEffect()
    //{
    //    if (marbleRigidbody != null)
    //    {
    //        marbleRigidbody.isKinematic = false;
    //        //marbleRigidbody.useGravity = true;
    //        marbleRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    //    }
    //    if (isPlayerInPushRange && playerTransform != null)
    //    {
    //        Vector3 currentPlayerPosition = playerTransform.position;
    //        bool isPlayerMoving = Vector3.Distance(currentPlayerPosition, lastPlayerPosition) > 0.01f;

    //        Vector3 directionToObject = (transform.position - playerTransform.position).normalized;
    //        float angleToPlayer = Vector3.Angle(playerTransform.forward, directionToObject);

    //        if (isPlayerMoving && angleToPlayer <= pushAngleThreshold)
    //        {
    //            Vector3 targetPosition = playerTransform.position + playerTransform.forward * pushDistance;
    //            transform.position = Vector3.MoveTowards(transform.position, targetPosition, pushSpeed * Time.deltaTime);
    //        }
    //        lastPlayerPosition = currentPlayerPosition;
    //    }
    //}


    private void ApplyLiftForce(CharacterController characterController)
    {
        PlayerMovement movementScript = characterController.GetComponent<PlayerMovement>();
        float windTopHeight = transform.position.y + maxHeight;
        //Debug.Log($"windtopheight = {windTopHeight}");
        float distanceToMaxHeight = windTopHeight - characterController.transform.position.y;
        
        if (movementScript != null) //&& !movementScript.isWindTop)
        {
            movementScript.isLifting = true;
            movementScript.ApplyLift(liftForce, windTopHeight);
        }
    }

    private void StopLiftForPlayer(CharacterController characterController)
    {
        PlayerMovement movementScript = characterController.GetComponent<PlayerMovement>();
        if (movementScript != null)
        {
            Debug.Log("Player has left the wind's effect range, stopping lift.");
            
            movementScript.StopLift();

        }
    }

    // Clears Effects When Extinguished
    public void Extinguish()
    {
        if (isIgnited)
        {
            if (currentIgnitionMagic != null)
            {
                Destroy(currentIgnitionMagic);
            }
            ResetOtherIgnitions();
            initialFire.SetActive(true);
            currentIgnitionMagic = null;
            currentEffectColor = null;
            isIgnited = false;
            Debug.Log($"{gameObject.name} has been extinguished!");

            // Reset material, collider, and forces
            innerRenderer.material = initialMaterial;
            marbleCollider.material = null;
            //isPushing = false;

            transform.SetParent(null);
            //
            marbleRigidbody.useGravity = true;
            marbleRigidbody.isKinematic = false;
            

            // Stop lift for all players in range
            foreach (CharacterController player in playersInRange)
            {
                StopLiftForPlayer(player);
            }
        }
    }

    // Checks if player is within the ignition range
    public bool IsInRange(Transform player)
    {
        return Vector3.Distance(transform.position, player.position) <= ignitionRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 检查是否与带有灯座标签的物体碰撞，并且 marble 已经熄灭
        if (other.CompareTag(marbleStandTag)&& !isIgnited && !other.GetComponent<MarbleStand>().isOccupied())
        {
            // 将 marble 设为灯座的子对象
            transform.SetParent(other.transform);
            marbleRigidbody.isKinematic = true;
            // 重置位置到灯座的中心
            transform.localPosition = new Vector3(-0.65f, -1.03f, 0.05f);

        }
    }
}
