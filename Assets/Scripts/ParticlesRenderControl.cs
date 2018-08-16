using UnityEngine;

public class ParticlesRenderControl : MonoBehaviour {

    public GameObject cage;
    public Material particlesMaterial;
    public ParticleSystemRenderer particleRenderer;

    void Start () {
        var cageBounds = cage.GetComponent<BoxCollider>().bounds;
        particlesMaterial.SetVector("_A", cageBounds.min);
        particlesMaterial.SetVector("_B", cageBounds.max);
        particleRenderer.material = particlesMaterial;
    }
}
