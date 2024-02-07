using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParticleDirection : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private ParticleSystem.Particle[] particles;
    private ParticleSystem.Particle particle;
    private Vector3 direction;
    private int numParticlesAlive;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
    }

    private void Update()
    {
        numParticlesAlive = particleSystem.GetParticles(particles);
        for (int i = 0; i < numParticlesAlive; i++)
        {
            particle = particles[i];
            direction = (particle.position - particleSystem.transform.position).normalized;
            particle.rotation3D = direction;
            particles[i] = particle;
        }
        particleSystem.SetParticles(particles, numParticlesAlive);
    }
}