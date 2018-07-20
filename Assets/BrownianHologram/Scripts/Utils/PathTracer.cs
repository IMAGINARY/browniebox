using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PathTracer : MonoBehaviour {

    public int MaxPositions = 200;
    public PeriodicDisplacer displacer;
    public Transform tracedObject;

    LineRenderer lineRenderer;
    public bool Tracing { get; private set; }

    private void Start() {
        lineRenderer.numCornerVertices = 2;
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

        position = tracedObject.localPosition;
        if (lineRenderer.positionCount < MaxPositions)
            lineRenderer.positionCount++;
        else
            ShiftPositions();

        lineRenderer.SetPosition(lineRenderer.positionCount - 1, position);
    }

    void ShiftPositions() {
        for (int i = 0; i < lineRenderer.positionCount - 1 ; i++)
            lineRenderer.SetPosition(i, lineRenderer.GetPosition(i + 1));
    }

    void UpdateCurrentPosition() {
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, tracedObject.localPosition);
    }
}

