using UnityEngine;

namespace Dialog.Tag
{
    public class ShakeAnimation : TextTag
    {
        public float power;

        public override void ApplyEffort(CharacterData characterData)
        {
            if (characterData.isVisible == false) return;

            float x = Mathf.Sin((characterData.timer) * 62.8f) * power;
            float y = Mathf.Cos((characterData.timer) * 40f) * power;
            for (int i = 0; i < characterData.current.positions.Length; i++)
            {
                Vector3 origin = characterData.source.positions[i];
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
