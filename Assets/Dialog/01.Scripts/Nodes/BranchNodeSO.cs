using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    public class BranchNodeSO : NodeSO
    {
        public ConditionSO condition;
        [HideInInspector] public List<NodeSO> nextNodes = new List<NodeSO>(2);

        public Action OnChangeCondition;


        public override void OnEnable() { nodeType = DialogNodeType.FlexMode; }
        private void OnValidate() { OnChangeCondition?.Invoke(); }
    }
}
