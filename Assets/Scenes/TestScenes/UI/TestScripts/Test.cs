using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Test : MonoBehaviour
{
    private ParticleSystem Particle;
    // Start is called before the first frame update

    
    void Start()
    {
        Particle = GameObject.Find("Particle System").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //var main = Particle.main;
        //ParticleSyste;
        //Particle.startRotation = (this.transform.rotation.y);
        //ParticleSyste.startRotation3D = this.transform.rotation;
        //main.startRotation = this.transform.rotation.y;
        //main.simulationSpace = ParticleSystemSimulationSpace.Custom;
        //main.customSimulationSpace = this.transform;

        //ParticleSystem.MainModule mainmode = Particle.

    }
}
