using Dialog.Animation;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    [Serializable]
    public class TagableText
    {
        [TextArea(5, 20)]
        [SerializeField] protected string text;

        [ReadOnly] public string parsedText;
        [SerializeReference] public List<TextAnimation> tagAnimations = new();

        public void ParseTag(TextAnimationGroupSO animationGroup)
        {
            parsedText = text;
            List<TextAnimation> animInstances = TagParser.ParseAnimation(ref parsedText, animationGroup.animations);

            for (int i = animInstances.Count - 1; i >= 0; i--)
            {
                var animation = tagAnimations.Find(anim => anim.tag == animInstances[i].tag);

                if (animation != null)
                {
                    int start = animInstances[i].startIndex;
                    int end = animInstances[i].endIndex;

                    animInstances[i] = animation;
                    animInstances[i].startIndex = start;
                    animInstances[i].endIndex = end;
                    tagAnimations.Remove(animation);
                }
            }

            tagAnimations = animInstances;
        }

        public TagableText(string text)
        {
            this.text = text;
        }
    }
}
