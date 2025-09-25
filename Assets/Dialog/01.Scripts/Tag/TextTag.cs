using System;
using UnityEngine;

namespace Dialog.Tag
{
    [Serializable]
    public abstract class TextTag
    {
        public Guid textGuid;
        [ReadOnly] public int startIndex;
        [ReadOnly] public int endIndex;
        public int AnimationLength => (endIndex - startIndex);

        [HideInInspector] public bool calcurateEndIndex = true;
        [HideInInspector] public string tag;

        public abstract void ApplyEffort(CharacterData characterDatas);
        public abstract TextTag Instantiate();
    }
}
