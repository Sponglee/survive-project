using System;
using DefaultNamespace;
using UnityEngine;
using Zenject;

public class WorldTile : MonoBehaviour, IInitializable, ITile, IDisposable
{
    public Transform Transform => transform;

    [SerializeField] private OutlineIndicatorView _outlineIndicatorView;
    
    private TileManager _tileManager;
    private TileInputService _inputService;

    private TileOutlineService _tileOutlineService;
    // private TileData _data;

    [Inject]
    public void Construct(TileManager tileManager, TileInputService tileInputService)
    {
        _tileManager = tileManager;
        _inputService = tileInputService;
        _inputService.OnTileClicked += TileClickedHandler;

        // _data = data;
    }

    public void Initialize()
    {
        // Setup tile based on _data
    }
    
    public void Dispose()
    {
        _inputService.OnTileClicked -= TileClickedHandler;
    }

    private void TileClickedHandler(WorldTile obj)
    {
        _outlineIndicatorView.ToggleOutline(obj != null && this == obj);
    }

}