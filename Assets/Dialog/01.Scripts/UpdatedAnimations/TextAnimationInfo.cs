using System;

namespace Dialog.Animation
{
    [Serializable]
    public struct TextAnimationInfo
    {
        public TextAnimationSO animSO;
        public int start, end;
        public string param;
    }
}
