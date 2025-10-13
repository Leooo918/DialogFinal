using Dialog.Tag;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dialog
{
    [CreateAssetMenu(menuName = "SO/Dialog/DialogSO")]
    public class DialogSO : ScriptableObject
    {
        public DialogNodeType dialogMode;
        public TextTagGroupSO animGroup;
        public ActorSO defaultPlayerActor;
        public TextTag defaultOpeningTag;

        [Space]
        public List<NodeSO> nodes;


#if UNITY_EDITOR
        public NodeSO CreateNode(Type type)
        {
            NodeSO node = ScriptableObject.CreateInstance(type) as NodeSO;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();
            node.animationGroup = animGroup;
            nodes.Add(node);

            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
            return node;
        }

        public void DeleteNode(NodeSO node)
        {
            nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        public void AddChild(NodeSO parent, NodeSO nextNode, int index)
        {
            if (parent is IngameNodeSO ingame)
            {
                ingame.linkedNode = nextNode;
                return;
            }

            if (parent is VisualNovelNodeSO visualNovel)
            {
                visualNovel.linkedNode = nextNode;
                return;
            }

            if (parent is OptionNodeSO option)
            {
                option.SetOption(nextNode, index);
                return;
            }

            if (parent is BranchNodeSO branch)
            {
                branch.nextNodes[index] = nextNode;
                return;
            }
        }

        public void RemoveChild(NodeSO parent, NodeSO child, int index)
        {
            if (parent is IngameNodeSO ingame)
            {
                ingame.linkedNode = null;
                return;
            }

            if (parent is VisualNovelNodeSO visualNovel)
            {
                visualNovel.linkedNode = null;
                return;
            }

            if (parent is OptionNodeSO option)
            {
                option.RemoveEdge(index);
                return;
            }


            if (parent is BranchNodeSO branch)
            {
                branch.nextNodes[index] = null;
                return;
            }
        }

        public List<NodeSO> GetChildren(NodeSO node)
        {
            List<NodeSO> children = new List<NodeSO>();

            if(node is IngameNodeSO ingame)
                children.Add(ingame.linkedNode);

            if(node is VisualNovelNodeSO visualNovel)
            {
                children.Add(visualNovel.linkedNode);
            }

            if (node is OptionNodeSO option)
                option.options.ForEach(opt => children.Add(opt.nextNode));

            if (node is BranchNodeSO branch)
                children = branch.nextNodes;

            return children;
        }
#endif
    }

    public enum DialogNodeType
    {
        IngameMode = 1,
        VisualNovelMode = 2,
        FlexMode = 4,
    }
}
