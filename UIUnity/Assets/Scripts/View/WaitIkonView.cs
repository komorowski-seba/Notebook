using Common.Interface;
using DG.Tweening;
using UnityEngine;

namespace View
{
    public class WaitIkonView : MonoBehaviour, IShowHideView
    {
        [SerializeField] private RectTransform _image;
        [SerializeField] private CanvasGroup _canvasGroup;

        public void Start()
        {
            _canvasGroup
                .DOFade(1f, 0.5f);

            DOTween.Sequence()
                .Append(_image
                    .transform
                    .DORotate(new Vector3(0, 0, -360), 0.9f, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear))
                .SetLoops(-1);
        }

        public void Show()
        {
        }

        public void Hide()
        {
            _canvasGroup
                .DOFade(0f, 0.5f)
                .OnComplete(() => { Destroy(gameObject); });
        }
    }
}