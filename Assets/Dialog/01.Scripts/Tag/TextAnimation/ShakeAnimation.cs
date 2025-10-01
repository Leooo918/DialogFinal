using UnityEngine;

namespace Dialog.Tag
{
    public class ShakeAnimation : TextTag
    {
        public float power;
        private const float ShakeFrequencyX = Mathf.PI * 20f;
        private const float ShakeFrequencyY = 40f;

        public override void ApplyEffort(CharacterData characterData)
        {
            if (characterData.isVisible == false) return;

            float offset = characterData.Source.positions[0].x;

            float x = Mathf.Sin((characterData.timer + offset) * ShakeFrequencyX) * power;
            float y = Mathf.Cos((characterData.timer + offset) * ShakeFrequencyY) * power;
            for (int i = 0; i < characterData.current.positions.Length; i++)
            {
                Vector3 origin = characterData.Source.positions[i];
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
