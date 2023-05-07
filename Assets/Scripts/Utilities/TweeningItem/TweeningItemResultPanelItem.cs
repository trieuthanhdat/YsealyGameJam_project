using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using static TweeningItemResultPanelItem;

public class TweeningItemResultPanelItem : TweeningItem
{
    [Header("FOR FADING EFFECTS")]
    [SerializeField] CanvasGroup targetCanvasGroup;
    [Header("FOR MOVEMENT EFFECTS")]
    [SerializeField] RectTransform rectTransform;
    public enum TextType
    {
        SCORE,
        HIGHSCORE,
        PERFECT,
        GREAT,
        GOOD,
        MISS,
        COMBO,
        SHARDREWARD,
        TICKETREWARD,
        NONE
    }
    public TextType textNumType;

    
    int digitCheckIndex = 1;//1 means 1 digit, 3 means 3 digits, 6 means 6 digits

    public override void Validate()
    {
        base.Validate();
        ValidateNumberOfDigits();
        switch (tweeningType)
        {
            case TweeningType.DoFadeIn:
                targetCanvasGroup.alpha = 0;
                break;
            case TweeningType.DoScale:
                transform.localScale = Vector2.zero;
                break;
            case TweeningType.DoScaleX:
                transform.localScale = new Vector2(0, transform.localScale.y);
                break;
            case TweeningType.DoScaleY:
                transform.localScale = new Vector2(transform.localScale.y, 0);
                break;
            case TweeningType.DoMoveFromTop:
                if (!targetCanvasGroup) return;
                targetCanvasGroup.alpha = 0;
                transform.position = new Vector3(OriginPosition.x, OriginPosition.y + 50f, OriginPosition.z);
                break;
            case TweeningType.DoScoreRun:
                //if (!resultController) return;
                //if (digitCheckIndex == 1)
                //    textNumber.text = "+" + RichTextFormatHelper.instance.RichTextFormat(0, 1);
                //else
                //    textNumber.text = RichTextFormatHelper.instance.RichTextFormat(0, digitCheckIndex);
                break;
        }
    }


    public override IEnumerator DOItemAnimation(GameObject item = null, bool shouldChangesize = false, TweeningType newType = TweeningType.None, bool canPlay = false)
    {
        yield return new WaitForSeconds(0.05f);
        ITween = GetItemTween(gameObject, OriginScale, OriginPosition);
    }

    public override Tween GetItemTween(GameObject item = null, Vector3 originScale = default, Vector3 originPosition = default, TweeningType newType = TweeningType.None)
    {
        //Default-DOScale

        Tween iTween = rectTransform.DOScale(ToScale, FadeTime).SetEase(EaseInMode);

        switch (tweeningType)
        {
            //Default-DoScale
            case TweeningType.DoScale:
                iTween = rectTransform.DOScale(ToScale, FadeTime).SetEase(EaseInMode);
                break;
            case TweeningType.DoFadeIn:
                if (targetCanvasGroup)
                {
                    iTween = targetCanvasGroup.DOFade(1, FadeTime).SetEase(EaseInMode);
                }
                break;
            case TweeningType.DoScaleX:
                iTween = rectTransform.DOScaleX(ToScale.x, FadeTime).SetEase(EaseInMode);
                break;
            case TweeningType.DoScaleY:
                iTween = rectTransform.DOScaleY(ToScale.y, FadeTime).SetEase(EaseInMode);
                break;
            case TweeningType.DoMoveFromTop:
                if (!rectTransform || !targetCanvasGroup) break;
                // Create the DoTween sequence
                Sequence sequence = DOTween.Sequence();
                // Add the RectTransform's anchored position tween
                Tween anchorPosTween = rectTransform.DOAnchorPos(OriginPosition, FadeTime).SetEase(EaseInMode);
                sequence.Join(anchorPosTween);
                Tween fadeTween = targetCanvasGroup.DOFade(1, FadeTime);
                sequence.Join(fadeTween);
                iTween = sequence;
                break;
            case TweeningType.DoScoreRun:
                //REPLACE RESULTSCENECONTROLLER
                //if(resultController)
                    //iTween = ProcessNumberAnimation();
                break;
        }
        return iTween;
    }

    //public Tween ProcessNumberAnimation()
    //{
    //    int endValue;
    //    string numText = GetTextNumType();
    //    if (!int.TryParse(numText, out endValue))
    //    {
    //        // If the string can't be converted to an integer, set the start value to 0
    //        endValue = 0;
    //    }
    //    return DOTween.To(() => 0, x =>
    //    {
    //        if (digitCheckIndex == 1)
    //            textNumber.text = "+" + x.ToString();
    //        else
    //            textNumber.text = RichTextFormatHelper.instance.RichTextFormat(x, digitCheckIndex);

    //    }, endValue, FadeTime).SetEase(EaseInMode);
    //}

    public void ValidateNumberOfDigits()
    {
        if (textNumType == TextType.SHARDREWARD || textNumType == TextType.TICKETREWARD)
            digitCheckIndex = 1;
        else if(textNumType == TextType.PERFECT ||
                textNumType == TextType.GOOD    ||
                textNumType == TextType.GREAT   ||
                textNumType == TextType.MISS)
            digitCheckIndex = 3;
        else if(textNumType == TextType.COMBO)
            digitCheckIndex = 4;
        else
            digitCheckIndex = 6;
    }

    //public string GetTextNumType()
    //{
    //    string txtType = "0";
    //    switch(textNumType)
    //    {
    //        case TextType.SCORE:
    //            txtType = Counter.score.ToString();
    //            break;
    //        case TextType.HIGHSCORE:
    //            PlayerSongModel songmodal = null;
    //            if (PlayerDataService.Instance.GetPlayerData().SongData.TryGetValue(GlobalService.Instance.CurrentSongPlay.id, out songmodal))
    //                txtType = songmodal.score.ToString();
    //            else
    //                txtType = Counter.score.ToString();
    //            break;
    //        case TextType.GOOD:
    //            txtType = Counter.bad.ToString();
    //            break;
    //        case TextType.GREAT:
    //            txtType = Counter.good.ToString();
    //            break;
    //        case TextType.MISS:
    //            txtType = Counter.miss.ToString();
    //            break;
    //        case TextType.PERFECT:
    //            txtType = Counter.pefect.ToString();
    //            break;
    //        case TextType.COMBO:
    //            txtType = Counter.combo_streak.ToString();
    //            break;
    //        case TextType.SHARDREWARD:
    //            txtType = ProcessShardRewardAmount().ToString();
    //            break;
    //        case TextType.TICKETREWARD:
    //            txtType = ProcessTicketRewardAmount().ToString();
    //            break;
    //    }
    //    return txtType;
    //}


    //public int ProcessShardRewardAmount()
    //{
    //    return resultController.ShardPlusReward;
    //}

    
    //public int ProcessTicketRewardAmount()
    //{
    //    return resultController.TicketPlusReward;
    //}

}
