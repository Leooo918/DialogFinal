using Dialog.Tag;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dialog
{
    public class OptionNodeSO : NodeSO
    {
        public TextTagGroupSO animationGroup;
        public List<Option> options = new List<Option>();
        public OptionButton optionPrefab;

        public Action OnOptionChange;


        public override void OnEnable()
        {
            nodeType = DialogNodeType.FlexMode;
            options.ForEach(option => option.optionTxt.ParseTag(animationGroup));
        }

        private void OnValidate()
        {
            OnOptionChange?.Invoke();
            options.ForEach(option => option.optionTxt.ParseTag(animationGroup));
        }

        public void AddOption()
        {
            options.Add(new Option());
            OnOptionChange?.Invoke();
        }

        public void SetOption(NodeSO nextNode, int index)
        {
            options[index].nextNode = nextNode;
            OnOptionChange?.Invoke();
        }

        public void RemoveOption(int idx)
        {
            options.RemoveAt(idx);
            OnOptionChange?.Invoke();
        }

        public void RemoveEdge(int idx)
        {
            options[idx].nextNode = null;
        }
    }

    [Serializable]
    public class Option
    {
        public TagableText optionTxt;
        public TextOutputMethodSO textOutputMethod;
        [HideInInspector] public NodeSO nextNode;

        public Option()
        {
            optionTxt = new TagableText("");
            nextNode = null;
        }

        public void Init(TextTagGroupSO animationGroup)
        {
        }
    }
}
