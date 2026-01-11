using UnityEngine;
using Random = UnityEngine.Random;

public class RoadView : BuildingView
{
    [SerializeField] private GameObject[] _roadVariants;


    private void Awake()
    {

    }

    public void EnableRoadVariantById(int index)
    {
      for (var i = 0; i < _roadVariants.Length; i++)
      {
          if (_roadVariants[i] != null)
          {
              _roadVariants[i].SetActive(i == index);
          }
      }
    }
}
