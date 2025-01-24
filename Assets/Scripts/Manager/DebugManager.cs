using UnityEngine;

namespace GGJ.Manager
{
    public class DebugManager : MonoBehaviour
    {
#if UNITY_EDITOR
        public void OnGUI()
        {
            if (PlayerManager.Instance != null)
            {
                GUI.Label(new Rect(10f, 10f, 500f, 50f), string.Join("\n", PlayerManager.Instance.PlayerStrings));
            }
        }
#endif
    }
}