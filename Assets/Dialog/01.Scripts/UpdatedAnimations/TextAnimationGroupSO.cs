using System.Collections.Generic;
using UnityEngine;

namespace Dialog.Animation
{
    [CreateAssetMenu(menuName = "SO/Dialog/AnimationGroup")]
    public class TextAnimationGroupSO : ScriptableObject
    {
        public List<TextAnimationSO> animations;
    }
}
