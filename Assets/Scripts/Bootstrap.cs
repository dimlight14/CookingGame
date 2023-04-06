using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Button _saladButton;
    [SerializeField] private Button _porrigeButton;
    [SerializeField] private Button _meatButton;
    [SerializeField] private Button _fishButton;
    [SerializeField] private GameObject customerImageReference;
    [SerializeField] private Updater updater;
    [SerializeField] private Transform customersParent;
    [SerializeField] private CustomersMarkUp customersMarkUp;
    [SerializeField] private List<OrderView> _orderViews;
    [Header("UI")]
    [SerializeField] private EndGameUIView _endGameUIView;
    [SerializeField] private UITopView _uiTopView;
    [SerializeField] private UIBuyBoostersView _boostersView;
    [Header("Settings")]
    [SerializeField] private string gameSettingsToLoad;
    [SerializeField] private string levelSettingsToLoad;

    private GameSettings _gameSettings;
    private LevelSettings _levelSettings;
    private int _maxOrdersOnScreen;
    private GameTest _gameTest;

    private void Start()
    {
        var serializer = new SettingsSerializer();
        _gameSettings = serializer.LoadSettings<GameSettings>(gameSettingsToLoad);
        _levelSettings = serializer.LoadSettings<LevelSettings>(levelSettingsToLoad);
        _maxOrdersOnScreen = _orderViews.Count;

        var orderInitializer = CreateOrderInitializer();
        var orderManager = CreateOrderManager(orderInitializer);
        var customerSpawner = CreateCustomerSpawner(orderManager);
        var boosterService = CreateBoosterService(orderManager);

        _gameTest = new GameTest(orderManager, orderInitializer, customerSpawner, updater, _levelSettings);

        var buttonsUIPresenter = CreateButtonsUIPresenter(orderManager);
        var endGamePresenter = CreateEndGamePresenter(_endGameUIView, _gameTest);
        var topPresenter = CreateTopPresenter(_gameTest, customerSpawner, boosterService);
        var buyBoosters = CreateBoosterPresenter(boosterService);

        _gameTest.StartGame();
    }

    #region UI

    private UIBuyBoostersPresenter CreateBoosterPresenter(BoosterService boosterService)
    {
        return new UIBuyBoostersPresenter(boosterService, _boostersView, updater);
    }

    private UITopPresenter CreateTopPresenter(IUITimerProvider timerProvider, IUICustomerProvider customerProvider, BoosterService boosterService)
    {
        return new UITopPresenter(_uiTopView, timerProvider, customerProvider, boosterService);
    }

    private UIEndGamePresenter CreateEndGamePresenter(EndGameUIView endGameUIView, GameTest gameTest)
    {
        return new UIEndGamePresenter(endGameUIView, gameTest);
    }

    private UIFoodButtonsPresenter CreateButtonsUIPresenter(OrdersManager ordersManager)
    {
        return new UIFoodButtonsPresenter(ordersManager, _fishButton, _meatButton, _porrigeButton, _saladButton);
    }

    #endregion

    private OrdersManager CreateOrderManager(OrderInitializer orderInitializer)
    {
        return new OrdersManager(orderInitializer, _maxOrdersOnScreen);
    }

    private BoosterService CreateBoosterService(OrdersManager ordersManager)
    {
        return new BoosterService(_gameSettings.InitialBoosters, ordersManager);
    }

    private CustomerSpawner CreateCustomerSpawner(OrdersManager ordersManager)
    {
        var factory = new CustomerFactory(
            customerImageReference,
            updater,
            customersParent,
            new ItemPool<Customer>(_maxOrdersOnScreen + 2),
            _gameSettings.CustomerSpeed
        );

        return new CustomerSpawner(factory, updater, _gameSettings.CustomerSpawnDelay, customersMarkUp, ordersManager, _maxOrdersOnScreen);
    }

    private OrderInitializer CreateOrderInitializer()
    {
        var orderFactory = new OrdersFactory(_orderViews);

        return new OrderInitializer(
            orderFactory.CreateOrder,
            new NumberOfFoodsCalculator(_gameSettings.AdditionalIncreaseDecreaseChance / 2, _gameSettings.AdditionalIncreaseDecreaseChance / 2),
            _maxOrdersOnScreen
        );
    }
}