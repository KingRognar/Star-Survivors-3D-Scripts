using DG.Tweening;
using UnityEngine;

public class UI_ScreenFlash_Scr : MonoBehaviour
{
    [SerializeField] private Transform imageTrans, maskTrans;

    [SerializeField] private float firstTime, pauseTime, secondTime;

    private void Start()
    {
        imageTrans.localScale = Vector3.zero;
        maskTrans.localScale = Vector3.zero;
        StartAnimationSequence();
    }
    
    private void StartAnimationSequence()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(imageTrans.DOScale(Vector3.one * 5, firstTime).SetEase(Ease.InQuad));
        sequence.AppendInterval(pauseTime);
        sequence.Append(maskTrans.DOScale(Vector3.one * 10, secondTime).SetEase(Ease.InQuad));
        sequence.AppendCallback(() => { Destroy(gameObject); });
    }
}
