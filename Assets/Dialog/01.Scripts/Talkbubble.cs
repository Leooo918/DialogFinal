using TMPro;
using UnityEngine;

namespace Dialog
{
    public class Talkbubble : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tmp;
        private NodeSO _node;
        private DialogPlayer _dialogPlayer;

        public void SetTalkbubble(NodeSO node)
        {
            if (node is IngameNodeSO normalNode)
            {
                _node = node;
                _tmp.SetText(normalNode.GetText());
                gameObject.SetActive(true);
            }

            gameObject.SetActive(false);
        }

        public void TurnOffTalkbubble()
        {
            gameObject.SetActive(false);
            _node = null;
            _tmp.SetText("");
        }

        public void Init(DialogPlayer player)
        {
            _dialogPlayer = player;
        }
    }
}
