using UnityEngine;

public class HammerLineConnector : MonoBehaviour
{
    [SerializeField] private LineRenderer[] _lineRenderers;
    [SerializeField] private Transform[] _startPoints;
    [SerializeField] private Transform[] _hammerPoints;

    private void Update()
    {
        if (_lineRenderers == null || _startPoints == null || _hammerPoints == null)
            return;

        for (var i = 0; i < _lineRenderers.Length && i < _startPoints.Length && i < _hammerPoints.Length; i++)
        {
            if (_lineRenderers[i] == null || _startPoints[i] == null || _hammerPoints[i] == null)
            {
                continue;
            }

            _lineRenderers[i].positionCount = 2;
            _lineRenderers[i].SetPosition(0, _startPoints[i].position);
            _lineRenderers[i].SetPosition(1, _hammerPoints[i].position);
        }
    }
}