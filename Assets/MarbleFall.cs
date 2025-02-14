using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleFall : MonoBehaviour
{
    public Stickrotation stickrotation; // 旋转控制对象
    public GameObject marble;           // Marble 对象

    private Rigidbody marbleRigidbody;  // Marble 的 Rigidbody
    private bool hasGivenVelocity = false;
    void Start()
    {
        // 检查 marble 是否赋值
        if (marble == null)
        {
            Debug.LogError("Marble is not assigned in the Inspector!");
            return; // 停止执行后续代码
        }

        // 获取 Rigidbody 组件
        marbleRigidbody = marble.GetComponent<Rigidbody>();
        if (marbleRigidbody == null)
        {
            Debug.LogError("Marble does not have a Rigidbody component!");
        }

        // 检查 stickrotation 是否赋值
        if (stickrotation == null)
        {
            Debug.LogError("Stickrotation is not assigned in the Inspector!");
        }
    }

    void Update()
    {
        // 确保 stickrotation 和 marbleRigidbody 已正确初始化
        if (stickrotation == null || marbleRigidbody == null)
        {
            return;
        }

        // 当 stickrotation.isRotated 为 true 且还未设置速度时
        if (stickrotation.isRotated && !hasGivenVelocity)
        {
            // 给 marble 一个微微向上的速度
            marbleRigidbody.velocity = new Vector3(
                marbleRigidbody.velocity.x, // 保留当前 x 方向速度
                0.2f,                       // 设置微微向上的速度
                marbleRigidbody.velocity.z  // 保留当前 z 方向速度
            );

            // 确保只执行一次
            hasGivenVelocity = true;
        }
    }
}
