using Assets.Scripts.Mechanics;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityToolkit;

namespace Assets.Scripts.MapData
{
    public partial class MapData : ScriptableObject
    {
        [Serializable]
        public class ItemData
        {
            public EMechanismType MechanismType;
            public Vector3 LocalPosition;
        }

        void SaveData_Items(Transform root, ref List<ItemData> dataList)
        {
            dataList ??= new();
            dataList.Clear();
            foreach (Transform child in root)
            {
                var itemData = new ItemData
                {
                    MechanismType = child.GetComponent<MechanismMono_Base>().MechanismType,
                    LocalPosition = child.localPosition
                };
                dataList.Add(itemData);
            }
        }

        void Load_Items(Transform root, List<ItemData> dataList)
        {
            for (int i = root.childCount - 1; i >= 0; i--)
            {
                var child = root.GetChild(i);
                if (Application.isPlaying) Destroy(child.gameObject);
                else DestroyImmediate(child.gameObject);
            }
            foreach (var itemData in dataList)
            {
                var newObj = itemData.MechanismType switch
                {
                    EMechanismType.Token => InstantiateGoj(curGameCtrl.ItemPrefab_Token, root),
                    EMechanismType.Destination => InstantiateGoj(curGameCtrl.ItemPrefab_Destination, root),
                    _ => throw new ArgumentOutOfRangeException()
                };
                newObj.transform.localPosition = itemData.LocalPosition;
            }
        }
    }
}
