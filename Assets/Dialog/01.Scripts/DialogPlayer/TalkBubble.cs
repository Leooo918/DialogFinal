using Dialog.Tag;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Dialog
{
    public class TalkBubble : MonoBehaviour
    {
        [SerializeField] private TMP_TagableTextReader _contentText;
        public event Action OnContentOverEvent;

        public TMP_TagableTextReader ContentTextMeshPro => _contentText;

        public bool IsEnable { get; private set; }

        public void SetEnable(bool value)
        {
            IsEnable = value;
            gameObject.SetActive(value);
        }

        public void SetDisabled()
        {
            SetEnable(false);
        }

        public void SetEnabled()
        {
            SetEnable(true);
        }


        public void ClearContent()
        {
            _contentText.SetText("", new List<TextTag>());
        }
    }
}