using UnityEngine;

namespace Dialog
{
    [CreateAssetMenu (menuName = "SO/Dialog/TextOutMethod/SingleText")]
    public class SingleTextOutputSO : TextOutputMethodSO
    {
        public float delay = 0.1f;

        public override bool ReadText(string text, int readingIndex)
        {
            if (_isStoppedTextOutput) return false;

            if(text.Length == readingIndex)
            {
                _prevTextReadTime = 0;
                OnCompleteReading?.Invoke();
                return true;
            }

            if (_prevTextReadTime + delay < Time.time)
            {
                _prevTextReadTime = Time.time;
                OnReadText?.Invoke();
            }

            return false;
        }
    }
}
