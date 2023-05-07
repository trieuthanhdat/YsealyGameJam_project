using UnityEngine;

public class MergeObject : MonoBehaviour
{
    public GameObject mergeTarget;
    public int currentLevel = 1;

    private void Start()
    {
        MergeManager.instance.Register(this);
    }

    public void Merge(GameObject otherObject)
    {
        MergeObject otherMergeObject = otherObject.GetComponent<MergeObject>();

        if (otherMergeObject == null)
        {
            return;
        }

        GameObject mergedObject = Instantiate(gameObject, transform.position, Quaternion.identity);
        MergeObject mergedMergeObject = mergedObject.GetComponent<MergeObject>();

        mergedMergeObject.currentLevel = Mathf.Max(currentLevel, otherMergeObject.currentLevel) + 1;

        Destroy(gameObject);
        Destroy(otherObject);
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z;
            transform.position = mousePosition;
            MergeManager.instance.SelectObject(this);
        }
    }

    private void OnMouseUp()
    {
        MergeObject selectedObject = MergeManager.instance.selectedObject;
        if (selectedObject != null && selectedObject != this && selectedObject.mergeTarget == gameObject)
        {
            MergeManager.instance.MergeObjects(selectedObject, this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (mergeTarget != null && collision.gameObject == mergeTarget)
        {
            Merge(collision.gameObject);
        }
    }
}
