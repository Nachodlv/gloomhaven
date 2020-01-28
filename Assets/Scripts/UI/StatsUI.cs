using TMPro;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class StatsUI : MonoBehaviour
{
    [SerializeField][Tooltip("Health bar of the character")]
    private StatBar healthBar;

    [SerializeField][Tooltip("Mana bar of the character")]
    private StatBar manaBar;

    [SerializeField][Tooltip("The Canvas Group from the canvas that contains the stats")]
    private CanvasGroup canvas;

    [SerializeField][Tooltip("Text where the speed of the character will be displayed")]
    private TextMeshProUGUI speedText;
    [SerializeField][Tooltip("Text where the defence of the character will be displayed")]
    private TextMeshProUGUI defenceText;

    [SerializeField][Tooltip("In charge of updating the status effects images")] 
    private StatusEffectsUI statusEffectsUi;

    private CharacterStats characterStats;
    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
    }

    
    private void Start()
    {
        SetInitialStats();

        characterStats.OnStatsChange += UpdateCharacterStats;
    }

    /// <summary>
    /// Set the initial stats for the health bar and the mana bar
    /// </summary>
    private void SetInitialStats()
    {
        var health = characterStats.Health;
        healthBar.MaxValue = health;

        var mana = characterStats.Mana;
        manaBar.MaxValue = mana;
        
        UpdateCharacterStats();
    }
    
    /// <summary>
    /// <para>
    /// Updates the health bar and the mana bar with their corresponding stats.
    /// Updates the status effects images.
    /// If the health is zero then it hides the canvas where the stats are being displayed.
    /// </para>.
    /// </summary>
    /// <remarks>This method is called when the event OnStatsChange from the Stats class is invoked</remarks>
    private void UpdateCharacterStats()
    {
        if (characterStats.Health <= 0) canvas.alpha = 0;
        healthBar.CurrentValue = characterStats.Health;
        manaBar.CurrentValue = characterStats.Mana;
        speedText.text = characterStats.Speed.ToString();
        defenceText.text = characterStats.Defence.ToString();
        
        statusEffectsUi.UpdateStatusEffects(characterStats.StatusEffects);
    }
    
}
