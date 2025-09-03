using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dialog
{
    public abstract class NodeSO : ScriptableObject
    {
        [HideInInspector] public string guid;
        [HideInInspector] public Vector2 position;
        public bool isFirstNode;

        public bool IsCompleteEvent()
        {
            bool isComplete = true;
            return isComplete;
        }
    }
}
