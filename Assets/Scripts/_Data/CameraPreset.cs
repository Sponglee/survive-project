using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraPreset", menuName = "Scriptable Objects/CameraPreset")]
public class CameraPreset : ScriptableObject
{
    public float CameraMoveSpeed = .25f;
    public float CameraPanSpeed = .15f;
    public float CameraRotateSpeed = 1f;
    public float CameraZoomSpeed = .05f;

    public Ease CameraLookAtEase = Ease.OutCubic;
    public float CameraLookAtDuration = 1f;
}

