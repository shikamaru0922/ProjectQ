using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActiveOnClick : MonoBehaviour
{
    public GameObject targetObject;  // 拖拽你想要切换活动状态的GameObject到这里

    // 当按钮被点击时调用此方法
    public void ToggleActiveStatus()
    {
        if (targetObject != null)
            targetObject.SetActive(!targetObject.activeSelf);  // 切换活动状态
    }
}
