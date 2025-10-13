using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dialog
{
    public abstract class DialogPlayer : MonoBehaviour
    {
        public Action OnDialogStart;
        public Action OnDialogEnd;
        [HideInInspector] public bool stopReading = false;

        [SerializeField] protected DialogSO _dialog;

        protected NodeSO _curReadingNode;
        protected Coroutine _readingNodeRoutine;
        protected bool _isReadingDialog = false;

        [SerializeField] protected float _textOutDelay;
        protected bool _isInputDetected;

        public float TextOutDelay => _textOutDelay;
        public DialogSO Dialog => _dialog;

        public virtual void StartDialog()
        {
            if (_isReadingDialog)
            {
                Debug.LogWarning("A Dialog is already running in this player\nYou can not run multiple dialog in single player");
                return;
            }

            _isReadingDialog = true;
            _curReadingNode = _dialog.nodes.Find(node => node.isFirstNode);
            ReadSingleLine();
        }

        public virtual void EndDialog()
        {
            _isReadingDialog = false;
            _curReadingNode = null;
            OnDialogEnd?.Invoke();
        }

        public virtual void ReadSingleLine()
        {
            if (_curReadingNode == null)
            {
                EndDialog();
                return;
            }

            ReadingNodeRoutine();
            DialogVisitCounter.CountVisit(_curReadingNode.guid);
        }

        protected abstract void ReadingNodeRoutine();

        public virtual void SetTextOutDelay(float delay) => _textOutDelay = delay;


        protected virtual void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (_isReadingDialog) SkipReading();
                else _isInputDetected = true;
            }
            if (_isReadingDialog == false && Keyboard.current.spaceKey.wasReleasedThisFrame)
            {
                _isInputDetected = false;
            }

            if (_isReadingDialog) _isInputDetected = false;
        }

        protected virtual void SkipReading() { }

        public virtual void SetDialog(DialogSO data)
        {
            _dialog = data;
        }
    }
}
