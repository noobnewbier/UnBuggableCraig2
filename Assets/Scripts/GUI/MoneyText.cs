using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Engine.Support.EventAggregator;

[RequireComponent(typeof(Text))]
public class MoneyText : MonoBehaviour, IHandle<MoneyEvent>
{
    Text _text;
    IEventAggregator _eventAggregator;

    void Awake()
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

    public void Handle(MoneyEvent @event)
    {
        _text.text = ((int)@event.Money).ToString();
    }

}
