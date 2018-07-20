using UnityEngine;

public delegate void PositionEvent(Vector3 position);

public class PeriodicDisplacer : MonoBehaviour {

    public event PositionEvent PositionReached;

    public Vector3 CurrentPosition { get { return transform.position;  } }

    public float Period = 0.1f;
    public float MaxDisplacement = 0.2f;

    float timeToNext = 0;
    bool paused = false;

    Vector3 nextPosition = Vector3.zero;
    Vector3 lastPosition = Vector3.zero;

    void Awake() {
        timeToNext = Period;
        lastPosition = transform.localPosition;
        nextPosition = CreateDisplacement(MaxDisplacement);
    }

    Vector3 CreateDisplacement(float max) {
        return new Vector3(Random.Range(-max, max), Random.Range(-max, max), Random.Range(-max, max));
    }

    void CreateNewPosition() {
        if (PositionReached != null)
            PositionReached(transform.position);

        lastPosition = transform.localPosition;
        nextPosition += CreateDisplacement(MaxDisplacement);
    }

    public void Unpause() {
        paused = false;
    }

    public void Pause() {
        paused = true;
    }

    bool ShouldCreateNewPosition(float elapsed) {
        timeToNext -= elapsed;
        if (timeToNext <= 0) {
            timeToNext += Period;
            return true;
        }
        return false;
    }

    public void Update() {
        if (paused)
            return;

        if (ShouldCreateNewPosition(Time.deltaTime))
            CreateNewPosition();

        transform.localPosition = Vector3.Lerp(lastPosition, nextPosition, (Period - timeToNext) / Period);
    }
}

