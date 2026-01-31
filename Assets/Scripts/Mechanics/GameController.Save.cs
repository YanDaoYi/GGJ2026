using Assets.Scripts.MapData;
using Platformer.Core;
using Platformer.Model;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This class exposes the the game model in the inspector, and ticks the
    /// simulation.
    /// </summary> 
    public partial class GameController
    {
        [SerializeField]
        MapData mapData;

#if UNITY_EDITOR
        [Button(ButtonSizes.Large)]
        void SaveLevel()
        {
            //没有地图数据文件就根据场景名自动创建一个
            if (mapData == null)
            {
                string sceneName = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name;
                mapData = MapData.CreateInstance<MapData>();
                string assetPath = $"Assets/Resources/MapData/{sceneName}.asset";
                //确保目录存在
                string directoryPath = System.IO.Path.GetDirectoryName(assetPath);
                if (!System.IO.Directory.Exists(directoryPath))
                {
                    System.IO.Directory.CreateDirectory(directoryPath);
                }
                UnityEditor.AssetDatabase.CreateAsset(mapData, assetPath);
                UnityEditor.AssetDatabase.SaveAssets();
                Debug.Log($"自动创建地图数据文件：{assetPath}");
            }

            mapData.ScanLevel(this);
        }
#endif
    }
}