using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UI
{
    public class AbilityUIBuilder
    {
        private readonly AbilityButton[] buttons;
        private AbilityButton selectedButton;
        private bool isAButtonSelected;
        private Color selectedColor;
        private readonly Color unSelectedColor;

        /// <summary>
        /// </summary>
        /// <param name="buttons">The buttons that will be used to set the abilities of the character</param>
        /// <param name="selectedColor">The color that will be painted the buttons when is selected</param>
        public AbilityUIBuilder(AbilityButton[] buttons, Color selectedColor)
        {
            this.buttons = buttons;
            this.selectedColor = selectedColor;
            if (buttons.Length > 0)
            {
                unSelectedColor = buttons[0].Image.color;
            }
        }

        /// <summary>
        /// Adds the logo and the listeners to the ability buttons.
        /// When a button is selected it calls SelectionManager.OnAbilitySelected.
        /// When a button is unselected it call SelectionManager.OnAbilityUnSelected.
        /// </summary>
        /// <param name="abilities">The abilities that will be used to modify the ability buttons</param>
        /// <param name="selectionManager">SelectedManager called when an ability button is selected or unselected
        /// </param>
        public void BuildAbilityButtons(Ability[] abilities, SelectionManager selectionManager)
        {
            isAButtonSelected = false;

            for (var i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];

                if (i < abilities.Length)
                {
                    var ability = abilities[i];
                    BuildAbilityButton(button, ability, selectionManager);
                }
                else
                {
                    button.CanvasGroup.alpha = 0;
                }
                
            }
        }

        private void BuildAbilityButton(AbilityButton button, Ability ability, SelectionManager selectionManager)
        {
            button.CanvasGroup.alpha = 1;
            button.Logo.sprite = ability.AbilityUi.icon;

            var isInCooldown = ability.CurrentCooldown != 0;
            button.ButtonFadeCg.alpha = isInCooldown ? 1 : 0;
            button.CooldownRemainingCg.alpha = isInCooldown ? 1 : 0;
            button.Button.enabled = !isInCooldown;
            button.Image.color = unSelectedColor;

            if (isInCooldown) button.CooldownRemaining.text = ability.CurrentCooldown.ToString();
            else OnClickOperation(button, ability, selectionManager);
        }

        private void OnClickOperation(AbilityButton abilityButton, Ability ability, SelectionManager selectionManager)
        {
            abilityButton.Button.onClick.RemoveAllListeners();
            abilityButton.Button.onClick.AddListener(() =>
            {
                if (isAButtonSelected)
                {
                    selectedButton.Image.color = unSelectedColor;
                    selectionManager.AbilityUnselected();
                }

                if (isAButtonSelected && selectedButton == abilityButton)
                {
                    isAButtonSelected = false;
                }
                else
                {
                    isAButtonSelected = true;
                    selectedButton = abilityButton;
                    selectedButton.Image.color = selectedColor;
                    selectionManager.OnAbilitySelected(ability, () => DeselectAbility(ability, selectionManager));
                }
            });
        }

        private void DeselectAbility(Ability ability, SelectionManager selectionManager)
        {
            isAButtonSelected = false;
            BuildAbilityButton(selectedButton, ability, selectionManager);
        }
    }
}