using UnityEngine;
using DG.Tweening;

public class GhostTrail : MonoBehaviour
{
    private PlayerMovement move;
    private AnimationScript anim;
    private SpriteRenderer sr;
    public Transform ghostsParent;
    public Color trailColor;
    public Color fadeColor;

    public Color N_TrailColor;
    public Color N_FadeColor;

    public Color S_TrailColor;
    public Color S_FadeColor;

    public float ghostInterval;
    public float fadeTime;

    public MagneticPlayer magneticPlayer;

    private void Start()
    {
        anim = FindObjectOfType<AnimationScript>();
        move = FindObjectOfType<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();
        magneticPlayer = FindAnyObjectByType<MagneticPlayer>();
    }

    private void Update()
    {
        if (magneticPlayer.currentPole == MagneticPlayer.PoleType.N_Pole) 
        {
            trailColor = N_TrailColor;
            fadeColor = N_FadeColor;
        }

        if (magneticPlayer.currentPole == MagneticPlayer.PoleType.S_Pole)
        {
            trailColor = S_TrailColor;
            fadeColor = S_FadeColor;
        }
    }

    public void ShowGhost()
    {
        Sequence s = DOTween.Sequence();

        for (int i = 0; i < ghostsParent.childCount; i++)
        {
            Transform currentGhost = ghostsParent.GetChild(i);
            s.AppendCallback(()=> currentGhost.position = move.transform.position);
            s.AppendCallback(() => currentGhost.GetComponent<SpriteRenderer>().flipX = anim.sr.flipX);
            s.AppendCallback(()=>currentGhost.GetComponent<SpriteRenderer>().sprite = anim.sr.sprite);
            s.Append(currentGhost.GetComponent<SpriteRenderer>().material.DOColor(trailColor, 0));
            s.AppendCallback(() => FadeSprite(currentGhost));
            s.AppendInterval(ghostInterval);
        }
    }



    public void FadeSprite(Transform current)
    {
        current.GetComponent<SpriteRenderer>().material.DOKill();
        current.GetComponent<SpriteRenderer>().material.DOColor(fadeColor, fadeTime);
    }

}
