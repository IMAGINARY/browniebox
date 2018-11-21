using UnityEngine;

public delegate void PositionEvent(Vector3 position);

public class PeriodicDisplacer : MonoBehaviour {

    public event PositionEvent PositionReached;

    public Vector3 CurrentPosition { get { return transform.position;  } }

    public float Period = 0.025f;
    public float MaxDisplacement = 0.01f;

    private float scale = 0f;
    public float Scale {
        get { return scale; }
        set { scale = Mathf.Clamp01(value); }
    }

    public float ScaledDisplacement { get { return MaxDisplacement * (1 + Scale); } }

    float timeToNext = 0;
    bool paused = false;

    Vector3 nextPosition = Vector3.zero;
    Vector3 lastPosition = Vector3.zero;

    void Awake() {
        timeToNext = Period;
        CalculateNextPosition();
    }

    private void CalculateNextPosition() {
        lastPosition = transform.position;
        nextPosition = lastPosition + CreateDisplacement(ScaledDisplacement);
    }

    Vector3 CreateDisplacement(float max) {
        return new Vector3(Random.Range(-max, max), Random.Range(-max, max), Random.Range(-max, max));
    }

    void CreateNewPosition() {
        if (PositionReached != null)
            PositionReached(transform.position);

        CalculateNextPosition();
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

        transform.position = Vector3.Lerp(lastPosition, nextPosition, (Period - timeToNext) / Period);
    }
}

