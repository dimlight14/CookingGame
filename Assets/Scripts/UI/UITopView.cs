using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class UITopView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI customersText;
        [SerializeField] private TextMeshProUGUI timer;
        [SerializeField] private Button boosterButton;

        public Button BoosterButton => boosterButton;
        public void SetCustomersText(string customersString)
        {
            customersText.text = customersString;
        }

        public void SetTimerText(string timeString)
        {
            timer.text = timeString;
        }
    }
}