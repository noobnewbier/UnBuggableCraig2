using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine.Support.EventAggregator;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TimeText : MonoBehaviour, IHandle<SurvivedTimeEvent>
{
    IEventAggregator _eventAggregator;
    Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
        _eventAggregator = ServiceLocator.EventAggregator;
    }

    private void OnEnable()
    {
        _eventAggregator.Subscribe(this);
    }

    private void OnDisable()
    {
        _eventAggregator.Unsubscribe(this);
    }

    public void Handle(SurvivedTimeEvent @event)
    {
        _text.text = "Time: \n" + System.TimeSpan.FromSeconds(@event.Time).ToString(@"mm\:ss");
    }
}
