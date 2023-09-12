using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActiveOnClick : MonoBehaviour
{
    public GameObject targetObject;  // ��ק����Ҫ�л��״̬��GameObject������

    // ����ť�����ʱ���ô˷���
    public void ToggleActiveStatus()
    {
        if (targetObject != null)
            targetObject.SetActive(!targetObject.activeSelf);  // �л��״̬
    }
}
