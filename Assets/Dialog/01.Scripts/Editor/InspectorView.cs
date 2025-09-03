
#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace Dialog
{
    [UxmlElement]
    public partial class InspectorView : VisualElement
    {
        private UnityEditor.Editor editor;
        private UnityEditor.Editor conditionEditor;

        public InspectorView()
        {
        }

        public void UpdateSelection(NodeView node)
        {
            Clear();

            UnityEngine.Object.DestroyImmediate(editor);
            editor = UnityEditor.Editor.CreateEditor(node.nodeSO);
            IMGUIContainer container = new IMGUIContainer(() =>
            {
                if (editor.target != null)
                {
                    editor.OnInspectorGUI();
                }
            });
            Add(container);
        }

        public void UpdateSelection(ConditionSO condition)
        {
            Clear();

            if (condition == null) return;

            UnityEngine.Object.DestroyImmediate(conditionEditor);
            conditionEditor = UnityEditor.Editor.CreateEditor(condition);
            IMGUIContainer container = new IMGUIContainer(() =>
            {
                if (conditionEditor.target != null)
                {
                    conditionEditor.OnInspectorGUI();
                }
            });
            Add(container);
        }

        public void ClearSelection()
        {
            Clear();
        }
    }
}

#endif