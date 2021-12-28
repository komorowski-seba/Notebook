using System;
using Common.Interface;
using Common.Messages;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace View
{
    public class LoginView : MonoBehaviour, IView
    {
        [SerializeField] private Button _buttonOk;
        [SerializeField] private InputField _inputFieldUser;
        [SerializeField] private InputField _inputFieldPassword;
        [SerializeField] private CanvasGroup _panel;
        [SerializeField] private WaitIkonView _waitIkonView;
        [SerializeField] private MessageView _messageView;
        [Inject] private ILoginController _loginController;
        [Inject] private IBrokerMessage _msg;

        private void Start()
        {
            _buttonOk.OnClickAsObservable()
                .Throttle(TimeSpan.FromSeconds(0.1))
                .Subscribe(_ => OnLogin())
                .AddTo(_buttonOk.gameObject);
        }

        public void OnEnable()
        {
            _inputFieldUser.Select();
        }

        private void OnLogin()
        {
            var waitIkon = Instantiate(_waitIkonView, transform.parent);
            var loginTask = _loginController
                .Login(_inputFieldUser.text, _inputFieldPassword.text)
                .GetAwaiter();

            loginTask.OnCompleted(() =>
            {
                waitIkon.Hide();

                var (resultOk, messageError) = loginTask.GetResult();
                if (resultOk)
                    _panel
                        .DOFade(0f, 0.5f)
                        .OnComplete(() =>
                        {
                            gameObject.SetActive(false);
                            _msg.Publish(new ViewMessage
                            {
                                Receiver = typeof(NoteView),
                                Sender = this,
                                Message = ViewMessageEnum.Show
                            });
                        });
                else
                    Instantiate(_messageView, transform.parent).Show(messageError);
            });
        }
    }
}