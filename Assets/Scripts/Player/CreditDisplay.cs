using UnityEngine;

namespace GGJ.Player
{
    public class CreditDisplay : MonoBehaviour
    {
        [SerializeField]
        private GameObject _creditDisplay;

        private int _enterCount = 0;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _enterCount++;
            if (_enterCount > 0) _creditDisplay.SetActive(true);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _enterCount--;
            if (_enterCount == 0) _creditDisplay.SetActive(false);
        }
    }
}