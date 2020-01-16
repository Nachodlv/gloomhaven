using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private BoardPainter boardPainter;

    private SquareSelection squareSelection;
    private AbilitySelection abilitySelection;
    private bool moving;
    private bool abilitySelected = true;

    private void Awake()
    {
        squareSelection = new SquareSelection();
        abilitySelection = new AbilitySelection();
    }

    /**
     * This method is executed when a square from the board is selected.
     * If an ability is selected then it calls the OnSquareSelected of the AbilitySelection.
     * If an ability is not selected then it call the OnSquareSelected of the SquareSelection.
     */
    public void OnSquareSelected(Square square)
    {
        if (moving) return;
        var character = turnManager.GetCurrentCharacter();

        if (abilitySelected)
        {
            abilitySelection.OnSquareSelected(boardPainter, square, character, OnAbilityUsed);
        }
        else
            squareSelection.OnSquareSelected(boardPainter, square, character, () =>
            {
                moving = false;
                if (character.Stats.Speed > 0) boardPainter.PaintWalkingRange(character);
                else EndTurn();
            });
    }

    /**
      * This method is executed when a square from the board is hovered.
      * If an ability is selected then it calls the OnSquareHovered of the AbilitySelection.
      * If an ability is not selected then it call the OnSquareHovered of the SquareSelection.
      */
    public void OnSquareHovered(Square square)
    {
        if (moving) return;
        var character = turnManager.GetCurrentCharacter();

        if (abilitySelected) abilitySelection.OnSquareHovered(boardPainter, square);
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

        if (abilitySelected) abilitySelection.OnSquareUnHovered(boardPainter, square);
        else squareSelection.OnSquareUnHovered(boardPainter, character);
    }


    ///<summary>
    /// It assigns the ability to the <see cref="abilitySelection">abilitySelection</see>
    /// </summary>
    /// <remarks>
    /// This method is called when an ability is selected on the UI.
    /// </remarks>
    /// <param name="ability">Ability selected on the UI</param>
    public void OnAbilitySelected(Ability ability)
    {
        abilitySelection.OnAbilitySelected(ability);
        abilitySelected = true;
        boardPainter.PaintAbilityRange(turnManager.GetCurrentCharacter(), ability.range);
    }

    /// <summary>
    /// Un paint the ability range that was selected and paint the walking range of the current character
    /// </summary>
    /// <remarks>This method is executed when an ability was successfully used</remarks>
    private void OnAbilityUsed()
    {
        abilitySelected = false;
        boardPainter.UnPaintAbilityRange(turnManager.GetCurrentCharacter(), abilitySelection.Ability.range);
        boardPainter.PaintWalkingRange(turnManager.GetCurrentCharacter());
    }

    public void StartTurn() //TODO remove
    {
        var character = turnManager.GetCurrentCharacter();
        OnAbilitySelected(character.Abilities[0]);
    }

    /// <summary>
    /// Calls the method EndTurn() from TurnManager.
    /// </summary>
    /// <remarks><see cref="TurnManager.EndTurn()"/> for more information.</remarks>
    private void EndTurn()
    {
        abilitySelected = true;
        turnManager.EndTurn();
    }
}