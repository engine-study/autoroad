using UnityEngine;

namespace Febucci.UI.Core
{
    [UnityEngine.Scripting.Preserve]
    [EffectInfo(tag: TAnimTags.bh_Rainb)]
    class RainbowBehavior : BehaviorBase
    {
        float hueShiftSpeed = 0.8f;
        float hueShiftWaveSize = 0.08f;

        public override void SetDefaultValues(BehaviorDefaultValues data)
        {
            hueShiftSpeed = data.defaults.hueShiftSpeed;
            hueShiftWaveSize = data.defaults.hueShiftWaveSize;
        }

        public override void SetModifier(string modifierName, string modifierValue)
        {
            switch (modifierName)
            {
                //frequency
                case "f": ApplyModifierTo(ref hueShiftSpeed, modifierValue); break;
                //wave size
                case "s": ApplyModifierTo(ref hueShiftWaveSize, modifierValue); break;
            }
        }

        Color32 temp;
        public override void ApplyEffect(ref CharacterData data, int charIndex)
        {
            for (byte i = 0; i < TextUtilities.verticesPerChar; i++)
            {
                //shifts hue
                temp = Color.HSVToRGB(Mathf.PingPong(time.timeSinceStart * hueShiftSpeed + charIndex * hueShiftWaveSize, 1), 1, 1);
                temp.a = data.colors[i].a; //preserves original alpha
                data.colors[i] = temp;
            }
        }


        public override string ToString()
        {
            return $"hueShiftSpeed: {hueShiftSpeed}\n" +
                $"hueShiftWaveSize: {hueShiftWaveSize}" +
                $"\n{base.ToString()}";
        }

    }
}