using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image), typeof(CanvasGroup))]
public class AbilityButton : MonoBehaviour
{
    [SerializeField] [Tooltip("Logo of the ability")]
    private Image logo;

    public Image Logo => logo;

    [SerializeField] [Tooltip("Remaining turns of the ability to be use again")]
    private TextMeshProUGUI cooldownRemaining;

    public TextMeshProUGUI CooldownRemaining => cooldownRemaining;

    [SerializeField] [Tooltip("Canvas group of the cooldown remaining TextMeshProUGUI")]
    private CanvasGroup cooldownRemainingCg;

    public CanvasGroup CooldownRemainingCg => cooldownRemainingCg;

    [SerializeField] [Tooltip("Image used to blacken the button")]
    private Image buttonFade;

    public Image ButtonFade => buttonFade;

    [SerializeField] [Tooltip("Canvas group of the button fade Image")]
    private CanvasGroup buttonFadeCg;

    public CanvasGroup ButtonFadeCg => buttonFadeCg;

    [NonSerialized] public Button Button;
    [NonSerialized] public Image Image;

    [NonSerialized] private CanvasGroup canvasGroup;
    public CanvasGroup CanvasGroup => canvasGroup;

    private void Awake()
    {
        Button = GetComponent<Button>();
        Image = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
}