using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeObjectPool : MonoBehaviour
{
    public MergeObject mergeObjectPrefab;
    public int poolSize;
    private List<MergeObject> mergeObjectPool = new List<MergeObject>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            MergeObject mergeObject = Instantiate(mergeObjectPrefab, Vector3.zero, Quaternion.identity);
            mergeObjectPool.Add(mergeObject);
            mergeObject.gameObject.SetActive(false);
        }
    }

    public MergeObject GetMergeObject()
    {
        MergeObject mergeObject = mergeObjectPool.Find(x => !x.gameObject.activeSelf);

        if (mergeObject == null)
        {
            mergeObject = Instantiate(mergeObjectPrefab, Vector3.zero, Quaternion.identity);
            mergeObjectPool.Add(mergeObject);
        }

        mergeObject.gameObject.SetActive(true);
        return mergeObject;
    }


}
