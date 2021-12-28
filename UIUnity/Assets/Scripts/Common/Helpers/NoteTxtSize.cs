using UnityEngine;
using UnityEngine.UI;

namespace Common.Helpers
{
    public class NoteTxtSize : MonoBehaviour
    {
        [SerializeField] private RectTransform labelText;
        [SerializeField] private RectTransform descText;
        private RectTransform myTransform;
        private Vector2? size;

        public (string note, string author) Text
        {
            get => (descText.GetComponent<Text>()?.text ?? "", labelText.GetComponent<Text>()?.text ?? "");
            set
            {
                descText.GetComponent<Text>().text = $"\n{value.note}\n" ;
                labelText.GetComponent<Text>().text = value.author;
            }
        }
    
        private void Start()
        {
            myTransform = (RectTransform) transform;
        }

        private void LateUpdate()
        {
            var root = (RectTransform) myTransform.root;
            myTransform.sizeDelta = new Vector2(root.sizeDelta.x * 0.95f, labelText.sizeDelta.y + descText.sizeDelta.y);
        }
    }
}
