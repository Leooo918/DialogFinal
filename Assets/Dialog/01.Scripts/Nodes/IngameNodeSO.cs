using Dialog.Tag;
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
            text = new TagableText(option.optionTxt.GetFullText());
            linkedNode = option.nextNode;

            text.ParseTag(animationGruop);
        }

        #region Data Getter

        public string GetText() => text.parsedText;
        public List<TextTag> GetTextAnimation() => text.tagAnimations;

        #endregion
    }
}
