using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stickrotation : MonoBehaviour
{
    public GameObject leftMarbleStand;
    public GameObject rightMarbleStand;
    public float tiltAngle = 45f;
    public bool isRotated = false;
    private Quaternion initialRotation; // 保存初始旋转

    void Start()
    {
        initialRotation = transform.rotation; // 在 Start 中保存初始旋转值
    }

    void Update()
    {
        bool leftMarbleActive = IsYellowWind(leftMarbleStand);
        bool rightMarbleActive = IsYellowWind(rightMarbleStand);

        // 调试信息，查看左右两边的点燃状态
        //Debug.Log($"Left Marble Active: {leftMarbleActive}, Right Marble Active: {rightMarbleActive}");

        UpdateTilt(leftMarbleActive, rightMarbleActive);
    }

    bool IsYellowWind(GameObject marbleStand)
    {
        foreach (Transform child in marbleStand.GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag("Marble"))
            {


                Ignitable marbleScript = child.GetComponent<Ignitable>();
                if (marbleScript != null && marbleScript.isYellowIgnited)
                {
                   // Debug.Log($"Marble {child.name} is yellow ignited.");
                    return true;
                }
            }
        }
        return false;
    }

    void UpdateTilt(bool leftMarbleActive, bool rightMarbleActive)
    {
        if (leftMarbleActive && !rightMarbleActive)
        {
            // 左边点燃，向左上倾斜60度
            transform.rotation = initialRotation * Quaternion.Euler(-tiltAngle, 0, 0);
            isRotated = true;
           // Debug.Log("Rotating left 60 degrees.");
        }
        else if (!leftMarbleActive && rightMarbleActive)
        {
            // 右边点燃，向右下倾斜60度
            transform.rotation = initialRotation * Quaternion.Euler(tiltAngle, 0, 0);
            isRotated = true;
           // Debug.Log("Rotating right -60 degrees.");
        }
        else
        {
            // 两边都点燃或都未点燃，保持初始水平
            transform.rotation = initialRotation;
            isRotated = false;
            //Debug.Log("Rotation reset to initial.");
        }
    }
}
