using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public class AbilityButton : MonoBehaviour
{
    [Tooltip("Logo of the ability")]
    public Image logo;
    [Tooltip("Remaining turns of the ability to be use again")]
    public Text cooldownRemaining;
    [Tooltip("Image used to blacken the button")]
    public Image buttonFade;

    [NonSerialized]
    public Button Button;
    [NonSerialized] public Image Image;

    private void Awake()
    {
        Button = GetComponent<Button>();
        Image = GetComponent<Image>();
    }
}
