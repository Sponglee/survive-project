using System;

public class TileInputService
{
    public event Action<WorldTile> OnTileClicked;
    private WorldTile _currentSelectedTile = null;
    
    public void NotifyTileClicked(WorldTile tile)
    {
        var tileToSelect = tile;
        if (_currentSelectedTile == tile)
        {
            _currentSelectedTile = null;
            tileToSelect = null;
        }
        _currentSelectedTile = tileToSelect;
        OnTileClicked?.Invoke(tileToSelect);
    }
}