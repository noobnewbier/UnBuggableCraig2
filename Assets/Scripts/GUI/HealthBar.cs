using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Engine.Support.EventAggregator;

namespace HealthBarGUI
{
    public interface IHealthBarView
    {
        void OnHealthChange(float healthPercentage);
    }

    public interface IHealthBarPresenter : IHandle<PlayerTakeDamageEvent>
    {
        void OnEnable();
        void OnDisable();
    }

    public class HealthBar : MonoBehaviour, IHealthBarView
    {
        [SerializeField] Slider _slider;
        IHealthBarPresenter _presenter;

        private void Awake()
        {
            _presenter = PresenterProvider.GetHealthBarPresenter(this);
        }

        public void OnHealthChange(float healthPercentage)
        {
            _slider.value = healthPercentage;
        }

        private void OnEnable()
        {
            _presenter.OnEnable();
        }

        private void OnDisable()
        {
            _presenter.OnDisable();
        }
    }

    public class HealthBarPresenter : IHealthBarPresenter
    {
        IEventAggregator _eventAggregator;
        IHealthBarView _view;

        public HealthBarPresenter(IHealthBarView view, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _view = view;
        }

        public void Handle(PlayerTakeDamageEvent @event)
        {
            _view.OnHealthChange(@event.PlayerData.Health / @event.PlayerData.MaxHealth);
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

}
