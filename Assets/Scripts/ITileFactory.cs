using UnityEngine;

namespace SurviveProject
{
    public interface ITileFactory
    {
        WorldTileView Create(GameObject prefab);
    }
}