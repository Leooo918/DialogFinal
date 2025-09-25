using Dialog.Tag;
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
        [SerializeReference] public List<TextTag> tagAnimations = new();

        public void ParseTag(TextTagGroupSO animationGroup)
        {
            parsedText = text;
            List<TextTag> animInstances = TagParser.ParseAnimation(ref parsedText, animationGroup.tagList);

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
        public string GetFullText() => text;
        public string GetParsedText() => parsedText;

        public TagableText(string text)
        {
            this.text = text;
        }
    }
}
