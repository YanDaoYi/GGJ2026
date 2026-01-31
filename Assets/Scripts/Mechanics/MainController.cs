using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityToolkit;

namespace Platformer.Mechanics
{
    public class MainController : MonoSingleton<MainController>
    {
        [SerializeField]
        List<string> levelNames;
        int curLevel = 0;

        private InputAction m_ResetAction;

        protected override bool DontDestroyOnLoad() => true;

        protected override void OnInit()
        {
            EnterLevel(0);

            m_ResetAction = InputSystem.actions.FindAction("Player/Reset");
            m_ResetAction.Enable();
        }

        private void Update()
        {
            if (m_ResetAction.WasPressedThisFrame())
            {
                OnResetLevel();
            }
        }

        async void EnterLevel(int idx)
        {
            curLevel = idx;
            if (curLevel < levelNames.Count)
            {
                SceneManager.LoadScene(levelNames[curLevel], LoadSceneMode.Single);
                await Task.Delay(100); //wait a frame for scene to load
                OnExitLevel();
                OnEnterLevel();
            }
            else
            {
                //todo show game end UI
            }
        }

        void OnEnterLevel()
        {
            GameController.Instance.OnLevelPassed += OnPassLevel;
            GameController.Instance.OnLevelFailed += OnFailLevel;
        }

        void OnExitLevel()
        {
            if (GameController.Instance)
            {
                GameController.Instance.OnLevelPassed -= OnPassLevel;
                GameController.Instance.OnLevelFailed -= OnFailLevel;
            }
        }

        void OnPassLevel()
        {
            EnterLevel(curLevel + 1);
        }

        void OnFailLevel()
        {
            EnterLevel(curLevel);
        }

        void OnResetLevel()
        {
            EnterLevel(curLevel);
        }
    }
}