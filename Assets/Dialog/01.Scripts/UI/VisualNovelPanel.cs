using DG.Tweening;
using Dialog.Tag;
using System;
using System.Collections;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


namespace Dialog
{
    public class VisualNovelPanel : MonoBehaviour
    {
        [SerializeField] private TMP_TagableTextReader _nameText;
        [SerializeField] private TMP_TagableTextReader _contentText;
        [SerializeField] private float _easingDuration;

        private Tween _openCloseTween;
        private bool _isOpen = false;

        private RectTransform RectTrm => transform as RectTransform;
        



        public void Open()
        {
            if (_openCloseTween != null && _openCloseTween.active)
                _openCloseTween.Kill();

            _openCloseTween = RectTrm.DOAnchorPosY(0, _easingDuration);
            _isOpen = true;
        }

        public void Close()
        {
            if (_openCloseTween != null && _openCloseTween.active)
                _openCloseTween.Kill();

            _openCloseTween = RectTrm.DOAnchorPosY(-RectTrm.rect.height, _easingDuration);
            _isOpen = false;  
        }

        public void SetText(TagableText readerName, TagableText content, TextOutputMethodSO textOutputMethod)
        {
            if (_isOpen == false)
            {
                _nameText.SetText(readerName);
                _contentText.SetText(content);
                StartCoroutine(DelayStart(textOutputMethod));

                Open();
                return;
            }

            _nameText.SetText(readerName);
            _contentText.SetText(content);
            StartCoroutine(DelayStart(textOutputMethod));
        }

        private IEnumerator DelayStart(TextOutputMethodSO textOutputMethod)
        {
            yield return null;
            _nameText.EnableAllText();
            _contentText.DisableText();
            _contentText.StartReading(textOutputMethod);
        }
    }
}
