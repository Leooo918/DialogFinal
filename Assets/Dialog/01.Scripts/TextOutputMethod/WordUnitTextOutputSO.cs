using UnityEngine;

namespace Dialog
{
    [CreateAssetMenu(menuName = "SO/Dialog/TextOutMethod/WordUnit")]
    public class WordUnitTextOutputSO : TextOutputMethodSO
    {
        public float characterDelay = 0.01f;
        public float wordDelay = 0.2f;

        public override bool ReadText(string text, int readingIndex)
        {
            if (_isStoppedTextOutput) return false;

            if (text.Length == readingIndex)
            {
                _prevTextReadTime = 0;
                OnCompleteReading?.Invoke();
                return true;
            }

            if (text[readingIndex] == ' ')
            {
                if (_prevTextReadTime + wordDelay < Time.time)
                {
                    _prevTextReadTime = Time.time;
                    //space바 포함해서 2번 넘겨야함ㅇㅇ
                    OnReadText?.Invoke();
                    OnReadText?.Invoke();
                }
            }
            else
            {
                if (_prevTextReadTime + characterDelay < Time.time)
                {
                    _prevTextReadTime = Time.time;
                    OnReadText?.Invoke();
                }
            }


            return false;
        }
    }
}
