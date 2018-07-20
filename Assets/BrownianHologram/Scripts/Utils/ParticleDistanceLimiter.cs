using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleDistanceLimiter : MonoBehaviour {

    public Transform target;
    public float MaxDistance = 2;

    new ParticleSystem particleSystem;
    ParticleSystem.Particle[] particles;

    private void Awake() {
        particleSystem = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
    }
    private void Update() {
        HideFarParticles();
    }

    void HideFarParticles() {
    }
}
