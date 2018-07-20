using UnityEngine;

public class Scaler : MonoBehaviour {

    AnimationCurve scaleCurve = null;
    float scaleTimeLeft;

    void Awake() {
        scaleTimeLeft = 0;
    }

    public void ScaleTo(float newScale, float time) {
        if (time < 0 || newScale < 0)
            throw new System.Exception("Time and scale should be positive numbers. " + time + "," + newScale);
        scaleCurve = AnimationCurve.EaseInOut(0, newScale, time, transform.localScale.x);
        scaleTimeLeft = time;
    }

    public void Update() {
        if (scaleTimeLeft == 0)
            return;

        scaleTimeLeft = Mathf.Clamp(scaleTimeLeft - Time.deltaTime, 0, float.MaxValue);
        transform.localScale = Vector3.one * scaleCurve.Evaluate(scaleTimeLeft);
    }
}

