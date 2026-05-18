using UnityEngine;

public class SpiderScript : MonoBehaviour
{
    private bool wasCollected = false;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
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

        if (gameManager != null)
        {
            gameManager.RegisterSpiderCaught(gameObject);
        }
        else
        {
            Debug.LogWarning("No GameManager found. Destroying spider directly.");
            Destroy(gameObject);
        }
    }
}
