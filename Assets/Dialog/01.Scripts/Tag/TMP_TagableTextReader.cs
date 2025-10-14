using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dialog.Tag
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public partial class TMP_TagableTextReader : MonoBehaviour
    {
        public event Action onCompleteReadLine;
        [SerializeField] private TextTagGroupSO _animationGruop;

        private TextMeshProUGUI _tmp;
        private TMP_TextInfo _tmpTextInfo;

        private List<TextTag> _textAnimInfos;
        private List<CharacterData> _characterDatas;

        private string _text;
        private bool _isTextInit = false;

        private bool _isReadingText = false;
        private TextOutputMethodSO _currentTextOutputMethod;

        public int textLength => _tmpTextInfo.characterCount;
        public int maxVisibleCharacters => _tmp.maxVisibleCharacters;

        private void Awake()
        {
            _tmp = GetComponent<TextMeshProUGUI>();
            _tmpTextInfo = _tmp.textInfo;
        }

        private void LateUpdate()
        {
            ReadText();
            UpdateText();
        }

        private void UpdateText()
        {
            if (_isTextInit == false) return;

            foreach (var data in _characterDatas)
            {
                if(data.isVisible) 
                    data.timer += Time.deltaTime;
            }

            //애니메이션을 CharcterInfo에 적용
            _textAnimInfos.ForEach(animation =>
            {
                if (animation.calcurateEndIndex)
                {
                    for (int i = animation.startIndex; i < animation.endIndex; i++)
                    {
                        var characterInfo = _tmpTextInfo.characterInfo[i];
                        if (characterInfo.isVisible == false) continue;

                        animation.ApplyEffort(_characterDatas[i]);
                    }
                }
                else
                {
                    int index = animation.startIndex;

                    if (index > 0)
                    {
                        if (_characterDatas[index - 1].isVisible == false) return;
                        animation.ApplyEffort(_characterDatas[index - 1]);
                    }
                    else
                    {
                        animation.ApplyEffort(_characterDatas[0]);
                    }
                }

            });

            Vector3[] vertices = new Vector3[_tmpTextInfo.meshInfo[0].vertices.Length];
            Color32[] colors = new Color32[_tmpTextInfo.meshInfo[0].colors32.Length];

            for (int i = 0; i < _tmpTextInfo.characterCount; i++)
            {
                TMP_CharacterInfo characterInfo = _tmpTextInfo.characterInfo[i];
                int vertextIndex = characterInfo.vertexIndex;
                if (characterInfo.isVisible == false) continue;

                for (int j = 0; j < 4; ++j)
                {
                    vertices[vertextIndex + j] = _characterDatas[i].current.positions[j];
                    colors[vertextIndex + j] = _characterDatas[i].current.colors[j];
                }
            }

            _tmp.ForceMeshUpdate();
            for (int i = 0; i < _tmpTextInfo.characterCount; ++i)
            {
                var charcterInfo = _tmpTextInfo.characterInfo[i];
                _tmpTextInfo.meshInfo[charcterInfo.materialReferenceIndex].vertices = vertices;
                _tmpTextInfo.meshInfo[charcterInfo.materialReferenceIndex].colors32 = colors;
            }

            for (int i = 0; i < _tmpTextInfo.meshInfo.Length; ++i)
            {
                var meshInfo = _tmpTextInfo.meshInfo[i];

                meshInfo.mesh.vertices = meshInfo.vertices;
                meshInfo.mesh.colors32 = meshInfo.colors32;

                _tmp.UpdateGeometry(meshInfo.mesh, i);
            }
        }

        #region TextReading

        public void StartReading(TextOutputMethodSO textOutput)
        {
            _isReadingText = true;
            _currentTextOutputMethod = textOutput;
            _currentTextOutputMethod.Initialize();
            _currentTextOutputMethod.SetStopTextOutput(false);
            _currentTextOutputMethod.OnReadText += EnableSingleText;
        }

        public void ReadText()
        {
            if (_isReadingText == false || _currentTextOutputMethod == null) return;

            if (_currentTextOutputMethod.ReadText(_text, _tmp.maxVisibleCharacters))
            {
                _isReadingText = false;
                _currentTextOutputMethod.OnReadText -= EnableSingleText;
            }
        }

        public void StopReadText(float time = -1)
        {
            if (time < 0) _currentTextOutputMethod.SetStopTextOutput(true);
            else StartCoroutine(StopReadingTextRoutine(time));
        }

        public void SkipReadingText()
        {
            EnableAllText();
            _currentTextOutputMethod.OnCompleteReading?.Invoke();
        }

        private IEnumerator StopReadingTextRoutine(float time)
        {
            _currentTextOutputMethod.SetStopTextOutput(true);
            yield return new WaitForSecondsRealtime(time);
            _currentTextOutputMethod.SetStopTextOutput(false);
        }


        #endregion

        #region SETTEXT

        public void SetText(string text, List<TextTag> animations)
        {
            _text = text;
            _textAnimInfos = animations;

            _tmp.SetText(_text);
            _tmp.maxVisibleCharacters = _text.Length;
            _tmp.ForceMeshUpdate(true, true);

            _isTextInit = false;
            StartCoroutine(DelaySetCharacterData());
        }

        public void SetText(TagableText text)
        {
            _text = text.parsedText;
            _textAnimInfos = text.tagAnimations;

            _tmp.SetText(_text);
            _tmp.maxVisibleCharacters = _text.Length;
            _tmp.ForceMeshUpdate(true, true);

            _isTextInit = false;
            StartCoroutine(DelaySetCharacterData());
        }

        private IEnumerator DelaySetCharacterData()
        {
            yield return null;

            _characterDatas = new List<CharacterData>();
            for (int i = 0; i < _tmpTextInfo.characterCount; ++i)
            {
                TMP_CharacterInfo charcterInfo = _tmpTextInfo.characterInfo[i];

                CharacterData data = new CharacterData(charcterInfo.isVisible);
                Vector3[] vertices = _tmpTextInfo.meshInfo[charcterInfo.materialReferenceIndex].vertices;
                Color32[] colors = _tmpTextInfo.meshInfo[charcterInfo.materialReferenceIndex].colors32;
                int vertextIndex = charcterInfo.vertexIndex;

                for (int j = 0; j < 4; ++j)
                {
                    data.Source.positions[j] = vertices[vertextIndex + j];
                    data.current.positions[j] = vertices[vertextIndex + j];

                    data.Source.colors[j] = colors[vertextIndex + j];
                    data.current.colors[j] = colors[vertextIndex + j];
                }

                _characterDatas.Add(data);
            }

            _textAnimInfos.ForEach(animation =>
            {
                if (animation is IGameFlowTag gameFlow)
                    gameFlow.SetData(this);
                if (animation is EventTextTag eventText)
                    eventText.Initialize();
            });

            _isTextInit = true;
        }

        #endregion

        #region DisableText

        //모든 텍스트 투명처리
        public void DisableText()
        {
            _tmp.maxVisibleCharacters = 0;
            _characterDatas?.ForEach(data =>
            {
                //if (data.isContainVertex == false) return;
                data.isVisible = false;
            });
        }

        public void EnableSingleText()
        {
            if (_tmp.maxVisibleCharacters >= textLength) return;

            CharacterData data = _characterDatas[_tmp.maxVisibleCharacters++];
            data.isVisible = true;
        }

        public void EnableAllText()
        {
            _isReadingText = false;
            _currentTextOutputMethod.OnReadText -= EnableSingleText;
            _tmp.maxVisibleCharacters = _characterDatas.Count;
            _currentTextOutputMethod.SetStopTextOutput(false);

            for (int i = 0; i < _characterDatas.Count; ++i)
            {
                //if (_characterDatas[i].isContainVertex == false) continue;
                _characterDatas[i].isVisible = true;
            }
        }

        #endregion
    }
}
