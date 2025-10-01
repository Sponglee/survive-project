using UnityEngine;
using Zenject;

public class WorldTile : MonoBehaviour, ITile
{
    public Transform Transform => transform;

    // private TileData _data;
    // private AudioManager _audioManager; // Example dependency
    
    [Inject]
    public void Construct()
    {
        // _data = data;
        // _audioManager = audioManager;
    }
    
    public void Initialize(Vector3 position)
    {
        transform.position = position;
        // Setup tile based on _data
    }
    
}