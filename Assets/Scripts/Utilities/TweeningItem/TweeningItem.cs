using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using System;

public class TweeningItem : MonoBehaviour
{
    public enum TweeningType
    {
        DoScale,
        DoMoveFromLeft,
        DoMoveFromRight,
        DoMoveFromTop,
        DoMoveFromBottom,
        DoZoomOut,
        DoZoomOutAndIn,
        DoFlyOut,
        DoFadeIn,
        DoScaleX,
        DoScaleY,
        DoScoreRun,
        None,
        DoFadeOut,
        DoZoomInOutOvertime
    }
    public TweeningType tweeningType = TweeningType.DoScale;
    public bool PlayOnAwake = false;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private Vector2 fromScale = Vector2.one;
    [SerializeField] private Vector2 toScale = Vector2.one;
    [Tooltip("Tick this if you want to set auto scale. This is used for Scaling options only")]
    [SerializeField] bool toCurrentScale = false;
    [SerializeField] private Ease easeInMode;
    [SerializeField] private Ease easeOutMode;
    [SerializeField] private bool autoClearTweeningCache = false;

    private Tween iTween;
    private bool hasFinishedEffect = false;
    private Vector3 originScale = Vector3.one;
    private Vector3 originPosition = Vector3.one;

    public Vector3 OriginScale { get => originScale; set => originScale = value; }
    public Vector3 OriginPosition { get => originPosition; set => originPosition = value; }

    public bool ToCurrentScale { get => toCurrentScale; }
    public bool HasFinishedEffect => hasFinishedEffect;

    public float FadeTime { get => fadeTime; set => fadeTime = value; }

    public Vector2 ToScale { get => toScale; set => toScale = value; }
    public Vector2 FromScale { get => fromScale; set => fromScale = value; }

    public Ease EaseInMode { get => easeInMode; set => easeInMode = value; }
    public Ease EaseOutMode { get => easeOutMode; set => easeOutMode = value; }

    public Tween ITween { get => iTween; set => iTween = value; }


