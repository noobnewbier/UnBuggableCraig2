using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine.Support.EventAggregator;

public class DialogPoster : MonoBehaviour, IHandle<PlayerTakeDamageEvent>
{
    [SerializeField] List<DialogueData> _datas;
    int _currentIndex;
    float _step;
    IEventAggregator _eventAggregator;

    public void Handle(PlayerTakeDamageEvent @event)
    {
        var data = @event.PlayerData;
        if ((1f - data.Health / data.MaxHealth) / _step > _currentIndex && _currentIndex < _datas.Count)
        {
            _eventAggregator.Publish(new DialogueEvent(_datas[_currentIndex]));
            _currentIndex++;
        }
    }

    private void Awake()
    {
        _step = 1f / _datas.Count;
        _eventAggregator = ServiceLocator.EventAggregator;
        _eventAggregator.Subscribe(this);
    }

    private void OnDestroy()
    {
        _eventAggregator.Unsubscribe(this);
    }

}
