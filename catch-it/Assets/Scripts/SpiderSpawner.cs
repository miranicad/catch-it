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
        LayerMask ignoreSpiders = ~LayerMask.GetMask("Spider");
        int successfulSpawns = 0;

        for (int i = 0; i < spiderCount; i++)
        {
            Transform spawnPoint = spawnPoints[i];
            
            Vector3[] directions = { Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

            RaycastHit hit;
            bool placed = false;

            foreach (Vector3 dir in directions)
            {
                Vector3 rayOrigin = spawnPoint.position + (-dir * 2f);
                if (Physics.Raycast(rayOrigin, dir, out hit, 20f, ignoreSpiders)) // 10f → 20f
                {
                    Vector3 randomOffset = new Vector3(Random.Range(-0.3f, 0.3f), 0, Random.Range(-0.3f, 0.3f));
                    Instantiate(spiderPrefab, hit.point + randomOffset, Quaternion.FromToRotation(Vector3.up, hit.normal));
                    placed = true;
                    successfulSpawns++;
                    break;
                }
            }

            if (!placed)
            {
                // Fallback: spawne direkt am SpawnPoint ohne Raycast
                Debug.LogWarning("Kein Treffer für: " + spawnPoint.name);
                Instantiate(spiderPrefab, spawnPoint.position, spawnPoint.rotation);
                successfulSpawns++;
            }
    }
}
}