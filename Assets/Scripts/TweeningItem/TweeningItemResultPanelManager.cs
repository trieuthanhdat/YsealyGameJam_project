using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class TweeningItemResultPanelManager : MonoBehaviour
{
    [System.Serializable]
    public class ResultItemUI
    {
        public TweeningItemResultPanelItem item;
        [Range(0, 20)]
        public int sequenceOrder;
    }
    public bool PlaySequenceOnAwake = false;
    public float delayBetweenGroups = 0.2f;
    public float delayBetweenItemOfGroup = 0.2f;
    public List<ResultItemUI> targetItems;
    bool havingReward = false;
    public void OnEnable()
    {
        if (PlaySequenceOnAwake)
            PlaySequenceAnimations();
    }

    public void PlaySequenceAnimations()
    {
        // Sort targetItems by sequenceOrder
        targetItems.Sort((a, b) => a.sequenceOrder.CompareTo(b.sequenceOrder));

        Sequence sequence = DOTween.Sequence();
        List<ResultItemUI> currentGroup = new List<ResultItemUI>();
        int currentSequenceOrder = -1;

        foreach (ResultItemUI resultItem in targetItems)
        {
            if (resultItem.sequenceOrder != currentSequenceOrder)
            {
                // Start a new group
                if (currentGroup.Count > 0)
                {
                    // Append the group to the sequence
                    sequence.Join(CreateSequenceFromGroup(currentGroup, delayBetweenItemOfGroup));
                    sequence.AppendInterval(delayBetweenGroups);
                }

                currentGroup.Clear();
                currentSequenceOrder = resultItem.sequenceOrder;
            }
            // Add the item to the current group
            currentGroup.Add(resultItem);
        }

        // Append the last group to the sequence
        if (currentGroup.Count > 0)
        {
            sequence.Join(CreateSequenceFromGroup(currentGroup));
        }
        sequence.Play();
    }

    private Sequence CreateSequenceFromGroup(List<ResultItemUI> group, float gapTime = 0f)
    {
        if (group.Count <= 0 || group == null) return DOTween.Sequence();
        Sequence groupSequence = DOTween.Sequence();
        try
        {

            foreach (ResultItemUI resultItem in group)
            {
                Tween tween = resultItem.item.GetItemTween(resultItem.item.gameObject, resultItem.item.OriginScale, resultItem.item.OriginPosition);
                if (tween != null)
                {
                    groupSequence.Join(tween);
                }
            }

            if (gapTime > 0f && group.Count > 1)
            {
                groupSequence.AppendInterval(gapTime);
            }
        }catch(Exception ex)
        {
            Debug.Log("TWEENINGITEMRESULTPANELMANAGER: exception " + ex);
        }

        return groupSequence;
    }


    public void PlayAnimationSequenceOfReward()
    {

        if (targetItems.Count <= 0 || targetItems == null) return;
        try
        {

            List<ResultItemUI> itemList = new List<ResultItemUI>();
            Sequence sequence = DOTween.Sequence();
            foreach (ResultItemUI targetItem in targetItems)
            {
                if (targetItem.item.textNumType == TweeningItemResultPanelItem.TextType.SHARDREWARD ||
                    targetItem.item.textNumType == TweeningItemResultPanelItem.TextType.TICKETREWARD)
                    itemList.Add(targetItem);
            }
            sequence = CreateSequenceFromGroup(itemList);
            sequence.Play();
        }catch(Exception ex)
        {
            Debug.Log("TWEENINGITEMRESULTPANELMANAGER: exception "+ ex);
        }

    }

    //public bool HavingReward()
    //{
    //    foreach (ResultItemUI targetItem in targetItems)
    //    {
    //        if (targetItem.item.textNumType == TweeningItemResultPanelItem.TextType.SHARDREWARD ||
    //            targetItem.item.textNumType == TweeningItemResultPanelItem.TextType.TICKETREWARD)
    //        {
    //            if (targetItem.item.ProcessShardRewardAmount() != 0 ||
    //                targetItem.item.ProcessTicketRewardAmount() != 0)
    //            {
    //                return true;
    //            }
    //        }
    //    }
    //    return false;
    //}

    public void PlayAnimationSequenceOfAType(TweeningItem.TweeningType tweeningType)
    {
        List<ResultItemUI> itemsToAnimate = targetItems.Where(item => item.item.tweeningType == tweeningType).ToList();

        if (itemsToAnimate.Count > 0)
        {
            Sequence sequence = DOTween.Sequence();

            foreach (ResultItemUI resultItem in itemsToAnimate)
            {
                Tween tween = resultItem.item.GetItemTween(resultItem.item.gameObject, resultItem.item.OriginScale, resultItem.item.OriginPosition);
                if (tween != null)
                {
                    sequence.Join(tween);
                }
            }

            sequence.Play();
        }
    }

}
