using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CurrentCharacterUI : MonoBehaviour
{
    [SerializeField] [Tooltip("Ability buttons where will be displayed the abilities of the current character")]
    private AbilityButton[] abilityButtons;

    [SerializeField] private SelectionManager selectionManager;

    [SerializeField] [Tooltip("Color in which the ability logos will be colored when selected")]
    private Color selectColor;

    [SerializeField] private Button endTurnButton;

    [NonSerialized] private AbilityUIBuilder abilityUiBuilder;

    private void Start()
    {
        abilityUiBuilder = new AbilityUIBuilder(abilityButtons, selectColor);
        endTurnButton.onClick.AddListener(OnTurnEndClicked);
    }

    /// <summary>
    /// Calls the AbilityUIBuilder to build the ability buttons
    /// </summary>
    /// <param name="character">The abilities from the character will be used to build the ability buttons</param>
    public void SetCurrentCharacter(Character character)
    {
        abilityUiBuilder.BuildAbilityButtons(character.Abilities, selectionManager);
    }

    private void OnTurnEndClicked()
    {
        selectionManager.EndTurn();
    }
}