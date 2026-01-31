using Assets.Scripts.Mechanics;
using Platformer.Mechanics;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MapData
{
    public partial class MapData : ScriptableObject
    {
        [SerializeField]
        private GridData gridData_Inner;
        public GridData GridData_Inner => gridData_Inner;
        [SerializeField]
        private GridData gridData_Outer;
        public GridData GridData_Outer => gridData_Outer;
        [SerializeField]
        public List<MaskData> masksData;
        public List<MaskData> MasksData => masksData;
        [SerializeField]
        public List<ItemData> itemDatas_Inner;
        public List<ItemData> ItemDatas_Inner => itemDatas_Inner;
        [SerializeField]
        public List<ItemData> itemDatas_Outer;
        public List<ItemData> ItemDatas_Outer => itemDatas_Outer;

        GameController curGameCtrl;

        public void ScanLevel(GameController gameController)
        {
            curGameCtrl = gameController;
            SwitchCtrl switchCtrl = gameController.GetComponent<SwitchCtrl>();
            SaveData_Grid(switchCtrl.World_Inner, ref gridData_Inner);
            SaveData_Grid(switchCtrl.World_Outer, ref gridData_Outer);
            SaveData_Masks(gameController.MasksRoot, ref masksData);
            SaveData_Items(gameController.InnerItemsRoot, ref itemDatas_Inner);
            SaveData_Items(gameController.OuterItemsRoot, ref itemDatas_Outer);
        }

        public void LoadLevel(GameController gameController)
        {
            curGameCtrl = gameController;
            Load_Grid(curGameCtrl.SwitchCtrl.World_Inner, gridData_Inner);
            Load_Grid(curGameCtrl.SwitchCtrl.World_Outer, gridData_Outer);
            Load_Masks(curGameCtrl.MasksRoot, masksData);
            Load_Items(curGameCtrl.InnerItemsRoot, itemDatas_Inner);
            Load_Items(curGameCtrl.OuterItemsRoot, itemDatas_Outer);
        }

        GameObject InstantiateGoj(GameObject original, Transform parent)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return UnityEditor.PrefabUtility.InstantiatePrefab(original, parent) as GameObject;
            }
#endif
            return Instantiate(original, parent);
        }
    }
}
