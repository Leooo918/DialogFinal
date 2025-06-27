using Dialog.Animation;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Dialog
{
    public class TalkBubble : MonoBehaviour
    {
        [SerializeField] private TMP_AnimationPlayer _contentText;
        public event Action OnContentOverEvent;

        public TMP_AnimationPlayer ContentTextMeshPro => _contentText;

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
            _contentText.SetText("", new List<TextAnimation>());
        }
    }
}