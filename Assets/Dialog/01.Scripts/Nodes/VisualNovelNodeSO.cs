using Dialog;
using UnityEngine;

namespace Dialog
{
    public class VisualNovelNodeSO : NodeSO
    {

        public override void OnEnable()
        {
            nodeType = DialogNodeType.VisualNovelMode;
        }
    }
}
