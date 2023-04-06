using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class EndGameUIView : MonoBehaviour
    {
        private const string WIN_MESSAGE = "You won!";
        private const string LOSE_MESSAGE = "You lost!";
        
        [SerializeField] private TextMeshProUGUI _textField;
        [SerializeField] private Button _restartButton;

        public Button RestartButton => _restartButton;

        public void Show(bool victory)
        {
            _textField.text = victory ? WIN_MESSAGE : LOSE_MESSAGE;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}