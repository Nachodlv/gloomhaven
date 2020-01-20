using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    [SerializeField][Tooltip("Color of the stat bar when is full")]
    private Color statBarColor;
    [SerializeField][Tooltip("Current value text")]
    private TextMeshProUGUI currentValueText;

    [SerializeField] [Tooltip("Max value text")]
    private TextMeshProUGUI maxValueText;

    [SerializeField] [Tooltip("Image of the scrollbar")]
    private Image statBarImage;

    [SerializeField][Tooltip("Max value of the stat bar")]
    private int maxValue;
    [SerializeField][Tooltip("Current value of the stat bar")]
    private int currentValue;

    /// <summary>
    /// When the CurrentValue is set then it updates the fillAmount of the statBarImage and the text of the currentValue.
    /// </summary>
    public int CurrentValue
    {
        get => currentValue;
        set
        {
            currentValue = value;
            currentValueText.text = value.ToString();

            var barValue =  currentValue / (float) maxValue;
            statBarImage.fillAmount = barValue;
        }
    }

    /// <summary>
    /// When the MaxValue is set then it updates the text of the maxValueText.
    /// </summary>
    public int MaxValue
    {
        get => maxValue;
        set
        {
            maxValue = value;
            maxValueText.text = value.ToString();
        }
    }
    

   
}
