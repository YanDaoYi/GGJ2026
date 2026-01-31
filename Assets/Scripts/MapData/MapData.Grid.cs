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
        public class TileCell
        {
            public Vector3Int position;
            public TileBase tile;
        }

        [Serializable]
        public class TileLayer
        {
            public string name;
            public List<TileCell> cells = new List<TileCell>();
        }

        [Serializable]
        public class GridData
        {
            public SerializableDictionary<string, List<TileCell>> LayersData = new();
            public SerializableDictionary<string, Sprite> SpriteData = new();
        }

        void SaveData_Grid(Grid grid, ref GridData gridData)
        {
            gridData ??= new();
            gridData.LayersData.Clear();
            gridData.SpriteData.Clear();
            foreach (Transform child in grid.transform)
            {
                if (child.TryGetComponent<Tilemap>(out var tilemap))
                {
                    var layerData = new List<TileCell>();
                    foreach (var position in tilemap.cellBounds.allPositionsWithin)
                    {
                        var tile = tilemap.GetTile(position);
                        if (tile != null)
                        {
                            layerData.Add(new TileCell
                            {
                                position = position,
                                tile = tile
                            });
                        }
                    }
                    gridData.LayersData[tilemap.gameObject.name] = layerData;
                }
                else if (child.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
                {
                    gridData.SpriteData[child.gameObject.name] = spriteRenderer.sprite;
                }
            }
        }

        void Load_Grid(Grid grid, GridData gridData)
        {
            foreach (var layer in gridData.LayersData)
            {
                var tilemap = grid.transform.Find(layer.Key)?.GetComponent<Tilemap>();
                if (tilemap != null)
                {
                    tilemap.ClearAllTiles();
                    foreach (var cell in layer.Value)
                    {
                        tilemap.SetTile(cell.position, cell.tile);
                    }
                }
            }

            foreach (var sprite in gridData.SpriteData)
            {
                var spriteRenderer = grid.transform.Find(sprite.Key)?.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sprite = sprite.Value;
                }
            }
        }
    }
}
