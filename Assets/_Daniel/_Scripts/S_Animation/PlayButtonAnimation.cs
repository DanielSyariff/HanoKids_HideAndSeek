using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayButtonAnimation : MonoBehaviour
{
    public float bounceScale = 1.1f;
    public float bounceDuration = 0.3f;
    public float bounceReturnDuration = 0.2f;
    public float bounceDelay = 0.5f;

    private Vector3 originalScale;

    void Start()
    {
        
        originalScale = transform.localScale;
        StartBounceAnimation();
    }

    void StartBounceAnimation()
    {
        Sequence bounceSequence = DOTween.Sequence();
        bounceSequence.Append(transform.DOScale(originalScale * bounceScale, bounceDuration).SetEase(Ease.OutQuad));
        bounceSequence.Append(transform.DOScale(originalScale, bounceReturnDuration).SetEase(Ease.InQuad));
        bounceSequence.AppendInterval(bounceDelay);
        bounceSequence.SetLoops(-1, LoopType.Restart);
    }
}
