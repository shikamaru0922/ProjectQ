using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroAnimation : MonoBehaviour
{
    public Image blackScreen;
    public Image yourImage;

    public float imageFadeInDuration = 2.0f;
    public float delayBeforeFadeOut = 2.0f;
    public float fadeOutDuration = 1.5f;

    private void Start()
    {
        StartAnimation();
    }


    void StartAnimation()
    {
        // ��ʼʱȷ����Ļ�����ͼƬ���ǲ�͸����
        blackScreen.color = Color.black;
        yourImage.color = new Color(1, 1, 1, 0); // ����Ϊ͸��

        // ͼƬ����
        yourImage.DOFade(1f, imageFadeInDuration).OnComplete(() =>
        {
            // �ӳٺ󵭳�
            DOVirtual.DelayedCall(delayBeforeFadeOut, () =>
            {
                // �ڴ������һ������Sequence���¹��ܣ������������Ӷ��Tweens
                Sequence s = DOTween.Sequence();

                s.Append(yourImage.DOFade(0f, fadeOutDuration));
                s.Join(blackScreen.DOFade(0f, fadeOutDuration));

                // �����ж�����ɺ�����Image��activeΪfalse
                s.OnComplete(() =>
                {
                    blackScreen.gameObject.SetActive(false);
                    yourImage.gameObject.SetActive(false);
                });
            });
        });
    }
}
