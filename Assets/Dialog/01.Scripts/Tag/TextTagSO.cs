using System;
using UnityEngine;

namespace Dialog.Tag
{
    [CreateAssetMenu(menuName = "SO/Dialog/TextTag")]
    public class TextTagSO : ScriptableObject
    {
        public string TagID;
        public string animationClassName;

        [SerializeReference] public TextTag textAnimation;

        private void OnValidate()
        {
            try
            {
                if (textAnimation == null)
                {
                    Type type = Type.GetType(animationClassName);
                    TextTag animation = Activator.CreateInstance(type) as TextTag;
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
