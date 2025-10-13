using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dialog
{
    public class VisualNovelDialogPlayer : DialogPlayer
    {
        [SerializeField] private VisualNovelPanel _panel;
        [SerializeField] private VisualNovelOption _option;

        private NodeSO _nextNode;

        protected override void Update()
        {
            base.Update();
            if (Keyboard.current.iKey.wasPressedThisFrame)
            {
                StartDialog();
            }
        }

        public override void EndDialog()
        {
            _panel.Close();
            base.EndDialog();
        }


        protected override void ReadingNodeRoutine()
        {
            _isReadingDialog = true;
            if (_curReadingNode is VisualNovelNodeSO visualNovel)
            {
                _nextNode = visualNovel.linkedNode;
                visualNovel.textOutputMethod.OnCompleteReading += OnCompleteRead;

                _panel.SetText(visualNovel.readerName, visualNovel.contentName, visualNovel.textOutputMethod);
            }
            else if (_curReadingNode is OptionNodeSO option)
            {
                _panel.Close();
                ReadingOptionNodeRoutine(option);
            }
            else if (_curReadingNode is BranchNodeSO branch)
            {
                _panel.Close();
                JudgementCondition(branch);
            }
        }


        private void OnCompleteRead()
        {
            if (_curReadingNode is VisualNovelNodeSO visualNovel) visualNovel.textOutputMethod.OnCompleteReading -= OnCompleteRead;
            StartCoroutine(WaitNodeRoutine(() => _isInputDetected, null));
        }

        private IEnumerator WaitNodeRoutine(Func<bool> waitPredict, Action endAction)
        {
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(waitPredict);

            endAction?.Invoke();
            _curReadingNode = _nextNode;
            _isReadingDialog = false;

            yield return null;
            stopReading = false;
            ReadSingleLine();
        }


        private void ReadingOptionNodeRoutine(OptionNodeSO node)
        {
            _option.AddOption(node, OnSelectOption);
        }

        private void OnSelectOption(NodeSO nextNode)
        {
            _curReadingNode = nextNode;
            ReadSingleLine();
        }


        private void JudgementCondition(BranchNodeSO branch)
        {
            bool decision = branch.condition.Decision();
            _curReadingNode = branch.nextNodes[decision ? 0 : 1];
            ReadSingleLine();
        }
    }
}
