using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettingsPreset", menuName = "Scriptable Objects/CameraSettingsPreset")]
public class CameraSettingsPreset : ScriptableObject
{
    public float CameraMoveSpeed = .25f;
    public float CameraPanSpeed = .15f;
    public float CameraRotateSpeed = 1f;
    public float CameraZoomSpeed = .05f;

    public Ease CameraLookAtEase = Ease.OutCubic;
    public float CameraLookAtDuration = 1f;
}

