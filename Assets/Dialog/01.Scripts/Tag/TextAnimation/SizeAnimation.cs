using DG.Tweening;
using UnityEngine;

namespace Dialog.Tag
{
    public class SizeAnimation : TextTag
    {
        public float duration;
        public float amplitude;

        public override void ApplyEffort(CharacterData characterData)
        {
            if (characterData.isVisible == false) return;

            Vector3 middlePos = (characterData.source.positions[0] + characterData.source.positions[2]) / 2f;
            float progress = characterData.timer / duration;

            for (int i = 0; i < characterData.current.positions.Length; i++)
            {
                Vector3 origin = characterData.source.positions[i];

                characterData.current.positions[i] = Vector3.LerpUnclamped(origin, middlePos,
                    Mathf.Lerp(amplitude, 0, progress));
            }
        }

        public override TextTag Instantiate()
        {
            SizeAnimation animationInstance = new SizeAnimation();
            animationInstance.duration = duration;
            animationInstance.amplitude = amplitude;

            return animationInstance;
        }
    }
}
