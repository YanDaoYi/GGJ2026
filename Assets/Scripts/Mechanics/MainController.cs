using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityToolkit;

namespace Platformer.Mechanics
{
    public class MainController : MonoSingleton<MainController>
    {
        [SerializeField]
        List<string> levelNames;
        int curLevel = 0;

        protected override bool DontDestroyOnLoad() => true;

        protected override void OnInit()
        {
            EnterLevel(0);
        }

        async Task EnterLevel(int idx)
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
        }

        void OnExitLevel()
        {
            if (GameController.Instance)
            {
                GameController.Instance.OnLevelPassed -= OnPassLevel;
            }
        }

        void OnPassLevel()
        {
            EnterLevel(curLevel + 1);
        }
    }
}