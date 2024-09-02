using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleAnimation : MonoBehaviour
{
    private void Start()
    {
        this.transform.localScale = Vector3.zero;

        this.transform.DOScale(1, 0.6f) 
            .SetEase(Ease.OutElastic, 1f, 0.5f);
    }
}
