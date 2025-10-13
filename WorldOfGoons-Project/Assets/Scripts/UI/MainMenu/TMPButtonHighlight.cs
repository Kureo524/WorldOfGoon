using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TMPButtonHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Text Settings")]
    public TMP_Text buttonText;
    public Color normalColor = Color.white;
    public Color hoverColor = new Color(1f, 0.85f, 0.4f); // doré clair ✨
    public float fadeSpeed = 8f;

    [Header("Scale Effect")]
    public bool scaleOnHover = true;
    public float hoverScale = 1.1f;
    private Vector3 baseScale;

    [Header("Glow Effect")]
    public bool glowOnHover = true;
    [Range(0f, 1f)] public float hoverGlow = 0.25f;
    private float currentGlow = 0f;

    private Color targetColor;
    private Material textMaterial;
    
    public bool isInteractable = false;

    void Start()
    {
        if (!buttonText)
            buttonText = GetComponentInChildren<TMP_Text>();

        baseScale = buttonText.transform.localScale;
        targetColor = normalColor;

        // On clone le material pour ne pas affecter les autres textes
        textMaterial = Instantiate(buttonText.fontMaterial);
        buttonText.fontMaterial = textMaterial;

        // Assure qu'on démarre sans glow
        textMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0f);
        textMaterial.SetColor(ShaderUtilities.ID_OutlineColor, hoverColor);
    }

    public void SetInteractable(bool interactable) {
        isInteractable = interactable;
    }

    void Update()
    {
        if (!isInteractable) return;
        // 🎨 Transition douce de couleur
        buttonText.color = Color.Lerp(buttonText.color, targetColor, Time.deltaTime * fadeSpeed);

        // 📏 Scale smooth
        if (scaleOnHover)
        {
            Vector3 targetScale = (targetColor == hoverColor) ? baseScale * hoverScale : baseScale;
            buttonText.transform.localScale = Vector3.Lerp(buttonText.transform.localScale, targetScale, Time.deltaTime * fadeSpeed);
        }

        // 💡 Glow smooth
        if (glowOnHover)
        {
            float targetGlow = (targetColor == hoverColor) ? hoverGlow : 0f;
            currentGlow = Mathf.Lerp(currentGlow, targetGlow, Time.deltaTime * fadeSpeed);
            textMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, currentGlow);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetColor = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetColor = normalColor;
    }
}