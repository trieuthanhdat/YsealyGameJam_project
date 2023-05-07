using UnityEngine;
using System.Collections.Generic;

public class MergeManager : MonoBehaviour
{
    public static MergeManager instance;

    private List<MergeObject> mergeObjects = new List<MergeObject>();
    public MergeObject selectedObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Register(MergeObject mergeObject)
    {
        mergeObjects.Add(mergeObject);
    }

    public void SelectObject(MergeObject mergeObject)
    {
        selectedObject = mergeObject;
    }

    public void MergeObjects(MergeObject mergeObject1, MergeObject mergeObject2)
    {
        mergeObject1.Merge(mergeObject2.gameObject);
        selectedObject = null;
    }
}
