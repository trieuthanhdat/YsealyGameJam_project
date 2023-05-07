using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TweeningItemButton : TweeningItem
{
    [Header("TWEENING ITEM BUTTONS")]
    [SerializeField] bool isLoop = false;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] float delay = 1f;
    Vector3 originPos = Vector3.zero;
    Vector3 originScale = Vector3.one;

    public float Delay
    {
        get { return delay; }
        set { delay = value; }
    }
    public override void Awake()
    {
        originPos = rectTransform.transform.localPosition;
        originScale = rectTransform.transform.localScale;
        Validate();

    }

    public override void Validate()
    {
        base.Validate();
        switch (tweeningType)
        {
            case TweeningType.DoFlyOut:
                break;
            case TweeningType.DoZoomOutAndIn:
                break;
            
            case TweeningType.DoScale:
                canvasGroup.alpha = 0;
                rectTransform.localScale = FromScale;
                break;
            case TweeningType.DoMoveFromBottom:
                canvasGroup.alpha = 0;
                rectTransform.transform.localPosition = new Vector3(transform.localPosition.x, -500f, transform.localPosition.z);
                break;
            case TweeningType.DoZoomOut:
                break;
        }
    }


    public override IEnumerator DOItemAnimation(GameObject item, bool shouldChangesize = false, TweeningType newType = TweeningType.None, bool canPlay = false)
    {
        yield return new WaitForSeconds(delay);

        switch (tweeningType)
        {
            case TweeningType.DoFadeOut:
                DOFadeOut();
                break;
            case TweeningType.DoZoomOutAndIn:
                break;
            case TweeningType.DoScale:
                DOScaleUp();
                break;
            case TweeningType.DoMoveFromBottom:
                DOFadeIn();
                break;
            case TweeningType.DoZoomOut:
                break;
        }
    }

    private void DOScaleUp()
    {
        rectTransform.DOScale(ToScale, FadeTime).SetEase(EaseInMode);
        canvasGroup.DOFade(1, FadeTime);
    }

    private void DOFadeIn()
    {
        rectTransform.DOAnchorPos(originPos, FadeTime, false).SetEase(EaseInMode);
        canvasGroup.DOFade(1, FadeTime);
    }
    private void DOFadeOut()
    {
        canvasGroup.DOFade(0, FadeTime).OnComplete(ResetAlphaAfterFadeOut);
    }
    private void ResetAlphaAfterFadeOut()
    {
        canvasGroup.alpha = 1;
    }
}
