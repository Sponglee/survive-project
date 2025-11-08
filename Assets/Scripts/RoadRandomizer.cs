using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadRandomizer : MonoBehaviour
{
  [SerializeField] private GameObject[] _roadVariants;


  private void Awake()
  {
    if (_roadVariants == null || _roadVariants.Length == 0)
    {
        return;
    }
    
    var randomIndex = Random.Range(0, _roadVariants.Length);
    
    for (var i = 0; i < _roadVariants.Length; i++)
    {
      if (_roadVariants[i] != null)
      {
        _roadVariants[i].SetActive(i == randomIndex);
      }
    }
  }
}
