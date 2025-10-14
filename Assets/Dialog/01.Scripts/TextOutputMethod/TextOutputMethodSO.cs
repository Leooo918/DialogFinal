using System;
using UnityEngine;

namespace Dialog
{
    public abstract class TextOutputMethodSO : ScriptableObject
    {
        public Action OnReadText;
        public Action OnCompleteReading;

        protected float _prevTextReadTime;
        protected float _stopTime;
        protected bool _isStoppedTextOutput;

        public virtual void Initialize()
        {
            _isStoppedTextOutput = false;
            _prevTextReadTime = 0;
            _stopTime = 0;

            OnReadText = null;
        }

        public void SetStopTextOutput(bool isStop)
        {
            if (isStop)
            {
                _isStoppedTextOutput = true;
                _stopTime = Time.time;
            }
            else
            {
                _isStoppedTextOutput = false;
                _prevTextReadTime = Time.time - (_stopTime - _prevTextReadTime);
            }
        }

        public abstract bool ReadText(string text, int readingIndex);
    }
}
