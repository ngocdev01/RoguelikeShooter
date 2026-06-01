using System.Collections.Generic;
using UnityEngine;

namespace NgocDev.Gameplay.MapGeneration
{

    public struct TileData
    {
        public TilePrefab prefab;
        public int rotation;
        public int scale;
    }

    [System.Serializable]
    public class PrefabDictionary : SerializableDictionary<Vector3Int, TileData> { }

    [System.Serializable]
    public class SerializableDictionary<T1, T2> : ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<T1> keys = new List<T1>();
        [SerializeField]
        private List<T2> values = new List<T2>();
        public Dictionary<T1, T2> dictionary = new Dictionary<T1, T2>();
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (var kvp in dictionary)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }
        public void OnAfterDeserialize()
        {
            dictionary = new Dictionary<T1, T2>();
            for (int i = 0; i < Mathf.Min(keys.Count, values.Count); i++)
            {
                dictionary[keys[i]] = values[i];
            }
        }
    }



    [CreateAssetMenu(fileName = "Map", menuName = "NgocDev/Map/Map")]
    public class Map : ScriptableObject
    {
        public TileMap tileMap;
        public Grid grid;
        public PrefabDictionary tiles = new PrefabDictionary();
        public void SetTilePrefabAt(Vector3Int position, TileData tileData)
        {
            tiles.dictionary[position] = tileData;
        }


    }

    [System.Serializable]
    public class Room
    {
        public Vector3Int position;
        public Vector3Int size;
        
        public Grid grid;

      
       

    }

    [System.Serializable]
    public class Grid
    {
        public float cellSize;
        public int gridSize;
        public Vector3 offset;
        public Vector3 origin;

        public static Vector3 GridCenterToWorld(Grid grid, Vector3Int gridPosition)
        {
            var origin = grid.origin + grid.offset;
            return origin + ((Vector3)gridPosition + Vector3.one * 0.5f) * grid.cellSize;

        }

        public static Vector3 GridCornerToWorld(Grid grid, Vector3Int gridPosition)
        {
            var origin = grid.origin + grid.offset;
            return origin + (Vector3)gridPosition * grid.cellSize;

        }

        public static Vector3Int WorldToGrid(Grid grid, Vector3 worldPosition)
        {
            var origin = grid.origin + grid.offset;
            Vector3 localPos = (worldPosition - origin) / grid.cellSize;
            return new Vector3Int(Mathf.FloorToInt(localPos.x), Mathf.FloorToInt(localPos.y), Mathf.FloorToInt(localPos.z));
        }

        public static Vector3 SnapToGridCenter(Grid grid, Vector3 worldPosition)
        {
            return GridCenterToWorld(grid, WorldToGrid(grid, worldPosition));
        }

        public static Vector3 SnapToGridCorner(Grid grid, Vector3 worldPosition)
        {
            return GridCornerToWorld(grid, WorldToGrid(grid, worldPosition));
        }

        public static bool IsInGridWorldBounds(Grid grid, Vector3 worldPosition)
        {
            Vector3Int gridPosition = WorldToGrid(grid, worldPosition);
            return IsInGridBounds(grid, gridPosition);
        }
        public static bool IsInGridBounds(Grid grid, Vector3Int gridPosition)
        {
            return Mathf.Abs(gridPosition.x) < grid.gridSize &&
                   Mathf.Abs(gridPosition.y) < grid.gridSize &&
                   Mathf.Abs(gridPosition.z) < grid.gridSize;
        }
    }




}

