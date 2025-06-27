using DG.Tweening;
using UnityEngine;

namespace Dialog.Animation
{
    [CreateAssetMenu(menuName = "SO/TextAnim/SizeAnimation")]
    public class SizeAnimation : TextAnimation
    {
        public float duration = 1f;
        public float amplitude;

        public override void ApplyEffort(CharacterData characterData, TMP_AnimationPlayer player)
        {
            for (int i = 0; i < characterData.current.positions.Length; i++)
            {
                if (characterData.isVisible == false) continue;
                Vector3 middlePos = (characterData.source.positions[0] + characterData.source.positions[2]) / 2f;
                Vector3 origin = characterData.source.positions[i];

                characterData.current.positions[i] = Vector3.LerpUnclamped(origin, middlePos, 
                    Mathf.Lerp(amplitude, 0, characterData.timer / duration));
            }
        }

        public override TextAnimation Instantiate()
        {
            SizeAnimation animationInstance = new SizeAnimation();
            animationInstance.duration = duration;
            animationInstance.amplitude = amplitude;

            return animationInstance;
        }
    }
}
