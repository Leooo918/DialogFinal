using UnityEngine;

namespace Dialog.Tag
{
    public class DialogStopTag : DialogFlowTag
    {
        public bool isStoppingWithTimer = true;
        [Condition("isStoppingWithTimer", true)] public float stoppingTime;


        protected override void PlayEvent()
        {
            _tagableTextReader.StopReadText(isStoppingWithTimer ? stoppingTime : -1);
        }

        public override TextTag Instantiate()
        {
            DialogStopTag tag = new DialogStopTag();
            tag.isStoppingWithTimer |= isStoppingWithTimer;
            tag.stoppingTime = stoppingTime;
            tag.calcurateEndIndex = false;

            return tag;
        }
    }
}
