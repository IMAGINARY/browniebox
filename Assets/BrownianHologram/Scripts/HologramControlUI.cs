using UnityEngine;
using UnityEngine.UI;

public class HologramControlUI : MonoBehaviour {

    public HologramControl hologramControl;
    public Slider speed;
    public Slider scale;
    public Toggle particleToggle;
    public Toggle traceToggle;
    public Toggle airParticlesToggle;

    public void OnEnable() {
        speed.onValueChanged.AddListener(OnSpeedChanged);
        speed.maxValue = HologramControl.MaxSpeed;
        speed.minValue = HologramControl.MinSpeed;
        scale.onValueChanged.AddListener(OnScaleChanged);
        scale.maxValue = HologramControl.MaxScale;
        scale.minValue = HologramControl.MinScale;

        particleToggle.onValueChanged.AddListener(OnParticleChanged);
        traceToggle.onValueChanged.AddListener(OnTraceChanged);
        airParticlesToggle.onValueChanged.AddListener(OnAirParticlesChanged);
    }

    private void OnDisable() {
        speed.onValueChanged.RemoveListener(OnSpeedChanged);
        scale.onValueChanged.RemoveListener(OnScaleChanged);
    }

    public void ResetScale() {
        scale.value = HologramControl.DefaultScale;
    }

    public void ResetSpeed() {
        speed.value = HologramControl.DefaultSpeed;
    }

    private void OnScaleChanged(float newValue) {
        hologramControl.SetScale(newValue);
    }

    private void OnSpeedChanged(float newValue) {
        hologramControl.SetSpeed(newValue);
    }

    private void OnParticleChanged(bool visible) {
        hologramControl.SetParticleVisible(visible);
    }

    private void OnTraceChanged(bool visible) {
        if (visible)
            hologramControl.StartTracing();
        else
            hologramControl.StopTracing();
    }

    private void OnAirParticlesChanged(bool visible) {
        if (visible)
            hologramControl.ShowAirParticles();
        else
            hologramControl.HideAirParticles();
    }
}
