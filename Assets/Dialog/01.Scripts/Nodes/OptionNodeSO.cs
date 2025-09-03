using Dialog.Animation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dialog
{
    public class OptionNodeSO : NodeSO
    {
        public TextAnimationGroupSO animationGroup;
        public List<Option> options = new List<Option>();
        public OptionButton optionPrefab;

        public Action OnOptionChange;

        public void AddOption()
        {
            options.Add(new Option());
            OnOptionChange?.Invoke();
        }

        public void AddOption(NodeSO nextNode, int index)
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

        public void RemoveOption(NodeSO nextNode)
        {
            for (int i = 0; i < options.Count; i++)
            {
                if (options[i].nextNode == nextNode)
                {
                    options[i] = null;
                    break;
                }
            }

            OnOptionChange?.Invoke();
        }

        private void OnEnable()
        {
            options.ForEach(option => option.Init(animationGroup));
        }
    }

    [Serializable]
    public class Option
    {
        public string option;
        [HideInInspector] public string optionTxt;
        [SerializeReference] public List<TextAnimation> optionTagAnimations = new();


        [HideInInspector]public NodeSO nextNode;

        public Option()
        {
            option = "";
            nextNode = null;
        }

        public void Init(TextAnimationGroupSO animationGroup)
        {
           optionTxt = option;
           optionTagAnimations = TagParser.ParseAnimation(ref optionTxt, animationGroup.animations);
        }
    }
}
