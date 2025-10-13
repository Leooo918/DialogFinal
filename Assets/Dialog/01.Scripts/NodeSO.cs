using Dialog.Tag;
using System;
using UnityEngine;

namespace Dialog
{
    public abstract class NodeSO : ScriptableObject
    {
        [HideInInspector] public string guid;
        [HideInInspector] public Vector2 position;
        [ReadOnly] public bool isFirstNode;
        [ReadOnly] public DialogNodeType nodeType;
        [ReadOnly] public TextTagGroupSO animationGroup;
        public Action<NodeSO> OnSetAsFirstNode;


        public virtual void OnEnable() { }
    }
}
