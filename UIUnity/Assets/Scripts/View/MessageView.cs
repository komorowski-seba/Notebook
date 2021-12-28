using System;
using Common.Interface;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class MessageView : MonoBehaviour, IShowHideView
{
    [SerializeField] private Text _text;
    [SerializeField] private Button _button;
    [SerializeField] private CanvasGroup _canvas;

    public void Show(string message)
    {
        _text.text = message;
        Show();
    }
    
    public void Show()
    {
        _canvas.DOFade(1f, 0.5f)
            .OnStart(() =>
            {
                _button.interactable = false;
            })
            .OnComplete(() =>
            {
                _button.GetComponent<CanvasGroup>()
                    .DOFade(1f, 0.5f)
                    .OnComplete(() => _button.interactable = true);
            });

        _button.OnClickAsObservable()
            .Throttle(TimeSpan.FromSeconds(0.1))
            .Subscribe(n => Hide())
            .AddTo(gameObject);
    }

    public void Hide()
    {
        _canvas.DOFade(0f, 0.5f)
            .OnComplete(() => Destroy(gameObject));
    }
}
