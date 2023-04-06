namespace DefaultNamespace
{
    public class UIEndGamePresenter
    {
        private readonly EndGameUIView _view;
        private readonly GameTest _gameTest;

        public UIEndGamePresenter(EndGameUIView view, GameTest gameTest)
        {
            _view = view;
            _gameTest = gameTest;

            _gameTest.OnGameEnded += OnGameEndedHandler;
            _gameTest.OnGameStarted += OnGameStartedHandler;
            _view.RestartButton.onClick.AddListener(gameTest.RestartGame);
        }

        private void OnGameStartedHandler()
        {
            _view.Hide();
        }

        private void OnGameEndedHandler(bool victory)
        {
            _view.Show(victory);
        }
    }
}