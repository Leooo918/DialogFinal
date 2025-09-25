using Dialog.Tag;
using System;
using UnityEngine;


namespace Dialog
{
    [Serializable]
    public class Actor
    {
        public ActorSO actorInfo;
        public Transform actorTransform;
        public SpriteRenderer spriteRenderer;
        public TalkBubble personalTalkBubble;
        //Customize here to add more work at actor

        public TMP_TagableTextReader ContentText => personalTalkBubble.ContentTextMeshPro;

        public void OnCompleteNode()
        {
            personalTalkBubble.SetDisabled();
        }

    }
}