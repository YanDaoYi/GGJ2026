using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityToolkit;

namespace Assets.Scripts.Mechanics
{
    public class SwitchCtrl : MonoSingleton<SwitchCtrl>
    {
        [SerializeField]
        Grid world_Inner;
        [SerializeField]
        Grid world_Outer;
        [SerializeField]
        Grid world_True;
        [SerializeField]
        Transform InnerItemsRoot;
        [SerializeField]
        Transform OuterItemsRoot;
        [SerializeField]
        Vector2Int minGridPos;
        [SerializeField]
        Vector2Int maxGridPos;
        [SerializeField]
        Vector2Int gridPadding;

        bool isOn = false;
        bool isOn_last = false;
        [ShowInInspector]
        public bool IsOn => isOn;

        private InputAction m_SwitchAction;

        HashSet<MaskMono> maskMonoSet = new();
        List<Tilemap> worldInnerTilemaps = new();
        List<Tilemap> worldOuterTilemaps = new();
        List<Tilemap> worldTrueTilemaps = new();
        Dictionary<Vector2, List<ItemInfo>> ItemInfoDic = new();

        int inMaskFullyCount = 0;
        bool inOuter = true;//当前在表世界

        protected override void OnInit()
        {
            m_SwitchAction = InputSystem.actions.FindAction("Player/Switch");

            m_SwitchAction.Enable();

            InitMap();
            RebuildTrueWorld();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }

        void InitMap()
        {
            worldInnerTilemaps.Clear();
            worldOuterTilemaps.Clear();
            worldTrueTilemaps.Clear();

            CollectTilemaps(world_Inner, worldInnerTilemaps);
            CollectTilemaps(world_Outer, worldOuterTilemaps);
            CollectTilemaps(world_True, worldTrueTilemaps);

            world_True.gameObject.SetActive(true);

            for (int i = 0; i < worldTrueTilemaps.Count; i++)
            {
                worldInnerTilemaps[i].GetComponent<TilemapRenderer>().enabled = false;
                worldOuterTilemaps[i].GetComponent<TilemapRenderer>().enabled = false;
                worldTrueTilemaps[i].GetComponent<TilemapRenderer>().enabled = true;
            }

            //计算相机内覆盖了网格的哪些坐标
            Vector3 cameraPos = Camera.main.transform.position;
            Vector3Int min = world_Inner.WorldToCell(cameraPos - new Vector3(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize, 0));
            Vector3Int max = world_Inner.WorldToCell(cameraPos + new Vector3(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize, 0));
            minGridPos = new Vector2Int(min.x, min.y) - gridPadding;
            maxGridPos = new Vector2Int(max.x, max.y) + gridPadding;

            //收集所有Items
            ItemInfoDic.Clear();
            CollectItems(InnerItemsRoot, true);
            CollectItems(OuterItemsRoot, false);

            static void CollectTilemaps(Grid grid, List<Tilemap> tilemapSet)
            {
                Tilemap[] tilemaps = grid.GetComponentsInChildren<Tilemap>();
                foreach (var tilemap in tilemaps)
                {
                    tilemapSet.Add(tilemap);
                }
            }

            void CollectItems(Transform root, bool isInner)
            {
                foreach (Transform child in root)
                {
                    //计算所在格子中心点坐标
                    Vector3Int gridPos3D = world_Inner.WorldToCell(child.position);
                    Vector2 gridPos = new Vector2(gridPos3D.x, gridPos3D.y);

                    List<ItemInfo> list = null;
                    if (!ItemInfoDic.TryGetValue(gridPos, out list))
                    {
                        ItemInfoDic[gridPos] = list = new();
                    }

                    list.Add(new ItemInfo
                    {
                        itemTf = child,
                        isInner = isInner
                    });
                }
            }
        }

        private void FixedUpdate()
        {
            isOn = m_SwitchAction.IsPressed();
            if (isOn != isOn_last)
            {
                //Debug.Log($"SwitchCtrl FixedUpdate isOn={isOn}");
                RebuildTrueWorld();
            }

            isOn_last = isOn;
        }

        void RebuildTrueWorld()
        {
            for (int x = minGridPos.x; x <= maxGridPos.x; x++)
                for (int y = minGridPos.y; y <= maxGridPos.y; y++)
                {
                    Vector3Int gridPos3D = new(x, y, 0);
                    Vector2Int gridPos = new(x, y);
                    // world pos
                    Vector3 centerWorldPos = world_Inner.CellToWorld(gridPos3D) + world_Inner.cellSize * 0.5f;
                    bool showInner = IsOn && CheckInMask(centerWorldPos);
                    if (!inOuter) showInner = !showInner;

                    for (int i = 0; i < worldTrueTilemaps.Count; i++)
                    {
                        TileBase tileBase = showInner ? worldInnerTilemaps[i].GetTile(gridPos3D) : worldOuterTilemaps[i].GetTile(gridPos3D);
                        worldTrueTilemaps[i].SetTile(gridPos3D, tileBase);
                    }

                    //处理物体
                    if (ItemInfoDic.TryGetValue(gridPos, out var itemList))
                    {
                        foreach (var itemInfo in itemList)
                        {
                            itemInfo.itemTf.gameObject.SetActive(!(showInner ^ itemInfo.isInner));
                        }
                    }
                }
        }

        bool CheckInMask(Vector3 worldPos)
        {
            bool ret = false;
            foreach (var maskMono in maskMonoSet)
            {
                Vector3 maskPos = maskMono.transform.position;
                Vector3 maskScale = maskMono.transform.lossyScale;
                if (worldPos.x >= maskPos.x - maskScale.x / 2 &&
                    worldPos.x <= maskPos.x + maskScale.x / 2 &&
                    worldPos.y >= maskPos.y - maskScale.y / 2 &&
                    worldPos.y <= maskPos.y + maskScale.y / 2)
                {
                    ret = true;
                    break;
                }
            }
            return ret;
        }

        public void RegisterMask(MaskMono maskMono)
        {
            if (!maskMonoSet.Contains(maskMono))
            {
                maskMono.EventEmitter.TriggerEnter_Fully += OnEnterMask_Fully;
                maskMono.EventEmitter.TriggerExit_Fully += OnExitMask_Fully;
                maskMonoSet.Add(maskMono);
            }
        }

        public void UnregisterMask(MaskMono maskMono)
        {
            if (maskMonoSet.Contains(maskMono))
            {
                maskMono.EventEmitter.TriggerEnter_Fully -= OnEnterMask_Fully;
                maskMono.EventEmitter.TriggerExit_Fully -= OnExitMask_Fully;
                maskMonoSet.Remove(maskMono);
            }
        }

        void OnEnterMask_Fully(Collider2D collider)
        {
            Debug.Log("OnEnterMask");
            inMaskFullyCount++;
        }

        void OnExitMask_Fully(Collider2D collider)
        {
            Debug.Log("OnExitMask");
            inMaskFullyCount--;

            //离开最后一个遮罩时
            if (inMaskFullyCount == 0)
            {
                if (isOn)
                {
                    inOuter = !inOuter;
                    RebuildTrueWorld();
                }
            }
        }
    }

    struct ItemInfo
    {
        public Transform itemTf;
        public bool isInner;
    }
}
