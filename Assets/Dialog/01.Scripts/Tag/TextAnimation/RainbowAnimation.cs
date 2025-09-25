using System;
using UnityEngine;

namespace Dialog.Tag
{
    public class RainbowAnimation : TextTag
    {
        public Gradient gradient;

        public override void ApplyEffort(CharacterData characterDatas)
        {
            for (int i = 0; i < characterDatas.current.colors.Length; i++)
            {
                characterDatas.current.colors[i] =
                    gradient.Evaluate(Mathf.Repeat(characterDatas.timer + (characterDatas.Source.positions[i].x * 0.001f), 1f));
            }
        }

        public override TextTag Instantiate()
        {
            RainbowAnimation rainbow = new RainbowAnimation();
            rainbow.gradient = gradient;
            return rainbow;
        }
    }
}
