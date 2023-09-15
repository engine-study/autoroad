using UnityEngine;

namespace Febucci.UI.Core
{
    [UnityEngine.Scripting.Preserve]
    [EffectInfo(tag: TAnimTags.bh_Fade)]
    class FadeBehavior : BehaviorBase
    {
        float delay = .3f;

        float[] charPCTs;

        public override void SetDefaultValues(BehaviorDefaultValues data)
        {
            delay = data.defaults.fadeDelay;
        }

        public override void Initialize(int charactersCount)
        {
            base.Initialize(charactersCount);
            charPCTs = new float[charactersCount];
        }

        public override void SetModifier(string modifierName, string modifierValue)
        {
            switch (modifierName)
            {
                //delay
                case "d": ApplyModifierTo(ref delay, modifierValue); break;
            }
        }

        Color32 temp;
        public override void ApplyEffect(ref CharacterData data, int charIndex)
        {
            if (data.passedTime <= delay) //not passed enough time yet
                return;

            charPCTs[charIndex] += time.deltaTime;
            
            if (charPCTs[charIndex] > 1) charPCTs [charIndex] = 1;
            
            //Lerps
            if (charPCTs[charIndex] < 1 && charPCTs[charIndex] >= 0)
            {
                for (var i = 0; i < TextUtilities.verticesPerChar; i++)
                {
                    temp = data.colors[i];
                    temp.a = 0;
                    
                    data.colors[i] = Color32.LerpUnclamped(data.colors[i], temp, Tween.EaseInOut(charPCTs[charIndex]));
                }
            }
            else //Keeps them hidden
            {
                for (var i = 0; i < TextUtilities.verticesPerChar; i++)
                {
                    temp = data.colors[i];
                    temp.a = 0;

                    data.colors[i] = temp;
                }
            }

        }


        public override string ToString()
        {
            return $"delay: {delay}\n" +
                $"\n{ base.ToString()}";
        }

    }
}