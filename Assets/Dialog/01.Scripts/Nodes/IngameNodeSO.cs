using Dialog.Animation;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    public class IngameNodeSO : NodeSO
    {
        public ActorSO reader;
        public TagableText text;

        [HideInInspector] public NodeSO linkedNode;


        public override void OnEnable() { nodeType = DialogNodeType.IngameMode; }
        private void OnValidate() { text?.ParseTag(animationGruop); }

        public void SetNormalNodeByOption(Option option, ActorSO defaultPlayerActor)
        {
            guid = "";
            reader = defaultPlayerActor;
            text = new TagableText(option.option);
            linkedNode = option.nextNode;

            text.ParseTag(animationGruop);
        }

        #region Data Getter

        public string GetText() => text.parsedText;
        public List<TextAnimation> GetTextAnimation() => text.tagAnimations;

        #endregion
    }
}
