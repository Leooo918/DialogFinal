using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    [CreateAssetMenu(menuName = "SO/Dialog/Condition/CoinCondition")]
    public class CoinConditionSO : ConditionSO
    {
        public int coinLess;
        //if coin is bigger than this value return true or return false

        public override bool Decision() => true;
    }
}
