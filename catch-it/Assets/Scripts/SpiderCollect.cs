using UnityEngine;

public class SpiderCollect : MonoBehaviour
{
    public static int score = 0;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit by: " + other.name);

        if (other.name == "Collider" || 
            other.name == "PinchArea" || 
            other.name == "PinchPointRange")
        {
            score++;
            Debug.Log("Spinne gesammelt! Score: " + score);
            Destroy(gameObject);
        }
    }
}