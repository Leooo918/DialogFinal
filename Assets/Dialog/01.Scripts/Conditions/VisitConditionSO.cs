using UnityEngine;

namespace Dialog
{
    [CreateAssetMenu(menuName = "SO/Condition/Visit")]
    public class VisitConditionSO : ConditionSO
    {   
        public NodeSO node;
        public int visitCnt;    
        // if node's visit count is bigger than this value return true or return false

        public override bool Decision()
        {
            return DialogVisitCounter.GetVisit(node.guid) >= visitCnt;
        }

    }
}
