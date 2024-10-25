using System;
using System.Net.Http.Headers;
using UnityEngine;

[CreateAssetMenu(fileName = "Song", menuName = "New Song")]
public class SongObject : ScriptableObject
{
    public TileStruct[] tiles;
    public float[] noteTimeLapse;
    public int totalNumberOfNode;
}

[Serializable]
public struct TileStruct
{
    public float timelapse;
    public TileType tileType;
    public int tileCount ;
}

[Serializable]
public enum TileType
{
    shortTile,
    longTile,
}
