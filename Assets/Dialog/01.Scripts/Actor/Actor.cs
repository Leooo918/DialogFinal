using Dialog;
using Dialog.Animation;
using System;
using TMPro;
using UnityEngine;


namespace Dialog
{
    [Serializable]
    public class Actor
    {
        public ActorSO actorInfo;
        public Transform target;
        public SpriteRenderer spriteRenderer;
        public TalkBubble personalTalkBubble;

        public TMP_AnimationPlayer ContentText => personalTalkBubble.ContentTextMeshPro;

        public void OnCompleteNode()
        {
            personalTalkBubble.SetDisabled();
        }

    }
}