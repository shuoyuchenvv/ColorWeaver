using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public CloudyLineDrawer linedrawer;
    public float fartDelayTime = 0.2f;
    public float fartOffSet = 0.1f;
    public float speed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public Transform playerCamera;
    public float mouseSensitivity = 100f;
    public float cameraDistance = 5f;
    public float verticalAngleLimit = 80f;
    public InstructionTutorial instructionTutorial;

    private Vector3 velocity;
    private bool isGrounded;
    private float currentX = 0f;
    private float currentY = 0f;
    private Animator animator;
    private bool isGravityEnabled = true;
    public bool isWindTop = false;

    public bool isLifting = false;
    public float yellowMaxHeight = 144f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        animator = GetComponent<Animator>();

        currentX = 180f;
        currentY = 10f;

        Quaternion initialRotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 cameraPosition = transform.position - (initialRotation * Vector3.forward * cameraDistance);
        playerCamera.position = cameraPosition;
        playerCamera.LookAt(transform.position + Vector3.up * 1.5f);
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        // 更新 isGrounded 状态
        isGrounded = controller.isGrounded;

        // 当在地面且向下移动时，设置一个小的向下速度
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 获取水平输入
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // 计算基于相机方向的移动向量
        Vector3 move = playerCamera.forward * z + playerCamera.right * x;
        move.y = 0f;
        controller.Move(move * speed * Time.deltaTime);

        // 更新动画的 "Speed" 参数
        animator.SetFloat("Speed", move.magnitude);

        // 旋转玩家面对移动方向
        if (move.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // 如果处于地面且跳跃按键被按下，触发跳跃
        if ((isGrounded || isWindTop) && Input.GetButtonDown("Jump") && instructionTutorial.jumpActivated)
        {
            
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isGrounded = false; // 立即取消着地状态，防止重复跳跃

            if (linedrawer.energyBar.HasEnoughEnergy(linedrawer.energyCostPerSet)&& !linedrawer.energyBar.isRegenerating)
            {
                StartCoroutine(DelayedFart(x,z));
            }

        }

        IEnumerator DelayedFart(float x, float z)
        {

            yield return new WaitForSeconds(fartDelayTime); // 等待0.1秒
            if (Mathf.Abs(x) <= fartOffSet && Mathf.Abs(z) <= fartOffSet)
            {
                linedrawer.JustFart(); // 延迟后执行
            }
        }

        //如果启用了重力，则持续应用重力
        if (isGravityEnabled)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // 应用垂直方向的移动
        controller.Move(velocity * Time.deltaTime);


    }

    public void ApplyLift(float liftForce, float windTopHeight)
    {
        if (!isLifting) return;

        float distanceToMaxHeight = windTopHeight - transform.position.y;
        isGravityEnabled = false; // 悬停时禁用重力

        if (distanceToMaxHeight > 5f)
        {
            // 当距离目标高度大于5时，正常上升
            
                velocity.y = liftForce;
           
        }
        else if (distanceToMaxHeight <= 5f && distanceToMaxHeight >-5)
        {
            // 在接近最大高度时平滑减速
            float lerpFactor = Mathf.InverseLerp(5f, 0f, distanceToMaxHeight);
            velocity.y = Mathf.Lerp(liftForce, 0, lerpFactor);
            isWindTop = true;

        
        }



        // 移动角色
        controller.Move(velocity * Time.deltaTime);
    }

    public void StopLift()
    {
        isLifting = false;
        isGravityEnabled = true;
        isWindTop = false;
        

        // 在空中时平滑下降
        if (!controller.isGrounded)
        {
            //velocity.y = -2f; // 设置一个小的负值模拟下落
            Debug.Log("Player falling after lift stopped.");
        }
        else
        {
            velocity.y = 0;
        }
    }

}
