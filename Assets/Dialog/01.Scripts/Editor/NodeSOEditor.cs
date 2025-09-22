using Dialog;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NodeSO), true)]
public class NodeSOEditor : Editor
{
    private NodeSO _target;

    private void OnEnable()
    {
        _target = (NodeSO)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if(GUILayout.Button("SetAsFirstNode"))
        {
            _target.OnSetAsFirstNode?.Invoke(_target);
        }
    }
}
