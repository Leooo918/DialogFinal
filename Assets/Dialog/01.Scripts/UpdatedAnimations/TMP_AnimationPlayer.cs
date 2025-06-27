using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Burst.Intrinsics.X86.Avx;

namespace Dialog.Animation
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TMP_AnimationPlayer : MonoBehaviour
    {
        [SerializeField] private TextAnimationGroupSO animationGruop;

        private TextMeshProUGUI _tmp;
        private TMP_TextInfo _tmpTextInfo;
        private List<TextAnimation> _textAnimInfos;
        private List<CharacterData> _characterDatas;

        private string _text;
        private bool _isTextInit = false;

        [SerializeField] private string _debugText;

        public int textLength => _text.Length;
        public int maxVisibleCharacters => _tmp.maxVisibleCharacters;

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            //if (Keyboard.current.pKey.wasPressedThisFrame)
            //{
            //    EnableSingleText();
            //}

            //if (Keyboard.current.oKey.wasPressedThisFrame)
            //{
            //    string parseText = _debugText;
            //    List<TextAnimation> animations = TagParser.ParseAnimation(ref parseText, animationGruop.animations);
            //    SetText(parseText, animations);
            //    DisableText();
            //}
        }

        private void LateUpdate()
        {
            if (_isTextInit == false) return;

            //애니메이션을 CharcterInfo에 적용
            _textAnimInfos.ForEach(animation =>
            {
                for (int i = animation.startIndex; i < animation.endIndex; i++)
                {
                    var characterInfo = _tmpTextInfo.characterInfo[i];
                    if (characterInfo.isVisible == false) continue;

                    _characterDatas[i].timer += Time.deltaTime;
                    animation.ApplyEffort(_characterDatas[i], this);
                }
            });

            //CharacterInfo를 바탕으로 TMP에 실제로 적용
            Vector3[] vertices = new Vector3[_tmpTextInfo.meshInfo[0].vertices.Length];
            Color32[] colors = new Color32[_tmpTextInfo.meshInfo[0].colors32.Length];

            for (int i = 0; i < _tmpTextInfo.characterCount; i++)
            {
                vertices[(i * 4)] = _characterDatas[i].current.positions[0];
                vertices[(i * 4) + 1] = _characterDatas[i].current.positions[1];
                vertices[(i * 4) + 2] = _characterDatas[i].current.positions[2];
                vertices[(i * 4) + 3] = _characterDatas[i].current.positions[3];

                colors[(i * 4)] = _characterDatas[i].current.colors[0];
                colors[(i * 4) + 1] = _characterDatas[i].current.colors[1];
                colors[(i * 4) + 2] = _characterDatas[i].current.colors[2];
                colors[(i * 4) + 3] = _characterDatas[i].current.colors[3];
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

        public char GetText(int index)
            => _text[index];

        public void SetText(string text, List<TextAnimation> animations)
        {
            _text = text;
            _textAnimInfos = animations;

            _tmp.SetText(_text);
            _tmp.maxVisibleCharacters = _text.Length;
            for(int i = 0; i <  _tmp.textInfo.characterCount; i++)
            {
                _tmp.textInfo.characterInfo[i].isVisible = true;
            }
            _tmp.ForceMeshUpdate(true, true);

            _isTextInit = false;
            StartCoroutine(DelaySetCharacterData());
        }

        private IEnumerator DelaySetCharacterData()
        {
            yield return null;

            _characterDatas = new List<CharacterData>();
            for (int i = 0; i < _text.Length; ++i)
            {
                CharacterData data = new CharacterData();

                var charcterInfo = _tmp.textInfo.characterInfo[i];
                Vector3[] vertices = _tmp.textInfo.meshInfo[charcterInfo.materialReferenceIndex].vertices;
                Color32[] colors = _tmp.textInfo.meshInfo[charcterInfo.materialReferenceIndex].colors32;
                for (int j = 0; j < 4; ++j)
                {
                    data.source.positions[j] = vertices[(i * 4) + j];
                    data.current.positions[j] = vertices[(i * 4) + j];

                    data.source.colors[j] = colors[(i * 4) + j];
                    data.current.colors[j] = colors[(i * 4) + j];
                }

                _characterDatas.Add(data);
            }

            _isTextInit = true;
        }

        private void Init()
        {
            _tmp = GetComponent<TextMeshProUGUI>();
            _tmpTextInfo = _tmp.textInfo;
        }

        public void DisableText()
        {
            _tmp.maxVisibleCharacters = 0;
            _characterDatas?.ForEach(data =>
            {
                for (int i = 0; i < data.current.colors.Length; i++)
                    data.current.colors[i].a = 0;

                data.isVisible = false;
            });
        }

        public void EnableSingleText()
        {
            if (_tmp.maxVisibleCharacters >= textLength) return;

            CharacterData data = _characterDatas[_tmp.maxVisibleCharacters++];
            data.isVisible = true;

            for (int i = 0; i < data.current.colors.Length; i++)
                data.current.colors[i].a = 255;
        }
    }
}
