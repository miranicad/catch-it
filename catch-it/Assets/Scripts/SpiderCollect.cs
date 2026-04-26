using UnityEngine;

public class SpiderCollector : MonoBehaviour
{
    public int spidersCollected = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spider"))
        {
            spidersCollected++;
            Debug.Log("Spider collected! Total: " + spidersCollected);
            Destroy(other.gameObject);
        }
    }
}                                