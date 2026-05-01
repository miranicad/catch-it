using System.Collections.Generic;
using UnityEngine;

public class DynamicSpiderSpawner : MonoBehaviour
{
    [Header("Spider Prefabs")]
    public GameObject cartoonSpiderPrefab;
    public GameObject realisticSpiderPrefab;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    private readonly List<GameObject> spawnedSpiders = new();

    public void SpawnSpiders(LevelConfig config)
    {
        ClearSpiders();

        int amountToSpawn = Mathf.Min(config.MaxActiveSpiders, config.SpidersToCatch);

        for (int i = 0; i < amountToSpawn; i++)
        {
            SpawnSingleSpider(config);
        }
    }

    public void SpawnSingleSpider(LevelConfig config)
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned.");
            return;
        }

        GameObject prefab = GetSpiderPrefab(config.SpiderVisualKind);

        if (prefab == null)
        {
            Debug.LogError($"No prefab assigned for spider type: {config.SpiderVisualKind}");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject spider = SpawnAtSurfaceOrFallback(prefab, spawnPoint);

        ApplyLevelConfigToSpider(spider, config);

        spawnedSpiders.Add(spider);
    }

    public void ClearSpiders()
    {
        foreach (GameObject spider in spawnedSpiders)
        {
            if (spider != null)
            {
                Destroy(spider);
            }
        }

        spawnedSpiders.Clear();
    }

    private GameObject GetSpiderPrefab(SpiderVisualKind spiderVisualKind)
    {
        switch (spiderVisualKind)
        {
            case SpiderVisualKind.Cartoon:
                return cartoonSpiderPrefab;

            case SpiderVisualKind.Realistic:
                return realisticSpiderPrefab;

            default:
                return cartoonSpiderPrefab;
        }
    }

    private GameObject SpawnAtSurfaceOrFallback(GameObject prefab, Transform spawnPoint)
    {
        LayerMask ignoreSpiders = ~LayerMask.GetMask("Spider");

        Vector3[] directions =
        {
            Vector3.down,
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back
        };

        foreach (Vector3 dir in directions)
        {
            Vector3 rayOrigin = spawnPoint.position + (-dir * 2f);

            if (Physics.Raycast(rayOrigin, dir, out RaycastHit hit, 20f, ignoreSpiders))
            {
                Vector3 randomOffset = new Vector3(
                    Random.Range(-0.3f, 0.3f),
                    0f,
                    Random.Range(-0.3f, 0.3f)
                );

                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

                return Instantiate(prefab, hit.point + randomOffset, rotation);
            }
        }

        Debug.LogWarning("No raycast hit for spawn point: " + spawnPoint.name);

        return Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
    }

    private void ApplyLevelConfigToSpider(GameObject spider, LevelConfig config)
    {
        float scale = config.SpiderSizeKind == SpiderSizeKind.Small
            ? config.SpiderScale
            : config.SpiderScale * 1.5f;

        spider.transform.localScale = Vector3.one * scale;

        // SpiderMovement movement = spider.GetComponent<SpiderMovement>();

        // if (movement != null)
        // {
        //     movement.Configure(config.SpiderMovementKind, config.SpiderSpeed);
        // }
    }
}
