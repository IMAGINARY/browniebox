using HoloToolkit.Examples.InteractiveElements;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.Receivers;
using UnityEngine;

public class MRHologramControlUI : InteractionReceiver {

    HologramControl hologramControl;
    public SliderGestureControl speedSlider;
    public SliderGestureControl scaleSlider;

    void Start() {
        hologramControl = FindObjectOfType<HologramControl>();

        speedSlider.OnUpdateEvent.AddListener(hologramControl.SetSpeed);
        speedSlider.SetSliderValue(HologramControl.DefaultSpeed);

        scaleSlider.OnUpdateEvent.AddListener(hologramControl.SetScale);
        scaleSlider.SetSliderValue(HologramControl.DefaultScale);
    }

    protected override void InputDown(GameObject obj, InputEventData eventData) {
        if (obj.name == "Path")
            hologramControl.SwitchTrace();
        else if (obj.name == "AirParticles")
            hologramControl.SwitchAirParticles();
    }

}
