using UnityEngine;

namespace DefaultNamespace
{
    public interface ITileFactory
    {
        ITile Create(GameObject prefab, Vector3 position);
    }
}