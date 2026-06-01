
using NgocDev.Core.Addressable;
using NgocDev.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NgocDev.Gameplay.MapGeneration
{
    [Serializable]
    public class TilePrefab
    {

        public string prefab;
#if UNITY_EDITOR
        public string editorPath;
#endif
        public TileRule tileRule;
        
    }

 
    public enum NeighborDirection
    {
        North,
        East,
        South,
        West,
    }

    [Serializable]
    public class NeighborTiles
    {
        public NeighborDirection direction;
        [SerializeReference]
        public List<TilePrefab> tiles;
    }

    [Serializable]
    public class TileRule
    { 
        public NeighborTiles[] neighborTiles = new NeighborTiles[4];
    }

    [CreateAssetMenu(fileName = "TileMap", menuName = "NgocDev/Map/TileMap")]
    public class TileMap : ScriptableObject
    {
       
        public List<TilePrefab> tilePrefabs = new List<TilePrefab>();

    }

    public class Tile
    {
        public int x;
        public int y;
        public Tile(int x, int y)
        {
            this.x = x;
            this.y = y;
        }    

    }

}