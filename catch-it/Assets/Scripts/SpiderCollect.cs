using UnityEngine;

public class SpiderCollect : MonoBehaviour
{
    public static int score = 0;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit by: " + other.name);
        score++;
        Destroy(gameObject);
    }
}