using System.Collections.Generic;
using Abilities;
using UnityEngine;

public class StatusEffectsUI : MonoBehaviour
{
    [SerializeField] private StatusEffectImage[] statusEffectImages;

    /// <summary>
    /// <para>Sets the color and the turns remaining to the statusEffectImages.
    /// If statusEffects is larger that the quantity of statusEffectImages the overflow elements of the list will be
    /// ignored.
    /// If the statusEffectImages is larger than the statusEffect list the overflow elements will be hidden</para>
    /// </summary>
    /// <param name="statusEffects"></param>
    public void UpdateStatusEffects(List<StatusEffect> statusEffects)
    {
        var i = 0;
        var j = 0;
        while (i < statusEffectImages.Length)
        {
            var statusEffectImage = statusEffectImages[i];

            if (j >= statusEffects.Count)
            {
                statusEffectImage.Hide();
                i++;
            }
            else if (statusEffects[j].Color != Color.clear)
            {
                var newStatusEffect = statusEffects[j];
                statusEffectImage.Show();
                statusEffectImage.SetColor(newStatusEffect.Color);
                statusEffectImage.SetTurnsRemaining(newStatusEffect.DurationLeft);
                j++;
                i++;
            }
            else
            {
                j++;
            }

        }
    }

}