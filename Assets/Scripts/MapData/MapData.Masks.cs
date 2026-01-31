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
        public class MaskData
        {
            public Vector3 LocalPosition;
            public Vector3 LocalScale;
        }

        void SaveData_Masks(Transform root, ref List<MaskData> dataList)
        {
            dataList.Clear();
            foreach (Transform child in root)
            {
                dataList.Add(new MaskData
                {
                    LocalPosition = child.localPosition,
                    LocalScale = child.localScale
                });
            }
        }

        void Load_Masks(Transform root, List<MaskData> dataList)
        {
            for (int i = root.childCount - 1; i >= 0; i--)
            {
                var child = root.GetChild(i);
                if (Application.isPlaying) Destroy(child.gameObject);
                else DestroyImmediate(child.gameObject);
            }
            foreach (var maskData in dataList)
            {
                var newObj = InstantiateGoj(curGameCtrl.MaskPrefab, root);
                newObj.transform.localPosition = maskData.LocalPosition;
                newObj.transform.localScale = maskData.LocalScale;
            }
        }
    }
}
