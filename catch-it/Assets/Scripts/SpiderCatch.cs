using UnityEngine;

public class SpiderCatch : MonoBehaviour
{
    private bool wasCollected = false;
    private LevelProgressionManager levelManager;

    private void Start()
    {
        levelManager = FindFirstObjectByType<LevelProgressionManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (wasCollected)
        {
            return;
        }

        Debug.Log("Hit by: " + other.name);

        if (IsValidHandCollider(other))
        {
            Catch();
        }
    }

    private bool IsValidHandCollider(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            return true;
        }

        if (other.transform.root.CompareTag("Hand"))
        {
            return true;
        }

        return other.name == "Collider"
            || other.name == "PinchArea"
            || other.name == "PinchPointRange";
    }

    private void Catch()
    {
        wasCollected = true;

        Debug.Log("Spider caught: " + gameObject.name);

        if (levelManager != null)
        {
            levelManager.RegisterSpiderCaught(gameObject);
        }
        else
        {
            Debug.LogWarning("No LevelManager found. Destroying spider directly.");
            Destroy(gameObject);
        }
    }
}
