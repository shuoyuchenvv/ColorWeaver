using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipegreener : MonoBehaviour
{
    // Start is called before the first frame update
 
    public Material greenpipeMaterial; // 绿色材质，需在 Inspector 中指定

    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Marble"))
        {
            // 将对象的材质更改为绿色
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null && greenpipeMaterial != null)
            {
                renderer.material = greenpipeMaterial;
                Debug.Log("Material changed to green on collision with marble.");
            }
            else
            {
                Debug.LogWarning("Renderer or greenMaterial is missing.");
            }
        }
    }
}


