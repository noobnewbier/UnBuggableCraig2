using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TurretGUI
{
    public interface ITurretPanelPresenter
    {
        void OnMouseLeftClick();

        void OnMouseScroll(bool isForward);

        void ShowCurrentSelectedTurret();
    }

    public interface ITurretPanelView
    {
        void SetTurretText(string turretName, int cost);
    }

    public class TurretPanelView : MonoBehaviour, ITurretPanelView
    {
        [SerializeField] Text _turretNameText;
        [SerializeField] Text _turretCostText;
        ITurretPanelPresenter _presenter;
        
        private void Awake()
        {
            _presenter = PresenterProvider.GetTurretPanelPresenter(this);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0)) //left click
            {
                _presenter.OnMouseLeftClick();
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                _presenter.OnMouseScroll(Input.GetAxis("Mouse ScrollWheel") > 0);
            }
        }

        public void SetTurretText(string turretName, int cost)
        {
            _turretNameText.text = turretName;
            _turretCostText.text = "$" + cost;
        }

        private void OnEnable()
        {
            _presenter.ShowCurrentSelectedTurret();
        }
    }

    public class TurretPanelPresenter : ITurretPanelPresenter
    {
        readonly TurretModel _model;
        readonly TurretPlacer _turretPlacer;
        readonly ShopManager _shopManager;
        readonly ITurretPanelView _view;

        public TurretPanelPresenter(TurretModel model, ShopManager shopManager, TurretPlacer turretPlacer, ITurretPanelView view)
        {
            _model = model;
            _shopManager = shopManager;
            _turretPlacer = turretPlacer;
            _view = view;
        }

        public void OnMouseLeftClick()
        {
            GameObject selectedTurret = _model.SelectedTurret;
            if (_shopManager.Purchase(selectedTurret.GetComponent<Turret>().TurretData)) // if have enough money
            {
                _turretPlacer.PlaceTurret(selectedTurret);
            }
        }

        public void OnMouseScroll(bool isForward)
        {
            _model.ChooseOtherOption(isForward);

            ShowCurrentSelectedTurret();
        }

        public void ShowCurrentSelectedTurret()
        {
            TurretData turretData = _model.SelectedTurret.GetComponent<Turret>().TurretData;
            _view.SetTurretText(turretData.Turretname, turretData.Cost);
        }
    }

    public class TurretModel
    {
        public GameObject SelectedTurret { get { return _availableTurrets[_currentIndex % _availableTurrets.Count]; } }
        List<GameObject> _availableTurrets;
        int _currentIndex = 1;

        public TurretModel(List<GameObject> availableTurrets)
        {
            _availableTurrets = availableTurrets;
        }

        public void ChooseOtherOption(bool isNext)
        {
            _currentIndex += isNext ? 1 : -1;
            _currentIndex = Mathf.Abs(_currentIndex);
        }
    }
}