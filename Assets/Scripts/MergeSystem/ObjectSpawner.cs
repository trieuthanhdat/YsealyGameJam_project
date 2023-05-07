using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    public float spawnInterval = 2.0f;
    public float spawnRadius = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnObject", spawnInterval, spawnInterval);
    }

    // Update is called once per frame
    private void SpawnObject()
    {
        int randomIndex = Random.Range(0, objectPrefabs.Length);
        Vector2 randomPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;

        Instantiate(objectPrefabs[randomIndex], randomPosition, Quaternion.identity);
    }

}
