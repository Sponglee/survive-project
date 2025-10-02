using System;
using DefaultNamespace;
using UnityEngine;
using Zenject;

public class WorldTile : MonoBehaviour, ITile, IDisposable
{
    public Transform Transform => transform;

    [SerializeField] private OutlineIndicatorView _outlineIndicatorView;
    
    private TileManager _tileManager;
    private TileInputService _inputService;

    private TileOutlineService _tileOutlineService;
    // private TileData _data;
    // private AudioManager _audioManager; // Example dependency

    [Inject]
    public void Construct(TileManager tileManager, TileInputService tileInputService)
    {
        _tileManager = tileManager;
        _inputService = tileInputService;
        // _tileOutlineService = tileOutlineService;

        _inputService.OnTileClicked += _outlineIndicatorView.ToggleOutline;
        // tileOutlineService.RegisterOutline(this, _outlineIndicatorView);
        // _data = data;
        // _audioManager = audioManager;
    }

    public void Dispose()
    {
        _inputService.OnTileClicked -= _outlineIndicatorView.ToggleOutline;
    }
    public void Initialize(Vector3 position)
    {
        transform.position = position;
        // Setup tile based on _data
    }
    
    private void OnMouseDown()
    {
        _inputService.NotifyTileClicked(this);
    }
    
}