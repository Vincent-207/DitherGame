using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]

public class AmountBar : MonoBehaviour
{
    [SerializeField] RectTransform colorBar;
    RectTransform backgroundBar;
    [SerializeField]
    float proportion;
    CanvasGroup canvasGroup;
    
    void Start()
    {
        backgroundBar = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    void Update()
    {
        UpdateBar();
    }

    void UpdateBar()
    {
        float fullWidth = backgroundBar.sizeDelta.x * 2;
        Vector2 dimensions = new Vector2(fullWidth * proportion, colorBar.sizeDelta.y);
        colorBar.sizeDelta = dimensions;
    }

    public void SetProportion(float input)
    {
        proportion = input;
        UpdateBar();
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
    }

    public void Show()
    {
        canvasGroup.alpha = 1;
    }
}
