using Dialog.Tag;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dialog
{
    public class InGameDialogPlayer : DialogPlayer
    {
        [SerializeField] private IngameDialogOption _option;

        private Actor _currentActor;
        private OptionNodeSO _optionTalk;
        private NodeSO _nextNode;

        protected override void Update()
        {
            base.Update();
            if (Keyboard.current.iKey.wasPressedThisFrame)
            {
                StartDialog();
            }
        }


        #region ReadingRoutines

        protected override void ReadingNodeRoutine()
        {
            _isReadingDialog = false;

            //Text 노드일 경우 Actor를 갸져와
            if (_curReadingNode is IngameNodeSO node)
                DialogActorManager.TryGetActor(node.reader.actorName, out _currentActor);

            _isReadingDialog = true;
            if (_curReadingNode is IngameNodeSO normal)
            {
                _currentActor?.personalTalkBubble.SetEnabled();
                _readingNodeRoutine = StartCoroutine(ReadingNormalNodeRoutine(normal));
            }
            else if (_curReadingNode is OptionNodeSO option)
            {
                ReadingOptionNodeRoutine(option);
            }
            else if (_curReadingNode is BranchNodeSO branch)
            {
                JudgementCondition(branch);
            }
        }

        private IEnumerator ReadingNormalNodeRoutine(IngameNodeSO node)
        {
            TMP_TagableTextReader animationPlayer = _currentActor.ContentText;

            animationPlayer.SetText(node.text);
            yield return null;
            animationPlayer.DisableText();
            node.textOutputMethod.OnCompleteReading += OnCompleteRead;
            animationPlayer.StartReading(node.textOutputMethod);
            _nextNode = node.linkedNode;
        }

        private void OnCompleteRead()
        {
            if (_curReadingNode is IngameNodeSO ingame) ingame.textOutputMethod.OnCompleteReading -= OnCompleteRead;
            StartCoroutine(WaitNodeRoutine(() => _isInputDetected, _currentActor.OnCompleteNode));
        }


        private void ReadingOptionNodeRoutine(OptionNodeSO node)
        {
            _option.SetOption(node, OnSelectOption);
            //StartCoroutine(WaitNodeRoutine(() => _optionSelected, null));
        }

        private void OnSelectOption(Option option)
        {
            _optionTalk = _curReadingNode as OptionNodeSO;
            IngameNodeSO nodeInstance = ScriptableObject.CreateInstance<IngameNodeSO>();
            nodeInstance.animationGroup = Dialog.animGroup;
            nodeInstance.SetNormalNodeByOption(option, Dialog.defaultPlayerActor);
            _curReadingNode = nodeInstance;

            ReadSingleLine();
        }

        private void JudgementCondition(BranchNodeSO branch)
        {
            bool decision = branch.condition.Decision();
            _curReadingNode = branch.nextNodes[decision ? 0 : 1];
            ReadSingleLine();
        }

        private IEnumerator WaitNodeRoutine(Func<bool> waitPredict, Action endAction)
        {
            _isReadingDialog = false;
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(waitPredict);

            endAction?.Invoke();
            _curReadingNode = _nextNode;

            if (_optionTalk)
            {
                _option.Close();
                _optionTalk = null;
            }

            yield return null;
            stopReading = false;
            ReadSingleLine();
        }

        protected override void SkipReading()
        {
            if(_isReadingDialog)
            {
                _isReadingDialog = false;
                StopCoroutine(_readingNodeRoutine);
                _currentActor.ContentText.SkipReadingText();
            }
        }

        #endregion
    }
}
