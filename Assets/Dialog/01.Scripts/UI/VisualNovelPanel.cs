using DG.Tweening;
using TMPro;
using UnityEngine;


namespace Dialog
{
    public class VisualNovelPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _contentText;
        [SerializeField] private float _easingDuration;

        private Tween _openCloseTween;

        private RectTransform RectTrm => transform as RectTransform;


        public void Open()
        {
            if (_openCloseTween != null && _openCloseTween.active)
                _openCloseTween.Kill();

            _openCloseTween = RectTrm.DOAnchorPosY(0, _easingDuration);
        }

        public void Close()
        {
            if (_openCloseTween != null && _openCloseTween.active)
                _openCloseTween.Kill();

            _openCloseTween = RectTrm.DOAnchorPosY(-RectTrm.rect.height, _easingDuration);
        }
    }
}
