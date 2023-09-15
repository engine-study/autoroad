using UnityEngine;

namespace Febucci.UI.Core
{
    [UnityEngine.Scripting.Preserve]
    [EffectInfo(tag: TAnimTags.ap_Fade)]
    class FadeAppearance : AppearanceBase
    {

        public override void SetDefaultValues(AppearanceDefaultValues data)
        {
            effectDuration = data.defaults.fadeDuration;
        }

        Color32 temp;

        public override void ApplyEffect(ref CharacterData data, int charIndex)
        {
            //from transparent to real color
            for (int i = 0; i < TextUtilities.verticesPerChar; i++)
            {
                temp = data.colors[i];
                temp.a = 0;

                data.colors[i] = Color32.LerpUnclamped(data.colors[i], temp,
                    Tween.EaseInOut(1 - (data.passedTime / effectDuration)));
            }
        }
    }

}