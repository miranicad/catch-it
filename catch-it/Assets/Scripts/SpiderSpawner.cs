using UnityEngine;

public class SpiderSpawner : MonoBehaviour
{
    public GameObject spiderPrefab;
    public Transform[] spawnPoints;
    public int spiderCount = 5;

    void Start()
    {
        SpawnSpiders();
    }

    void SpawnSpiders()
    {
        for (int i = 0; i < spiderCount; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(spiderPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}