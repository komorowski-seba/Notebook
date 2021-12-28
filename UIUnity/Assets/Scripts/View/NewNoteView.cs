using System;
using Common.Interface;
using Common.Messages;
using DG.Tweening;
using Model.Note;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace View
{
    public class NewNoteView : MonoBehaviour, IShowHideView, IView
    {
        [SerializeField] private Button _button;
        [SerializeField] private Button _buttonCancel;
        [SerializeField] private InputField _inputAuthor;
        [SerializeField] private InputField _inputText;
        [SerializeField] private CanvasGroup _canvas;
        [SerializeField] private WaitIkonView _waitIkonView;
        [SerializeField] private MessageView _messageView;
        [Inject] private INoteController _noteController;
        [Inject] private IBrokerMessage _msg;

        private bool ButtonsInteractable
        {
            set
            {
                _button.interactable = value;
                _buttonCancel.interactable = value;
            }
        }

        private bool TextInteractable
        {
            set
            {
                _inputAuthor.interactable = value;
                _inputText.interactable = value;
            }
        }

        public void Show()
        {
            _canvas.DOFade(1f, 0.5f)
                .OnComplete(() =>
                {
                    TextInteractable = true;
                    _buttonCancel.interactable = true;
                });
        }

        public void Hide()
        {
            _canvas.DOFade(0f, 0.5f)
                .OnStart(() =>
                {
                    ButtonsInteractable = false;
                    TextInteractable = false;
                    CleanText();
                })
                .OnComplete(() =>
                {
                    _msg.Publish(new ViewMessage
                    {
                        Receiver = typeof(NoteView),
                        Sender = this,
                        Message = ViewMessageEnum.Show
                    });
                });
        }

        private void Awake()
        {
            _msg.Receive<ViewMessage>()
                .Where(n =>
                    n.Receiver == typeof(NewNoteView)
                    && n.Message == ViewMessageEnum.Show)
                .Subscribe(_ => Show())
                .AddTo(gameObject);

            _buttonCancel.OnClickAsObservable()
                .Throttle(TimeSpan.FromSeconds(0.5))
                .Subscribe(n => Hide())
                .AddTo(gameObject);

            _button.OnClickAsObservable()
                .Do(_ =>
                {
                    TextInteractable = false;
                    ButtonsInteractable = false;
                })
                .Subscribe(async n => AddNote())
                .AddTo(_button.gameObject);
        }

        private void Update()
        {
            _button.interactable = _inputAuthor.text.Length > 0 && _inputText.text.Length > 0;
        }

        private void CleanText()
        {
            _inputAuthor.text = string.Empty;
            _inputText.text = string.Empty;
        }

        private void AddNote()
        {
            var waitIkon = Instantiate<WaitIkonView>(_waitIkonView, _canvas.transform);
            var newNote = new NoteModel
            {
                Desc = _inputText.text,
                Topic = _inputAuthor.text
            };
            var result = _noteController
                .AddNewNote(newNote)
                .GetAwaiter();

            result.OnCompleted(() =>
            {
                waitIkon.Hide();
                Hide();

                var (noteId, message) = result.GetResult();
                if (noteId is null)
                    Instantiate(_messageView, transform.parent).Show(message);
                else
                {
                    newNote.Id = noteId;
                    _msg.Publish(new AddNoteMessage
                    {
                        Receiver = typeof(NoteView),
                        Sender = this,
                        NewNote = newNote
                    });
                }
            });
        }
    }
}