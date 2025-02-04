
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCamera : MonoBehaviour
{
    
    public string lineTag = "Line"; // 用于识别线条的Tag
    public float cameraHeight = 10f; // 相机固定高度
    public float rotationSpeed = 30f; // 旋转速度
    public float rotationRadius = 10f; // 旋转半径

    private Vector3 centerPoint; // 线条的中心点

    void Start()
    {
        AdjustCameraToLines();
    }

    void Update()
    {
        RotateCameraAroundCenter();
    }

    private void AdjustCameraToLines()
    {
        Bounds lineBounds = new Bounds(Vector3.zero, Vector3.zero);
        bool hasLines = false;

        // 找到所有线条对象并计算其包围盒
        foreach (GameObject lineObj in GameObject.FindGameObjectsWithTag(lineTag))
        {
            Renderer renderer = lineObj.GetComponent<Renderer>();
            if (renderer != null)
            {
                if (!hasLines)
                {
                    lineBounds = renderer.bounds;
                    hasLines = true;
                }
                else
                {
                    lineBounds.Encapsulate(renderer.bounds);
                }
            }
        }

        if (hasLines)
        {
            // 计算线条的中心点
            centerPoint = lineBounds.center;

            // 设置初始相机位置
            gameObject.transform.position = centerPoint + new Vector3(rotationRadius, cameraHeight, 0);
            gameObject.transform.LookAt(centerPoint);
        }
    }

    private void RotateCameraAroundCenter()
    {
        if (gameObject != null)
        {
            // 按指定速度围绕中心旋转
            gameObject.transform.RotateAround(centerPoint, Vector3.up, rotationSpeed * Time.deltaTime);

            // 始终朝向中心点
            gameObject.transform.LookAt(centerPoint);
        }
    }
}
