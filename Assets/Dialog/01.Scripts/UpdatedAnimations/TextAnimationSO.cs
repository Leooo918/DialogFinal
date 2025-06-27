using System;
using UnityEngine;

namespace Dialog.Animation
{
    [CreateAssetMenu(menuName = "SO/Dialog/TextAnimation")]
    public class TextAnimationSO : ScriptableObject
    {
        public string TagID;
        public string animationClassName;

        [SerializeReference] public TextAnimation textAnimation;

        private void OnValidate()
        {
            try
            {
                if (textAnimation == null)
                {
                    Type type = Type.GetType(animationClassName);
                    TextAnimation animation = Activator.CreateInstance(type) as TextAnimation;
                    textAnimation = animation;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }
}
