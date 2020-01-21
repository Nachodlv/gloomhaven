using System;
using System.Collections;
using System.Collections.Generic;
using Selection;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private BoardPainter boardPainter;

    private SquareSelection squareSelection;
    private AbilitySelection abilitySelection;
    private SelectorState currentSelector;
    private bool actionInProgress;
    private bool abilitySelected;

    private void Awake()
    {
        squareSelection = new SquareSelection();
        abilitySelection = new AbilitySelection();
        abilitySelection.OnAbilityCasted += (ability, destination) =>
        {
            actionInProgress = false;
            AbilityUnselected();
            turnManager.AbilityUsedController.AbilityUsed(turnManager, ability, destination);
        };
    }

    /**
     * This method is executed when a square from the board is selected.
     * If an ability is selected then it calls the OnSquareSelected of the AbilitySelection.
     * If an ability is not selected then it call the OnSquareSelected of the SquareSelection.
     */
    public void OnSquareSelected(Square square)
    {
        if (actionInProgress) return;
        var character = turnManager.GetCurrentCharacter();

        actionInProgress = true;
        if (abilitySelected)
        {
            actionInProgress = abilitySelection.OnSquareSelected(boardPainter, square, character);
        }
        else
        {
            squareSelection.OnSquareSelected(boardPainter, square, character, () =>
            {
                actionInProgress = false;
                if (character.Stats.Speed > 0) boardPainter.PaintWalkingRange(character);
                else EndTurn();
            });
        }
    }

    /**
      * This method is executed when a square from the board is hovered.
      * If an ability is selected then it calls the OnSquareHovered of the AbilitySelection.
      * If an ability is not selected then it call the OnSquareHovered of the SquareSelection.
      */
    public void OnSquareHovered(Square square)
    {
        if (actionInProgress) return;
        var character = turnManager.GetCurrentCharacter();

        if (abilitySelected) abilitySelection.OnSquareHovered(boardPainter, square, character);
        else squareSelection.OnSquareHovered(boardPainter, square, character);
    }

    /**
     * This method is executed when the mouse exits a square from the board.
     * If an ability is selected then it calls the OnSquareUnHovered of the AbilitySelection.
     * If an ability is not selected then it call the OnSquareUnHovered of the SquareSelection.
     */
    public void OnSquareUnHovered(Square square)
    {
        var character = turnManager.GetCurrentCharacter();

        if (abilitySelected) abilitySelection.OnSquareUnHovered(boardPainter, character);
        else squareSelection.OnSquareUnHovered(boardPainter, character);
    }


    /// <summary>
    ///  It assigns the ability to the AbilitySelection.
    ///  Calls the method StopWalking from SquareSelection
    ///  </summary>
    ///  <remarks>
    ///  This method is called when an ability is selected on the UI.
    ///  </remarks>
    ///  <param name="ability">Ability selected on the UI</param>
    /// <param name="onAbilityUsed">Method that will be called when the ability finish casting</param>
    public void OnAbilitySelected(Ability ability, Action onAbilityUsed)
    {
        if (actionInProgress)
        {
            onAbilityUsed();
            return;
        }

        abilitySelected = true;
        var character = turnManager.GetCurrentCharacter();
        abilitySelection.OnAbilitySelected(ability, boardPainter, character, onAbilityUsed);
        SquareSelection.StopWalking(boardPainter, character);
    }

    /// <summary>
    /// Calls the method UnSelectAbility from the AbilitySelection.
    /// Calls the method StartWalking from the SquareSelection.
    /// If an action is in progress such as walking or casting an ability then the invocation of this method is ignored.
    /// </summary>
    public void AbilityUnselected()
    {
        if (actionInProgress) return;
        abilitySelected = false;
        var character = turnManager.GetCurrentCharacter();
        abilitySelection.UnSelectAbility(boardPainter, character);
        SquareSelection.StartWalking(boardPainter, character);
    }

    /// <summary>
    /// Calls the method EndTurn() from TurnManager.
    /// </summary>
    /// <remarks><see cref="TurnManager.EndTurn()"/> for more information.</remarks>
    private void EndTurn()
    {
        turnManager.EndTurn();
    }
}