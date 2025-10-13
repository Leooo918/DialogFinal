using UnityEngine;

namespace Dialog.Tag
{
    public abstract class DialogFlowTag : EventTextTag, IGameFlowTag
    {
        protected TMP_TagableTextReader _tagableTextReader;

        public virtual void SetData(TMP_TagableTextReader player)
        {
            _tagableTextReader = player;
        }
    }
}
