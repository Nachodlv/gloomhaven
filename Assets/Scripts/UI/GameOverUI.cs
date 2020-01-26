using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

[RequireComponent(typeof(CanvasGroup))]
public class GameOverUI : MonoBehaviour
{
    [SerializeField][Tooltip("Time it takes to the display to show once the method ShowGameOver is called")] 
    private int waitTime = 2;
    
    private Action showGameOverPanel;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        showGameOverPanel = ShowGameOverPanel;
        canvasGroup = GetComponent<CanvasGroup>();
        
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }

    private void ShowGameOverPanel()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
    }

    /// <summary>
    /// Shows a panel with a text showing that the game is over and a text to reset the game.
    /// </summary>
    public void ShowGameOver()
    {
        CoroutineHelper.WaitForSeconds(this, waitTime, showGameOverPanel);
    }
}
