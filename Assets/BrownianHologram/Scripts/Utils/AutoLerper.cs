using System;
using UnityEngine;

class AutoLerper {
    AnimationCurve scaleCurve = null;
    Action<float> onChange;
    float timeLeft = 0;

    public void LerpTo(float from, float to, float time, Action<float> onChange) {
        timeLeft = time;
        scaleCurve = AnimationCurve.EaseInOut(0, to, time, from);
        this.onChange = onChange;
    }

    public void Update(float elapsed) {
        if (timeLeft <= 0)
            return;

        timeLeft -= elapsed;
        onChange(scaleCurve.Evaluate(timeLeft));
    }
}
