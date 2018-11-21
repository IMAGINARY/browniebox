using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PathTracer : MonoBehaviour {

    public int MaxPositions = 400;
    public PeriodicDisplacer displacer;
    public Transform tracedObject;

    LineRenderer lineRenderer;
    public bool Tracing { get; private set; }

    Vector3[] renderPositions;
    int positionCount = 0;
    float currentScale;

    public float MaxDisplacement { get; internal set; }

    private void Start() {
        lineRenderer.numCornerVertices = 2;
        renderPositions = new Vector3[MaxPositions];
    }

    public void StartTracing() {
        Tracing = true;
    }

    public void StopTracing() {
        Tracing = false;
        lineRenderer.positionCount = 1;
    }

    void OnEnable() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 1;
        UpdateCurrentPosition();
        displacer.PositionReached += AddNewPosition;
    }

    void OnDisable() {
        displacer.PositionReached -= AddNewPosition;
    }

    void Update() {
        UpdateCurrentPosition();
    }

    void AddNewPosition(Vector3 position) {
        if (!Tracing)
            return;

        AddPosition(tracedObject.localPosition);
    }

    void AddPosition(Vector3 position) {
        position = tracedObject.localPosition;

        if (positionCount < MaxPositions)
            positionCount += 4;
        else
            ShiftPositions();

        renderPositions[positionCount - 1] = UnityEngine.Random.onUnitSphere;
        renderPositions[positionCount - 2] = UnityEngine.Random.onUnitSphere;
        renderPositions[positionCount - 3] = UnityEngine.Random.onUnitSphere;
        renderPositions[positionCount - 4] = position;

        UpdateRenderPositions();
    }

    void ShiftPositions() {
        Array.Copy(renderPositions, 4, renderPositions, 0, positionCount - 4);
    }

    internal void SetScale(float currentScale) {
        this.currentScale = currentScale;
        Debug.LogFormat("new scale: {0}", currentScale);
    }

    Vector3[] GetRenderPositions() {
        var positions = new Vector3[positionCount];
        Array.Copy(renderPositions, positions, positionCount);
        for (int i = 0 ; i < positionCount - 4 ; i += 4) {
            var quarterPos = Vector3.Lerp(positions[i], positions[i + 4], 0.25f);
            var midPos = Vector3.Lerp(positions[i], positions[i + 4], 0.5f);
            var threeQuarterPos = Vector3.Lerp(positions[i], positions[i + 4], 0.75f);

            var scale = currentScale * (MaxDisplacement / 4);

            positions[i + 1] = quarterPos + positions[i + 1] * scale;
            positions[i + 2] = midPos + positions[i + 2] * scale;
            positions[i + 3] = threeQuarterPos + positions[i + 3] * scale;
        }
        positions[positionCount - 1] = positions[positionCount - 4];
        positions[positionCount - 2] = positions[positionCount - 4];
        positions[positionCount - 3] = positions[positionCount - 4];
        return positions;
    }

    void UpdateRenderPositions() {
        lineRenderer.positionCount = positionCount;
        lineRenderer.SetPositions(GetRenderPositions());
    }

    void UpdateCurrentPosition() {
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, tracedObject.localPosition);
    }
}