    public virtual void Awake()
    {
        Validate();
    }
    public virtual void OnEnable()
    {
        if (tweeningType == TweeningType.None)
            return;

        if (autoClearTweeningCache)
            ClearCached();

        if (PlayOnAwake)
        {
            StartCoroutine(DOItemAnimation(gameObject, true));
        }
    }
    public virtual void Validate()
    {
        GetTransformScaleAndPosition();
    }
    private void GetTransformScaleAndPosition()
    {
        originPosition = transform.position;
        originScale = transform.localScale;
        //try
        //{
        //    if (GetComponent<IpadContentSettings>() != null)
        //    {
        //        if (GetComponent<IpadContentSettings>().SetNewScale == true)
        //        {
        //            originScale = GetComponent<IpadContentSettings>().newScale;
        //        }
        //    }

        //    if (toCurrentScale)
        //        ToScale = originScale;
        //}
        //catch (Exception ex)
        //{
        //    Debug.Log("TWEENING ITEM: exception "+ ex);
        //}
        
        
    }
    #region
    //=====>TWEENING ANIM<=====//
    public virtual IEnumerator DOItemAnimation(GameObject item = null, bool shouldChangesize = false, TweeningType newType = TweeningType.None, bool canPlay = false)
    {
        Vector3 originScale = item.transform.localScale;
        Vector3 originPosition = item.transform.position;

        if(shouldChangesize)
            gameObject.transform.localScale = fromScale;

        yield return new WaitForSeconds(0.001f);

        iTween = GetItemTween(item, originScale, originPosition, newType);
        if (canPlay)
            iTween.Play();
    }
    public virtual Tween GetItemTween(GameObject item = null, Vector3 originScale = new Vector3(), Vector3 originPosition = new Vector3(), TweeningType newType = TweeningType.None)
    {

        //Default-DOScale
        Tween iTween = item.transform.DOScale(toScale, fadeTime).SetEase(easeInMode)
                .OnComplete(() => DOScaleBackToOriginalSize(item, originScale, fadeTime));
        TweeningType tweeningType = this.tweeningType;
        if (newType != TweeningType.None)
        {
            tweeningType = newType;
        }
        switch (tweeningType)
        {
            case TweeningType.DoFlyOut:
                iTween = item.transform.DOScale(0, fadeTime).SetEase(easeInMode);
                break;
            case TweeningType.DoZoomOutAndIn:
                iTween = item.transform.DOScale(toScale, fadeTime).SetEase(easeInMode)
                .OnComplete(() => DOScaleBackToOriginalSize(item, originScale, fadeTime));
                break;
            case TweeningType.DoFadeIn:

                break;
            case TweeningType.DoFadeOut:
                if (item == null)
                    item = gameObject;
                if (item.GetComponent<CanvasGroup>())
                    iTween = item.GetComponent<CanvasGroup>().DOFade(0, fadeTime).SetEase(easeOutMode)
                             .OnComplete(() => ProcessAfterFadingOut(item)) ;
                break;
            case TweeningType.DoScale:
                break;
            case TweeningType.DoZoomInOutOvertime:
                if (item == null)
                    item = gameObject;
                Vector2 originalScale = item.transform.localScale; // Store the original scale
                Vector2 targetScale = originalScale * toScale; // Calculate the target scale
                                                               // Create the zoom in tween
                Tween zoomInTween = item.transform.DOScale(targetScale, fadeTime / 2).SetEase(easeInMode);

                // Create the zoom out tween
                Tween zoomOutTween = item.transform.DOScale(originalScale, fadeTime / 2).SetEase(easeOutMode)
                    .SetDelay(fadeTime / 2);

                // Create a sequence of tweens
                Sequence zoomInOutSequence = DOTween.Sequence();

                // Add the zoom in and out tweens to the sequence
                zoomInOutSequence.Append(zoomInTween).Append(zoomOutTween).SetLoops(-1);

                return zoomInOutSequence;

            case TweeningType.DoMoveFromLeft:
                item.transform.position = new Vector2(originPosition.x - 1500f, originPosition.y);
                iTween = item.transform.DOLocalMoveX(originPosition.x, fadeTime).SetEase(easeInMode).OnComplete(() => hasFinishedEffect = true);
                break;
            case TweeningType.DoZoomOut:
                iTween = item.transform.DOScale(ToScale, fadeTime).SetEase(easeInMode).OnComplete(() => hasFinishedEffect = true);
                break;
            default:
                break;

        }
        return iTween;
    }
    public virtual void StopTweeningEffect()
    {
        if (iTween.IsActive() && iTween != null)
            iTween.Kill();

    }
    public virtual void ProcessAfterFadingOut(GameObject item)
    {
        item.SetActive(false);
        item.GetComponent<CanvasGroup>().alpha = 1;
    }
    public virtual IEnumerator DOItemsAnimation(GameObject[] items = null)
    {
        yield return new WaitForSeconds(0.1f);
        switch (tweeningType)
        {
            case TweeningType.DoScale:
                foreach (var item in items)
                {
                    item.transform.DOScale(toScale, fadeTime).SetEase(easeOutMode);
                }
                yield return new WaitForSeconds(1f);

                foreach (var item in items)
                {
                    Vector3 originScale = item.transform.localScale;
                    DOScaleBackToOriginalSize(item, originScale, fadeTime);
                }
                break;
            //case TweeningType.DoMove:
                //Debug.Log("TWEENING ITEM: not yet support please add more code");
                //break;
        }
       
    }
    public virtual void ResetOriginSize(GameObject gameObject)
    {
        gameObject.transform.localScale = fromScale;
        Debug.Log("TWEENING ITEM: reset to scale " + gameObject.transform.localScale);
    }
    public virtual IEnumerator SetActiveAftertime(GameObject gameObject, float time, bool isActive)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(isActive);
    }

    public virtual void DOScaleBackToOriginalSize(GameObject item, Vector3 originScale, float fadeTime)
    {
        item.transform.DOScale(originScale, fadeTime).SetEase(easeOutMode);
        //Clearing cache
        if (autoClearTweeningCache)
            ClearCached();
    }
    public void ClearCached()
    {
        DOTween.ClearCachedTweens();
        DOTween.Kill(this);
    }

    
    #endregion


}
