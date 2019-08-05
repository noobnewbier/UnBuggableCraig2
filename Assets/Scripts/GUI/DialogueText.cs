using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine.Support.EventAggregator;
using UnityEngine.UI;

public struct DialogueEvent
{
    public DialogueData Data { get; private set; }

    public DialogueEvent(DialogueData data)
    {
        Data = data;
    }
}

public struct DialogueEndEvent { }

namespace DialogueGUI
{
    public interface IDialogueView
    {
        void OnNextDialogue(string dialogue);
        void OnDialogueFinish();
        void OnBeginDialogue();
    }

    public interface IDialoguePresenter
    {
        void NextDialogue();
        void OnEnable();
        void OnDisable();
    }

    public class DialogueText : MonoBehaviour, IDialogueView
    {
        [SerializeField] Image _image;
        [SerializeField] Text _text;
        IDialoguePresenter _presenter;

        public void OnDialogueFinish()
        {
            _image.enabled = false;
            _text.enabled = false;
        }

        public void OnNextDialogue(string dialogue)
        {
            _text.text = dialogue;
        }

        private void Awake()
        {
            _presenter = PresenterProvider.GetDialoguePresenter(this);

            _image.enabled = false;
            _text.enabled = false;
        }

        void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                _presenter.NextDialogue();
            }
        }

        private void OnEnable()
        {
            _presenter.OnEnable();
        }

        private void OnDisable()
        {
            _presenter.OnDisable();

            _image.enabled = false;
            _text.enabled = false;
        }

        public void OnBeginDialogue()
        {
            _image.enabled = true;
            _text.enabled = true;
        }
    }

    public class DialoguePresenter : IDialoguePresenter, IHandle<DialogueEvent> 
    {
        DialogueModel _model;
        IDialogueView _view;
        IEventAggregator _eventAggregator;

        public DialoguePresenter(IDialogueView view, IEventAggregator eventAggregator)
        {
            _view = view;
            _eventAggregator = eventAggregator;
        }

        public void Handle(DialogueEvent @event)
        {
            _model = new DialogueModel(@event.Data);

            _view.OnBeginDialogue();
            NextDialogue();
        }

        public void NextDialogue()
        {
            if (_model == null) return;

            if (_model.HasNextDialogue)
            {
                _view.OnNextDialogue(_model.GetDialogue());
            }
            else
            {
                _model = null;
                _view.OnDialogueFinish();
                _eventAggregator.Publish(new DialogueEndEvent());
            }
        }

        public void OnDisable()
        {
            _eventAggregator.Unsubscribe(this);
        }

        public void OnEnable()
        {
            _eventAggregator.Subscribe(this);
        }
    }

    internal class DialogueModel
    {
        DialogueData _data;
        int _currentIndex;

        public bool HasNextDialogue { get { return _currentIndex < _data.Dialogues.Length; } }

        public DialogueModel(DialogueData data)
        {
            _data = data;
        }

        public string GetDialogue()
        {
            Debug.Log("getting");
            return _data.Dialogues[_currentIndex++];
        }
    }

}