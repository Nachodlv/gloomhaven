using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class CurrentCharacterUI : MonoBehaviour
{
    [SerializeField] [Tooltip("Ability buttons where will be displayed the abilities of the current character")]
    private AbilityButton[] abilityButtons;

    [SerializeField] private SelectionManager selectionManager;

    [SerializeField] [Tooltip("Color in which the ability logos will be colored when selected")]
    private Color selectColor;

    [NonSerialized] private AbilityUIBuilder abilityUiBuilder;

    private void Start()
    {
        abilityUiBuilder = new AbilityUIBuilder(abilityButtons, selectColor);
    }

    /// <summary>
    /// Calls the AbilityUIBuilder to build the ability buttons
    /// </summary>
    /// <param name="character">The abilities from the character will be used to build the ability buttons</param>
    public void SetCurrentCharacter(Character character)
    {
        abilityUiBuilder.BuildAbilityButtons(character.Abilities, selectionManager);
    }
}