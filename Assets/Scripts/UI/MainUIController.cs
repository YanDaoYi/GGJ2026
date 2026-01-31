using Platformer.Mechanics;
using UnityEngine;
using UnityToolkit;

namespace Platformer.UI
{
    /// <summary>
    /// A simple controller for switching between UI panels.
    /// </summary>
    public class MainUIController : MonoSingleton<MainUIController>
    {
        protected override bool DontDestroyOnLoad() => true;

        public GameObject[] panels;

        public void SetRtfActiveOn(RectTransform rtf)
        {
            rtf.gameObject.SetActive(true);
        }

        public void SetRtfActiveOff(RectTransform rtf)
        {
            rtf.gameObject.SetActive(false);
        }

        public void StartGame()
        {
            MainController.Singleton.EnterLevel(0);
        }
    }
}