using Dialog.Animation;
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

            if (_curReadingNode is NormalNodeSO node)
            {
                DialogActorManager.Instance.TryGetActor(node.reader.actorName, out _currentActor);
            }

            _isReadingDialog = true;
            if (_curReadingNode is NormalNodeSO normal)
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

        private IEnumerator ReadingNormalNodeRoutine(NormalNodeSO node)
        {
            TMP_AnimationPlayer animationPlayer = _currentActor.ContentText;

            animationPlayer.SetText(node.GetContents(), node.GetAllAnimations());
            yield return null;
            animationPlayer.DisableText();
            _isReadingDialog = true;

            while (animationPlayer.maxVisibleCharacters < animationPlayer.textLength)
            {
                animationPlayer.EnableSingleText();
                if (animationPlayer.GetText(animationPlayer.maxVisibleCharacters - 1) == ' ') continue;

                yield return new WaitForSeconds(_textOutDelay);
                yield return new WaitUntil(() => stopReading == false);
            }

            _nextNode = node.nextNode;
            StartCoroutine(WaitNodeRoutine(GetInput, _currentActor.OnCompleteNode));
        }


        private void ReadingOptionNodeRoutine(OptionNodeSO node)
        {
            _option.SetOption(node, OnSelectOption);
            //StartCoroutine(WaitNodeRoutine(() => _optionSelected, null));
        }

        private void OnSelectOption(Option option)
        {
            _optionTalk = _curReadingNode as OptionNodeSO;
            NormalNodeSO nodeInstance = ScriptableObject.CreateInstance<NormalNodeSO>();
            nodeInstance.animationGruop = Dialog.animGroup;
            nodeInstance.SetNormalNodeByOption(option, Dialog.defaultPlayerActor);
            _curReadingNode = nodeInstance;
            //if (DialogActorManager.Instance.TryGetActor(Dialog.defaultPlayerActor.actorName, out _currentActor) == false)
            //{
            //    Debug.LogError($"Player Actor is not set in the gameScene : Player named{Dialog.defaultPlayerActor.actorName}");
            //}
            
            
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
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(waitPredict);

            endAction?.Invoke();
            _curReadingNode = _nextNode;
            _isReadingDialog = false;

            if (_optionTalk)
            {
                _option.Close();
                _optionTalk = null;
            }

            yield return null;
            stopReading = false;
            ReadSingleLine();
        }

        #endregion
    }
}
