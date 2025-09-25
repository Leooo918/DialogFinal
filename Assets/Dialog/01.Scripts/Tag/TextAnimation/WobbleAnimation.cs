using Dialog.Tag;
using UnityEngine;

namespace Dialog.Tag
{
    public class WobbleAnimation : TextTag
    {
        public float power;
        public float speed;

        public override void ApplyEffort(CharacterData characterDatas)
        {
            for (int i = 0; i < 4; ++i)
            {
                var orig = characterDatas.Source.positions[i];
                characterDatas.current.positions[i] = orig + new Vector3(0, Mathf.Sin(characterDatas.timer * speed + orig.x * 0.01f) * power, 0);
            }
        }

        public override TextTag Instantiate()
        {
            WobbleAnimation wobble = new WobbleAnimation();

            return wobble;
        }
    }
}


