
#if UNITY_EDITOR
using UnityEngine.UIElements;

namespace Dialog
{
    [UxmlElement]
    public partial class SplitView : TwoPaneSplitView
    {
        public void SetConditionInspectorActive(bool activeConditionInspector)
        {
            fixedPaneInitialDimension = activeConditionInspector ? 300 : 1000;
        }
    }
}


#endif