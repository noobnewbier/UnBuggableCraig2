using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Engine.Support.EventAggregator;

public class IntroSceneManager : MonoBehaviour, IHandle<DialogueEndEvent>{

    [SerializeField] DialogueData _data;
    [SerializeField] string _sceneName;
    IEventAggregator _eventAggregator;
	// Use this for initialization
	void Start () {
        Screen.fullScreen = true;
        Screen.SetResolution(2560, 1440, true);

        _eventAggregator = ServiceLocator.EventAggregator;

        _eventAggregator.Subscribe(this);
        _eventAggregator.Publish(new DialogueEvent(_data));
	}

    private void OnDestroy()
    {
        _eventAggregator.Unsubscribe(this);
    }

    public void Handle(DialogueEndEvent @event)
    {
        SceneManager.LoadSceneAsync(_sceneName);
    }
}
