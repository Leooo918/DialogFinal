using UnityEditor.U2D.Animation;
using UnityEngine;

namespace Dialog.Tag
{
    public class ShakeAnimation : TextTag
    {
        public float power;

        public override void ApplyEffort(CharacterData characterData, TMP_AnimationPlayer player)
        {
            if (characterData.isVisible == false) return;

            for (int i = 0; i < characterData.current.positions.Length; i++)
            {
                Vector3 origin = characterData.source.positions[i];

                float x = Mathf.Sin((Time.time + i) * 62.8f) * power;
                float y = Mathf.Cos((Time.time + i) * 40f) * power;
                characterData.current.positions[i] = origin + new Vector3(x, y, 0);
            }
        }

        public override TextTag Instantiate()
        {
            ShakeAnimation animation = new ShakeAnimation();
            animation.power = power;
            return animation;
        }
    }
}
