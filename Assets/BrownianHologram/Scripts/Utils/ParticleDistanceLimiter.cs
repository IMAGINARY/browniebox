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
        int count = particleSystem.GetParticles(particles);
        int remainingCount = KeepNearParticles(count);
        particleSystem.SetParticles(particles, remainingCount);
    }

    int KeepNearParticles(int max) {
        int remainingCount = 0;
        for (int i = 0 ; i < max ; i++)
            if (Vector3.Distance(particles[i].position, target.position) < MaxDistance) {
                particles[remainingCount] = particles[i];
                remainingCount++;
            }

        return remainingCount;
    }
}
