using UnityEngine;

public class HologramControl : MonoBehaviour {
    public GameObject scaleBox;
    public GameObject particle;
    public PeriodicDisplacer displacer;
    public PathTracer pathTracer;

    public ParticleSystem airParticles;
    public AudioSource airParticlesSound;

    public const float DefaultScale = 1f;
    public const float MinScale = 1f;
    public const float MaxScale = 2f;

    public const float DefaultPeriod = 0.1f;
    public const float DefaultSpeed = 1f;
    public const float MinSpeed = 0f;
    public const float MaxSpeed = 8f;

    public const float DefaultPitch = 1;
    public const float MinPitch = 0;
    public const float MaxPitch = 3;

    AutoLerper scaleLerper = new AutoLerper();
    AutoLerper speedLerper = new AutoLerper();
    float currentSpeed = DefaultSpeed;
    float currentScale = DefaultScale;
    float emissionRadius;

    private void Start() {
        emissionRadius = airParticles.shape.radius;
        displacer.Period = DefaultPeriod;
        pathTracer.MaxDisplacement = displacer.MaxDisplacement;
        pathTracer.SetScale((currentScale - MinScale) / (MaxScale - MinScale));
    }

    public virtual void SetScale(float newScale) {
        scaleLerper.LerpTo(currentScale, newScale, 2f, AdjustScale);
    }

    public virtual void SetSpeed(float newSpeed) {
        speedLerper.LerpTo(currentSpeed, newSpeed, 1f, AdjustSpeed);
    }

    private void Update() {
        scaleLerper.Update(Time.deltaTime);
        speedLerper.Update(Time.deltaTime);
    }

    void AdjustScale(float newScale) {
        currentScale = newScale;
        scaleBox.transform.localScale = Vector3.one * currentScale;
        var shape = airParticles.shape;
        shape.radius = emissionRadius / currentScale;
        var normalizedScale = (currentScale - MinScale) / (MaxScale - MinScale);
        pathTracer.SetScale(normalizedScale);
        displacer.Scale = normalizedScale;
    }

    void AdjustSpeed(float newSpeed) {
        if (newSpeed <= 0)
            PauseAll();
        else if (currentSpeed <= 0)
            ResumeAll();

        currentSpeed = Mathf.Clamp(newSpeed, MinSpeed, MaxSpeed);

        displacer.Period = DefaultPeriod / currentSpeed;

        AdjustAirParticlesSpeed();

    }

    private void AdjustAirParticlesSpeed() {
        var airParticlesMain = airParticles.main;
        airParticlesMain.simulationSpeed = currentSpeed;
        airParticlesSound.pitch = Mathf.Clamp(currentSpeed, MinPitch, MaxPitch);
    }

    private void ResumeAll() {
        displacer.Unpause();
        if (airParticles.isPaused)
            airParticles.Play();
    }

    private void PauseAll() {
        displacer.Pause();
        if (airParticles.isPlaying)
            airParticles.Pause();
    }

    public virtual void SetParticleVisible(bool visible) {
        Debug.LogFormat("SetParticleVisible: {0}", visible);
        particle.GetComponent<MeshRenderer>().enabled = visible;
        if (visible)
            EnableAllSounds(particle.gameObject);
        else
            DisableAllSounds(particle.gameObject);
    }

    public virtual void ShowAirParticles() {
        Debug.LogFormat("ShowAirParticles:");
        airParticles.Play();
        EnableAllSounds(airParticles.gameObject);
    }

    public virtual void HideAirParticles() {
        Debug.LogFormat("HideAirParticles:");
        airParticles.Stop();
        airParticles.Clear();
        DisableAllSounds(airParticles.gameObject);
    }

    public virtual void StartTracing() {
        Debug.LogFormat("StartTracing:");
        pathTracer.StartTracing();
        EnableAllSounds(pathTracer.gameObject);

    }

    public virtual void StopTracing() {
        Debug.LogFormat("StopTracing:");
        pathTracer.StopTracing();
        DisableAllSounds(pathTracer.gameObject);
    }

    void EnableAllSounds(GameObject go) {
        foreach (var source in go.GetComponents<AudioSource>())
            source.enabled = true;
    }

    void DisableAllSounds(GameObject go) {
        foreach (var source in go.GetComponents<AudioSource>())
            source.enabled = false;
    }

    public void ToggleAirParticles() {
        if (airParticles.isPlaying)
            HideAirParticles();
        else
            ShowAirParticles();
    }

    public void ToggleTracing() {
        if (pathTracer.Tracing)
            StopTracing();
        else
            StartTracing();
    }
}
