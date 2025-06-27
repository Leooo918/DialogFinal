using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog.VisualNovel
{
    public class VisualNovelOption : MonoBehaviour
    {
        [SerializeField] private VerticalLayoutGroup _layoutGroup;
        [SerializeField] private float _optionHeight = 100;
        [SerializeField] private CanvasGroup _visual;
        private RectTransform LayoutRectTrm => _layoutGroup.transform as RectTransform;

        private List<OptionButton> optionButtonList;

        public void AddOption(OptionNodeSO option, Action<NodeSO> optionSelectEvent)
        {
            _visual.alpha = 1;
            _visual.blocksRaycasts = true;
            _visual.interactable = true;

            optionButtonList = new List<OptionButton>();
            int optionCnt = option.options.Count;

            for(int i = 0; i < optionCnt; i++)
            {
                OptionButton optionButton = Instantiate(option.optionPrefab, _layoutGroup.transform);
                optionButton.SetOption(option.options[i]);
                optionButton.OnClcickEvent += optionSelectEvent;
                optionButton.OnClcickEvent += OnSelectOption;
                optionButtonList.Add(optionButton);
            }

            float height = _optionHeight * optionCnt +
                _layoutGroup.spacing * (optionCnt - 1);

            LayoutRectTrm.sizeDelta = new Vector2(LayoutRectTrm.sizeDelta.x, height);
        }

        private void OnSelectOption(NodeSO nextNode)
        {
            optionButtonList.ForEach(option => Destroy(option.gameObject));
            optionButtonList.Clear();

            _visual.alpha = 0;
            _visual.blocksRaycasts = false;
            _visual.interactable = false;
        }
    }
}
