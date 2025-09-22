#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialog
{
    [UxmlElement]
    public partial class DialogView : GraphView
    {
        public Action<NodeView> OnNodeSelected;
        public Action OnNodeRemoved;
        private DialogSO _dialog;

        public DialogSO Dialog => _dialog;


        public DialogView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }

        public void PopulateTree(DialogSO dialog)
        {
            _dialog = dialog;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);  //자기가 가지고 있는 모든 엘리멘트 삭제
            graphViewChanged += OnGraphViewChanged;


            _dialog.nodes.ForEach(CreateNodeView);

            //자식한테 엣지 연결해주기
            _dialog.nodes.ForEach(node =>
            {
                NodeView parent = FindNodeView(node);
                var children = _dialog.GetChildren(node);
                int index = 0;

                children.ForEach(cn =>
                {
                    if (cn != null)
                    {
                        NodeView childNV = FindNodeView(cn);

                        //output이 여러개인 경우
                        if (parent.output == null)
                        {
                            //시작 노드일 경우
                            if (childNV.input == null)
                            {
                                _dialog.RemoveChild(parent.nodeSO, childNV.nodeSO, index);
                                return;
                            }

                            Edge edge = parent.outputs[index].ConnectTo(childNV.input);
                            AddElement(edge);
                        }
                        else
                        {
                            //시작 노드일 경우
                            if (childNV.input == null)
                            {
                                _dialog.RemoveChild(parent.nodeSO, childNV.nodeSO, index);
                                return;
                            }

                            Edge edge = parent.output.ConnectTo(childNV.input);
                            AddElement(edge);
                        }
                    }
                    index++;
                });
            });
        }

        private NodeView FindNodeView(NodeSO node) => GetNodeByGuid(node.guid) as NodeView;

        private GraphViewChange OnGraphViewChanged(GraphViewChange changeInfo)
        {
            if (changeInfo.elementsToRemove != null)
            {
                changeInfo.elementsToRemove.ForEach(elem =>
                {
                    if (elem is NodeView nv)
                    {
                        _dialog.DeleteNode(nv.nodeSO);
                    }

                    if (elem is Edge edge)
                    {
                        int index = 0;
                        NodeView parent = edge.output.node as NodeView;
                        NodeView child = edge.input.node as NodeView;

                        if (parent.nodeSO is OptionNodeSO || parent.nodeSO is BranchNodeSO)
                        {
                            for (int i = 0; i < parent.outputs.Count; i++)
                            {
                                if (parent.outputs[i] == edge.output)
                                {
                                    index = i;
                                }
                            }
                        }

                        _dialog.RemoveChild(parent.nodeSO, child.nodeSO, index);
                    }
                });
                OnNodeRemoved?.Invoke();
            }

            if (changeInfo.edgesToCreate != null)
            {
                changeInfo.edgesToCreate.ForEach(elem =>
                {
                    int index = 0;
                    NodeView parent = elem.output.node as NodeView;
                    NodeView child = elem.input.node as NodeView;

                    if (parent.nodeSO is OptionNodeSO || parent.nodeSO is BranchNodeSO)
                    {
                        for (int i = 0; i < parent.outputs.Count; i++)
                        {
                            if (parent.outputs[i] == elem.output)
                            {
                                index = i;
                            }
                        }
                    }

                    _dialog.AddChild(parent.nodeSO, child.nodeSO, index);
                });
            }

            return changeInfo;
        }

        private void CreateNodeView(NodeSO node)
        {
            NodeView nv = new NodeView(node);
            nv.OnNodeSelected = OnNodeSelected;
            AddElement(nv);

            node.OnSetAsFirstNode = OnSetAsFirstNode;
            if (node is OptionNodeSO option) option.OnOptionChange = Refresh;
            if (node is BranchNodeSO branch) branch.OnChangeCondition = Refresh;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            if (_dialog == null)
            {
                evt.StopPropagation();
                Debug.LogWarning("Dialog Not Selected");
                return;
            }

            var types = TypeCache.GetTypesDerivedFrom<NodeSO>();
            Vector2 mousePosition = this.ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);

            foreach (Type type in types)
            {
                NodeSO node = (NodeSO)Activator.CreateInstance(type);
                node.OnEnable();

                if (_dialog.dialogMode == DialogNodeType.FlexMode || node.nodeType == DialogNodeType.FlexMode)
                {
                    evt.menu.AppendAction(type.Name,
                        (a) => CreateNode(type, mousePosition));
                }
                else if (_dialog.dialogMode == node.nodeType)
                {
                    evt.menu.AppendAction(type.Name,
                        (a) => CreateNode(type, mousePosition));
                }
            }
        }

        private void CreateNode(Type type, Vector2 position)
        {
            NodeSO node = _dialog.CreateNode(type);
            node.position = position;
            node.OnEnable();

            if (_dialog.nodes.Count == 1) node.isFirstNode = true;
            CreateNodeView(node);
        }

        private void OnSetAsFirstNode(NodeSO node)
        {
            for (int i = 0; i < _dialog.nodes.Count; i++)
            {
                _dialog.nodes[i].isFirstNode = (_dialog.nodes[i] == node);
            }
            Refresh();
        }

        private void Refresh()
        {
            PopulateTree(_dialog);
        }
    }

}

#endif