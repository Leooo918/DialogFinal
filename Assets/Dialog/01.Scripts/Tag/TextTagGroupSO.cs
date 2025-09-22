using System.Collections.Generic;
using UnityEngine;

namespace Dialog.Tag
{
    [CreateAssetMenu(menuName = "SO/Dialog/TagGroup")]
    public class TextTagGroupSO : ScriptableObject
    {
        public List<TextTagSO> tagList;
    }
}
