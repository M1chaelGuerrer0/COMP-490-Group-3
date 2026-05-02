using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Manages the explainer panel and displays text with a typing animation.
/// </summary>
public class ExplainerManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject panel;
    public TMP_Text explanationText;
    public ScrollRect scrollRect;

    [Header("Text Data")]
    [TextArea(5, 20)]
    public string[] explanations;

    [Header("Typing Settings")]
    public float typingSpeed = 0.08f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip blipSound;

    private Coroutine typingCoroutine;

    private bool isTyping = false;
    private bool skipRequested = false;

    // Shows the explanation panel and starts typing the text at the given index
    public void ShowExplanation(int index)
    {
        panel.SetActive(true);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(explanations[index]));
    }

    // Coroutine that types out the text character by character with sound effects
    IEnumerator TypeText(string text)
    {
        explanationText.text = "";

        isTyping = true;
        skipRequested = false;

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 1f;

        foreach (char c in text)
        {
            // If skip pressed → show full text instantly
            if (skipRequested)
            {
                explanationText.text = text;
                break;
            }

            explanationText.text += c;

            // Sound
            if (!char.IsWhiteSpace(c))
            {
                audioSource.pitch = Random.Range(0.95f, 1.05f);
                audioSource.PlayOneShot(blipSound, 1.2f);
            }

            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;

            float delay = typingSpeed;

            if (c == '.' || c == '!' || c == '?')
                delay *= 5f;
            else if (c == ',')
                delay *= 3f;
            else if (c == '\n')
                delay *= 4f;

            yield return new WaitForSeconds(delay);
        }

        isTyping = false;
    }

    // Hides the explanation panel and stops any ongoing typing
    public void ClosePanel()
    {
        panel.SetActive(false);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
    }

    // Skips the typing animation and shows the full text immediately
    public void SkipTyping()
    {
        if (isTyping)
        {
            skipRequested = true;
        }
    }
}