using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class UIBuyBoostersView : MonoBehaviour
    {
        [SerializeField] private Button buyBooster;
        [SerializeField] private Button refuse;
        
        public Button BuyBoosterButton => buyBooster;
        public Button RefuseButton => refuse;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
    
}