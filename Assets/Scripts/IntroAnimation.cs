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
        // 开始时确保黑幕和你的图片都是不透明的
        blackScreen.color = Color.black;
        yourImage.color = new Color(1, 1, 1, 0); // 设置为透明

        // 图片渐入
        yourImage.DOFade(1f, imageFadeInDuration).OnComplete(() =>
        {
            // 延迟后淡出
            DOVirtual.DelayedCall(delayBeforeFadeOut, () =>
            {
                // 在此添加了一个叫做Sequence的新功能，它允许你链接多个Tweens
                Sequence s = DOTween.Sequence();

                s.Append(yourImage.DOFade(0f, fadeOutDuration));
                s.Join(blackScreen.DOFade(0f, fadeOutDuration));

                // 在所有动画完成后，设置Image的active为false
                s.OnComplete(() =>
                {
                    blackScreen.gameObject.SetActive(false);
                    yourImage.gameObject.SetActive(false);
                });
            });
        });
    }
}
