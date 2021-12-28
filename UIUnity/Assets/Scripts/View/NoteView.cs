using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Common.Helpers;
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
    public class NoteView : MonoBehaviour, IShowHideView, IView
    {
        private enum NewNodesList
        {
            Lock, None, Wait, Get
        }
        
        [SerializeField] private RectTransform _view;
        [SerializeField] private GameObject _note;
        [SerializeField] private ScrollRect _scroll;
        [SerializeField] private Button _buttonAdd;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private int _notesNumberDownload = 10;
        [Inject] private INoteController _noteController;
        [Inject] private IBrokerMessage _msg;
        private NewNodesList _newNodesListEnum = NewNodesList.Lock;
        
        public void Show()
        {
            _canvasGroup.DOFade(1f, 0.5f)
                .OnComplete(() =>
                {
                    _buttonAdd.interactable = true;
                    if (_newNodesListEnum == NewNodesList.Lock)
                    {
                        InsertNotesToScroll(_noteController.GetFirstNotesPackAsync(_notesNumberDownload).GetAwaiter(),
                            () =>
                            {
                                _newNodesListEnum = NewNodesList.Wait;
                                StartCoroutine(WaifAfterLoad());
                            });
                    }
                });
        }

        public void Hide()
        {
            _canvasGroup
                .DOFade(0f, 0.5f)
                .OnStart(() =>
                {
                    _buttonAdd.interactable = false;
                    _msg.Publish(new ViewMessage
                    {
                        Receiver = typeof(NewNoteView),
                        Sender = this,
                        Message = ViewMessageEnum.Show
                    });
                });
        }

        private void Awake()
        {
            _msg.Receive<ViewMessage>()
                .Where(m => 
                    m.Receiver == typeof(NoteView)
                    && m.Message == ViewMessageEnum.Show)
                .Subscribe(_ => Show())
                .AddTo(gameObject);

            _msg.Receive<AddNoteMessage>()
                .Where(m => m.Receiver == typeof(NoteView))
                .Subscribe(_ =>
                {
                    RemoveLastNoteFromList();
                    CreateNote(_.NewNote).SetSiblingIndex(0);
                })
                .AddTo(gameObject);

            _buttonAdd
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    Hide();
                })
                .AddTo(_buttonAdd.gameObject);

            _scroll.OnValueChangedAsObservable()
                .Where(n => n.y < 0.001f && _newNodesListEnum == NewNodesList.None)
                .Subscribe(n =>
                {
                    _newNodesListEnum = NewNodesList.Get;
                    InsertNotesToScroll(_noteController.GetNotesPackAsync(_notesNumberDownload).GetAwaiter(),
                        () =>
                        {
                            _newNodesListEnum = NewNodesList.Wait;
                            StartCoroutine(WaifAfterLoad());
                        });
                })
                .AddTo(gameObject);
        }
        
        private void InsertNotesToScroll(TaskAwaiter<IList<NoteModel>> notesAwaiter, Action onEnd = null)
        {
            notesAwaiter.OnCompleted(() =>
            {
                var newNotes = notesAwaiter.GetResult();
                if (newNotes.Count < 1)
                    return;
                
                foreach (var note in newNotes)
                {
                    CreateNote(note);
                }
                onEnd?.Invoke();
            });
        }

        private Transform CreateNote(NoteModel note)
        {
            var newNote = Instantiate(_note, _view);
            newNote.GetComponent<NoteTxtSize>().Text = (note.Desc, note.Topic);
            return newNote.transform;
        }

        private void RemoveLastNoteFromList()
        {
            if (_view.childCount < 1)
                return;

            var result = _view.GetChild(_view.childCount - 1);
            if (result != null)
                Destroy(result.gameObject);
        }
        
        private IEnumerator WaifAfterLoad()
        {
            yield return new WaitForSeconds(2f);
            _newNodesListEnum = NewNodesList.None;
        }
    }
}