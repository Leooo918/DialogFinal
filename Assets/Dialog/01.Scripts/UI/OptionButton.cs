using Dialog.Animation;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Dialog
{
    public class OptionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public Action<NodeSO> OnClcickEvent;
        [SerializeField] private TMP_AnimationPlayer _tmp;

        private List<TagAnimation> _tagAnims = new List<TagAnimation>();
        private NodeSO _nextNode;

        public void SetOption(Option optionStruct)
        {
            _tmp.SetText(optionStruct.optionTxt, optionStruct.optionTagAnimations);
            _nextNode = optionStruct.nextNode;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClcickEvent?.Invoke(_nextNode);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = Vector3.one * 1.05f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = Vector3.one;
        }
    }
}
