using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class StatusEffectImage : MonoBehaviour
{
    [SerializeField][Tooltip("Text that represents how many turns will the status effect remain active")] 
    private TextMeshProUGUI turnsRemaining;
    [SerializeField][Tooltip("Used to hide the StatusEffectImage")] 
    private CanvasGroup canvasGroup;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    /// <summary>
    /// <para>
    /// Sets the color of the background image of the rectangle.
    /// </para>
    /// </summary>
    /// <param name="newColor">The new color that wil replace the old one.</param>
    public void SetColor(Color newColor)
    {
        image.color = newColor;
    }

    /// <summary>
    /// <para>
    /// Sets the text of turn remaining to the number passed as parameter.
    /// </para>
    /// </summary>
    /// <param name="turns"></param>
    public void SetTurnsRemaining(int turns)
    {
        turnsRemaining.text = turns.ToString();
    }
    
    /// <summary>
    /// <para>Hides the StatusEffectImage.</para>
    /// </summary>
    public void Hide()
    {
        ShowOrHide(false);
    }

    /// <summary>
    /// <para>Shows the StatusEffectImage.</para>
    /// </summary>
    public void Show()
    {
        ShowOrHide(true);
    }
    
    private void ShowOrHide(bool show)
    {
        canvasGroup.alpha = show? 1 : 0;
        canvasGroup.interactable = show;
        canvasGroup.blocksRaycasts = show;
    }
}
