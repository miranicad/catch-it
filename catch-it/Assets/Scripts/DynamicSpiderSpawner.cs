using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DynamicSpiderSpawner : MonoBehaviour
{
    [Header("Spider Prefabs")]
    public GameObject spiderPrefabCartoonStatic;
    public GameObject spiderPrefabCartoonWalking;
    public GameObject spiderPrefabCartoonIdle;
    public GameObject spiderPrefabRealistic;
    [SerializeField] private List<SpiderPrefabInfo> spiderPrefabByKind;

    [Header("Spawn Points")]
    public Transform spawnPointsInsideContainer;
    public Transform spawnPointsOutsideContainer;

    private ILookup<SpiderVisualKind, GameObject> prefabLookup => spiderPrefabByKind.ToLookup(entry => entry.VisualKind, entry => entry.Prefab);

    private readonly List<GameObject> spawnedSpiders = new();
    private Transform lastUsedSpawnPoint = null;

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
        // Get spawn points from environment specific container
        Transform containerToUse = config.EnvironmentKind == EnvironmentKind.Inside ? spawnPointsInsideContainer : spawnPointsOutsideContainer;
        Transform[] pointsToUse = containerToUse.GetComponentsInChildren<Transform>().Where(t => t != containerToUse).ToArray();

        if (pointsToUse == null || pointsToUse.Length == 0)
        {
            Debug.LogError("No spawn points assigned in config or inspector.");
            return;
        }

        GameObject prefab = GetSpiderPrefab(config);

        if (prefab == null)
        {
            Debug.LogError($"No prefab assigned for spider type: {config}");
            return;
        }

        Transform spawnPoint = pointsToUse.Except(new[] { lastUsedSpawnPoint }).ToArray()[Random.Range(0, pointsToUse.Length - (lastUsedSpawnPoint ? 1 : 0))];

        GameObject spider = SpawnAtSurfaceOrFallback(prefab, spawnPoint);

        ApplyLevelConfigToSpider(spider, config);

        spawnedSpiders.Add(spider);
        lastUsedSpawnPoint = spawnPoint;
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

    private GameObject GetSpiderPrefab(LevelConfig config)
    {
        if (prefabLookup.Contains(config.SpiderVisualKind))
        {
            return prefabLookup[config.SpiderVisualKind].FirstOrDefault();
        }

        switch (config.SpiderVisualKind)
        {
            case SpiderVisualKind.None:
                return null;
            case SpiderVisualKind.Cartoon:
                switch (config.SpiderMovementKind)
                {
                    case SpiderMovementKind.Static:
                        return spiderPrefabCartoonStatic;
                    case SpiderMovementKind.Idle:
                        return spiderPrefabCartoonIdle;
                    case SpiderMovementKind.Walking:
                        return spiderPrefabCartoonWalking;
                    default:
                        return spiderPrefabCartoonStatic;
                }

            case SpiderVisualKind.Scary:
                return spiderPrefabRealistic;

            case SpiderVisualKind.Realistic:
                return spiderPrefabRealistic;

            default:
                return spiderPrefabCartoonStatic;
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

                Quaternion surfaceRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                Quaternion randomTwist = Quaternion.AngleAxis(Random.Range(0f, 360f), hit.normal);
                Quaternion rotation = randomTwist * surfaceRotation;

                return Instantiate(prefab, hit.point + randomOffset, rotation);
            }
        }

        Debug.LogWarning("No raycast hit for spawn point: " + spawnPoint.name);

        Quaternion fallbackRotation = Quaternion.Euler(
            spawnPoint.eulerAngles.x,
            Random.Range(0f, 360f),
            spawnPoint.eulerAngles.z
        );

        return Instantiate(prefab, spawnPoint.position, fallbackRotation);
    }

    private void ApplyLevelConfigToSpider(GameObject spider, LevelConfig config)
    {
        // we assume the prefab has a local scale that makes it appear as a "small" spider in the game world. A large spider will be made larger here.
        if (config.SpiderSizeKind == SpiderSizeKind.Large)
        {
            spider.transform.localScale *= 1.5f;
        }

        if (config.SpiderMovementKind != SpiderMovementKind.Static)
        {
            // todo: add movement
        }
    }
}
