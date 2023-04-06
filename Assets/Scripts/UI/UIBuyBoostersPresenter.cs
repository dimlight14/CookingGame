namespace DefaultNamespace
{
    public class UIBuyBoostersPresenter
    {
        private readonly BoosterService _boosterService;
        private readonly UIBuyBoostersView _view;
        private readonly Updater _updater;
        
        public UIBuyBoostersPresenter(BoosterService boosterService, UIBuyBoostersView view, Updater updater)
        {
            _boosterService = boosterService;
            _view = view;
            _updater = updater;

            _boosterService.OnBuyBoosterShow += Show;
            _view.BuyBoosterButton.onClick.AddListener(OnBuyClikedHandler);
            _view.RefuseButton.onClick.AddListener(OnRefuseClikedHandler);
        }

        private void OnRefuseClikedHandler()
        {
            _updater.Resume();
            _view.Hide();
        }

        private void OnBuyClikedHandler()
        {
            _updater.Resume();
            _view.Hide();
            _boosterService.BuyBooster();
        }

        private void Show()
        {
            _view.Show();
            _updater.Pause();
        }
    }
}