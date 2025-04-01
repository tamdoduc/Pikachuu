using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParalaxEffect : MonoBehaviour
{
    public Image imageUI;
    public Vector2 offsetSpeed = new Vector2(0.1f, 0); // Tốc độ di chuyển

    private Material materialInstance;

    void Start()
    {
        // Tạo một bản sao của material để tránh thay đổi toàn bộ UI
        materialInstance = new Material(imageUI.material);
        imageUI.material = materialInstance;
    }

    void Update()
    {
        // Lấy offset hiện tại
        Vector2 offset = materialInstance.GetTextureOffset("_MainTex");
        
        // Thay đổi giá trị offset theo thời gian
        offset += offsetSpeed * Time.deltaTime;
        
        // Gán lại giá trị mới
        materialInstance.SetTextureOffset("_MainTex", offset);
    }
}
