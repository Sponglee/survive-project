using System.Collections.Generic;

public class TileManager
{
    private List<ITile> _activeTiles = new List<ITile>();

    public TileManager()
    {
        
    }
    
    public void AddTile(ITile tile)
    {
        _activeTiles.Add(tile);
    }
    
}