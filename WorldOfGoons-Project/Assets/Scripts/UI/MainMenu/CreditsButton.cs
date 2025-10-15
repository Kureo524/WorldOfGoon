using System.Collections;
using EasyTextEffects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CreditsButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Text Settings")]
    public TextMeshProUGUI textEffect;
    public string url;
    public float inDuration;
    public float outDuration;
    
    public bool isInteractable;

    void Start() {
        if (!textEffect)
            textEffect = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    public void SetInteractable(bool interactable) {
        isInteractable = interactable;
    }

    public void OpenUrl(string _url) {
        Application.OpenURL(_url);
    }
    
    public void OnPointerEnter(PointerEventData eventData) {
        StopAllCoroutines();
        StartCoroutine(FadeEffect(1, inDuration));
    }

    public void OnPointerExit(PointerEventData eventData) {
        StopAllCoroutines();
        StartCoroutine(FadeEffect(0, outDuration));
    }

    private IEnumerator FadeEffect(float endValue, float duration) {
        float elapsedTime = 0;
        float startAlpha = textEffect.color.a;
        // float animTime = endValue < startAlpha? duration - (1 / startAlpha * duration) : duration - (startAlpha * duration);

        while (elapsedTime < duration) {
            textEffect.alpha = Mathf.Lerp(startAlpha, endValue, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
