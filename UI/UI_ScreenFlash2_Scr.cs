using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScreenFlash2_Scr : MonoBehaviour
{
    [SerializeField] private float fadeInTime, pauseTime, fadeOutTime;

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        StartAnimationSequence();
    }

    private void StartAnimationSequence()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(image.DOFade(1f, fadeInTime).SetEase(Ease.OutQuart));
        sequence.AppendInterval(pauseTime);
        sequence.Append(image.DOFade(0f, fadeOutTime).SetEase(Ease.InQuart));
        sequence.AppendCallback(() => { image.color = new Color(1, 1, 1, 1); Destroy(gameObject); });
    }
    public void SetTimes(float inTime, float pause, float outTime)
    {
        fadeInTime = inTime;
        pauseTime = pause;
        fadeOutTime = outTime;
    }
}
