using UnityEngine;

namespace SurviveProject
{
    public class BuildMenuView : MonoBehaviour
    {
        [SerializeField] private Transform buildButtonsHolder;
        
        public Transform BuildButtonsHolder => buildButtonsHolder;
    }
}