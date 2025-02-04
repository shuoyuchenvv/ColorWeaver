using UnityEngine;

public class EndGame : MonoBehaviour
{
    public Camera endGameCamera; // 俯瞰相机
    public string lineTag = "Line"; // 用于识别线条的Tag
    public float cameraHeight = 10f; // 相机的默认高度
    public float padding = 2f; // 视野边界的额外空间
    public GameObject endScreen;
    private Camera mainCamera; // 主相机

    void Start()
    {
        mainCamera = Camera.main;

        if (endGameCamera != null)
        {
            endGameCamera.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 检查是否是玩家触发
        if (other.CompareTag("Player"))
        {


            ViewLines();
        }
        else
        {
            Debug.LogError("EndGameController is not assigned!");
        }
        
    }


    public void ViewLines()
    {
        // 隐藏所有非线条对象
        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            if (!obj.CompareTag(lineTag)&&!obj.CompareTag("Light")&&!obj.CompareTag("UI"))
            {
                obj.SetActive(false);
            }
        }

        // 启用新相机
        if (endGameCamera != null)
        {
           
            endGameCamera.gameObject.SetActive(true);
            endScreen.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("EndGameCamera is not assigned!");
        }

        // 禁用主相机
        if (mainCamera != null)
        {
            mainCamera.gameObject.SetActive(false);
        }
    }

   
}
