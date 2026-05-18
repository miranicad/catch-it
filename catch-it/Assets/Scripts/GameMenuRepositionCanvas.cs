using UnityEngine;

public class GameMenuRepositionCanvas : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerHead;

    [Header("Placement")]
    [SerializeField] private float distanceFromPlayer = 0.8f;
    [SerializeField] private float menuHeight = 1.35f;

    [Header("Collision Avoidance")]
    [SerializeField] private LayerMask blockingLayers;
    [SerializeField] private float menuCollisionRadius = 0.35f;
    [SerializeField] private float fallbackDistanceStep = 0.15f;
    [SerializeField] private int fallbackAttempts = 4;

    private Vector3 lastGoodForward = Vector3.forward;

    private void Update()
    {
        UpdateLastGoodForward();
    }

    private void UpdateLastGoodForward()
    {
        if (playerHead == null)
        {
            return;
        }

        Vector3 forward = playerHead.forward;
        forward.y = 0f;

        // Only update if the direction is stable enough.
        // This avoids using weird directions when the user looks mostly up/down.
        if (forward.sqrMagnitude > 0.05f)
        {
            lastGoodForward = forward.normalized;
        }
    }

    public void PlaceInFrontOfPlayer()
    {
        if (playerHead == null)
        {
            Debug.LogWarning("Player head is not assigned.");
            return;
        }

        Vector3 origin = playerHead.position;
        origin.y = menuHeight;

        Vector3 safePosition = FindSafePosition(origin, lastGoodForward);

        transform.position = safePosition;
        FacePlayer();
    }

    private Vector3 FindSafePosition(Vector3 origin, Vector3 forward)
    {
        Vector3[] directionsToTry =
        {
            forward,
            Quaternion.Euler(0f, 25f, 0f) * forward,
            Quaternion.Euler(0f, -25f, 0f) * forward,
            Quaternion.Euler(0f, 45f, 0f) * forward,
            Quaternion.Euler(0f, -45f, 0f) * forward
        };

        foreach (Vector3 direction in directionsToTry)
        {
            Vector3 normalizedDirection = direction.normalized;

            for (int i = 0; i <= fallbackAttempts; i++)
            {
                float candidateDistance = distanceFromPlayer - i * fallbackDistanceStep;
                candidateDistance = Mathf.Max(candidateDistance, 0.6f);

                Vector3 candidate = origin + normalizedDirection * candidateDistance;
                candidate.y = menuHeight;

                bool blocked = Physics.CheckSphere(
                    candidate,
                    menuCollisionRadius,
                    blockingLayers,
                    QueryTriggerInteraction.Ignore
                );

                if (!blocked)
                {
                    return candidate;
                }
            }
        }

        // Last-resort fallback: still put it in front, but close.
        Vector3 fallback = origin + forward.normalized * 0.65f;
        fallback.y = menuHeight;
        return fallback;
    }

    private void FacePlayer()
    {
        Vector3 directionToPlayer = playerHead.position - transform.position;
        directionToPlayer.y = 0f;

        if (directionToPlayer.sqrMagnitude < 0.001f)
        {
            return;
        }

        directionToPlayer.Normalize();

        // If the canvas faces away from the player, remove the minus sign.
        transform.rotation = Quaternion.LookRotation(-directionToPlayer, Vector3.up);
    }
}
