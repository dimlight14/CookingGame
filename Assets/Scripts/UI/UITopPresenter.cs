using UnityEngine;

namespace DefaultNamespace
{
    public class UITopPresenter
    {
        private readonly UITopView _view;
        private readonly IUITimerProvider _timerProvider;
        private readonly IUICustomerProvider _customerProvider;
        private readonly BoosterService _boosterService;

        private int _previousTimeValue;

        public UITopPresenter(UITopView view, IUITimerProvider timerProvider, IUICustomerProvider customerProvider, BoosterService boosterService)
        {
            _view = view;
            _timerProvider = timerProvider;
            _customerProvider = customerProvider;
            _boosterService = boosterService;

            _timerProvider.OnTimeChanged += TimerChangedHandler;
            customerProvider.OnCustomersChanged += CustomerChangedHandler;
            _view.BoosterButton.onClick.AddListener(_boosterService.OnBoosterClicked);
        }

        private void CustomerChangedHandler(int customersActive, int customersTotal)
        {
            var customersString = customersActive.ToString() + " / " + customersTotal.ToString();
            _view.SetCustomersText(customersString);
        }

        private void TimerChangedHandler(float newTime)
        {
            var ceilValue = Mathf.CeilToInt(newTime);
            if (_previousTimeValue == ceilValue)
            {
                return;
            }
            
            _previousTimeValue = ceilValue;
            var minutes = ceilValue / 60;
            var seconds = ceilValue % 60;
            var secondsString = seconds < 10 ? "0" + seconds.ToString() : seconds.ToString();
            var minutesString = minutes < 10 ? "0" + minutes.ToString() : minutes.ToString();

            var timeString = minutesString + ":" + secondsString;
            _view.SetTimerText(timeString);
        }
    }
}