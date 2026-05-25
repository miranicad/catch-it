using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanicModeManager : MonoBehaviour
{
    [Header("Hand Transforms")]
    public Transform leftHand;
    public Transform rightHand;

    [Header("Settings")]
    public float activationTime = 5f;
    public float handDistanceThreshold = 0.3f;

    [Header("UI")]
    public CanvasGroup fadeOverlay;       
    public GameObject panicUI;            
    public Slider countdownBar;           
    public GameObject countdownIndicator; 

    [Header("Audio")]
    public AudioSource panicMusic;
    public AudioSource gameAudio;         

    private float panicTimer = 0f;
    private bool panicActive = false;

    void Update()
    {
        if (panicActive) return;

        if (BothHandsNearFace())
        {
            panicTimer += Time.deltaTime;

            // Countdown-Balken anzeigen und füllen
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

    bool BothHandsNearFace()
    {
        Vector3 headPos = Camera.main.transform.position;
        bool leftClose = Vector3.Distance(leftHand.position, headPos) < handDistanceThreshold;
        bool rightClose = Vector3.Distance(rightHand.position, headPos) < handDistanceThreshold;
        return leftClose && rightClose;
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
        panicUI.SetActive(false);

        // Musik ausblenden
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


        if (gameAudio != null)
        {
            gameAudio.Play();
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime;
                gameAudio.volume = Mathf.Lerp(0f, 1f, t);
                yield return null;
            }
        }

        panicActive = false;
        panicTimer = 0f;
    }
}