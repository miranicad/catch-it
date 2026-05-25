using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanicModeManager : MonoBehaviour
{
    [Header("Hand Tracking")]
    public OVRHand leftOVRHand;
    public OVRHand rightOVRHand;

    [Header("Settings")]
    public float activationTime = 5f;

    [Header("UI")]
    public CanvasGroup fadeOverlay;
    public Slider countdownBar;
    public GameObject countdownIndicator;

    [Header("Audio")]
    public AudioSource panicMusic;

    private float panicTimer = 0f;
    private bool panicActive = false;

    void Update()
    {
        if (panicActive) return;

        if (BothHandsAreFists())
        {
            panicTimer += Time.deltaTime;
            countdownIndicator.SetActive(true);
            countdownBar.value = panicTimer / activationTime;

            if (panicTimer >= activationTime)
                StartCoroutine(TriggerPanicMode());
        }
        else
        {
            panicTimer = 0f;
            countdownIndicator.SetActive(false);
        }
    }

    bool BothHandsAreFists()
    {
        if (leftOVRHand == null || rightOVRHand == null) return false;

        // Faust = alle Finger gecurlt → kein Finger pincht den Daumen
        bool leftFist = !leftOVRHand.GetFingerIsPinching(OVRHand.HandFinger.Index) &&
                        !leftOVRHand.GetFingerIsPinching(OVRHand.HandFinger.Middle) &&
                        leftOVRHand.IsTracked;

        bool rightFist = !rightOVRHand.GetFingerIsPinching(OVRHand.HandFinger.Index) &&
                         !rightOVRHand.GetFingerIsPinching(OVRHand.HandFinger.Middle) &&
                         rightOVRHand.IsTracked;

        return leftFist && rightFist;
    }

    IEnumerator TriggerPanicMode()
    {
        panicActive = true;

        float elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.unscaledDeltaTime * 2f;
            fadeOverlay.alpha = Mathf.Lerp(0f, 1f, elapsed);
            yield return null;
        }

        Time.timeScale = 0f;
        FindFirstObjectByType<GameManager>().ActivatePanicMode();
        countdownIndicator.SetActive(false);

        if (panicMusic != null)
            panicMusic.Play();
    }

    public void ResumeGame()
    {
        StartCoroutine(ResumeCoroutine());
    }

    IEnumerator ResumeCoroutine()
    {
        if (panicMusic != null)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime;
                panicMusic.volume = Mathf.Lerp(1f, 0f, t);
                yield return null;
            }
            panicMusic.Stop();
            panicMusic.volume = 1f;
        }

        Time.timeScale = 1f;

        float elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * 1.5f;
            fadeOverlay.alpha = Mathf.Lerp(1f, 0f, elapsed);
            yield return null;
        }

        panicActive = false;
        panicTimer = 0f;
    }
}