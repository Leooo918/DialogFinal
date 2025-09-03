using Dialog.Animation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    public class NormalNodeSO : NodeSO
    {
        public TextAnimationGroupSO animationGruop;

        [Space]
        public ActorSO reader;
        [TextArea(5, 20)]
        [SerializeField] protected string contents;

        [ReadOnly] public string parsedContents;
        [SerializeReference] public List<TextAnimation> contentTagAnimations = new();

        [HideInInspector] public NodeSO nextNode;

        public void SetNormalNodeByOption(Option option, ActorSO defaultPlayerActor)
        {
            guid = "";
            reader = defaultPlayerActor;
            contents = option.option;
            nextNode = option.nextNode;

            OnValidate();
        }

        public string GetContents() => parsedContents;

        public List<TextAnimation> GetAllAnimations()
        {
            return contentTagAnimations;
        }

        private void OnValidate()
        {
            parsedContents = contents;
            List<TextAnimation> animInstances = TagParser.ParseAnimation(ref parsedContents, animationGruop.animations);

            for (int i = 0; i < animInstances.Count; i++)
            {
                TextAnimation instance = animInstances[i];
                bool isExsist = contentTagAnimations.Exists(anim =>
                anim.startIndex == instance.startIndex && anim.endIndex == instance.endIndex
                && anim.tag == instance.tag);

                if(isExsist == false)contentTagAnimations.Add(instance);
            }

            for (int i = 0; i < contentTagAnimations.Count; i++)
            {
                TextAnimation origin = contentTagAnimations[i];
                bool isExsist = animInstances.Exists(anim =>
                anim.startIndex == origin.startIndex && anim.endIndex == origin.endIndex
                && anim.tag == origin.tag);

                if (isExsist == false) contentTagAnimations.Remove(origin);
            }
        }
    }
}

