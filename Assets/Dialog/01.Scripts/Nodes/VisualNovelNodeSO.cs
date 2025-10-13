using Dialog;
using UnityEngine;

namespace Dialog
{
    public class VisualNovelNodeSO : NodeSO
    {
        public TagableText readerName;
        public TagableText contentName;
        [SerializeReference] public TextOutputMethodSO textOutputMethod;

        [ReadOnly] public NodeSO linkedNode;

        public override void OnEnable()
        {
            nodeType = DialogNodeType.VisualNovelMode;
        }

        private void OnValidate()
        {
            readerName?.ParseTag(animationGroup);
            contentName?.ParseTag(animationGroup);
        }
    }
}
