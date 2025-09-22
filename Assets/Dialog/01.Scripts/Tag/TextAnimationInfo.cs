using System;

namespace Dialog.Tag
{
    [Serializable]
    public struct TextAnimationInfo
    {
        public TextTagSO animSO;
        public int start, end;
        public string param;
    }
}
