using TMPro;
using UnityEngine;

namespace GGJ.Player
{
    public class PlayerScore : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _text;

        public void SetScore(int score)
        {
            _text.text = $"{score}";
        }
    }
}