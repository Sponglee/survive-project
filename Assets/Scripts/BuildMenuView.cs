using System;
using UnityEngine;
using UnityEngine.UI;

namespace SurviveProject
{
    public class BuildMenuView : MonoBehaviour
    {
        [SerializeField] private Button _buildButton;

        public Button BuildButton => _buildButton;
    }
}