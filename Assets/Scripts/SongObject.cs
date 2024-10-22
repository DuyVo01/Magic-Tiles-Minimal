using System.Net.Http.Headers;
using UnityEngine;

[CreateAssetMenu(fileName = "Song", menuName = "New Song")]
public class SongObject : ScriptableObject
{
    public float[] noteTimeLapse; 
    public int totalNumberOfNode;
    
}
