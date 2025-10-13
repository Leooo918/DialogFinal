using Dialog.Events.Cinemachine;
using UnityEngine;

namespace Dialog.Tag
{
    public abstract class EventTextTag : TextTag
    {
        protected bool _isPlayed = false;

        public override void ApplyEffort(CharacterData characterDatas)
        {
            if (_isPlayed == false)
            {
                PlayEvent();
                _isPlayed = true;
            }
        }

        public virtual void Initialize() => _isPlayed = false;

        protected abstract void PlayEvent();
    }
}
