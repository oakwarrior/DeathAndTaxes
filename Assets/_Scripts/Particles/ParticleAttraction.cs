//Copyright 2020 Placeholder Gameworks
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
//and associated documentation files (the "Software"), to deal in the Software without restriction, 
//including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
//and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
//subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
//PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
//FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR 
//OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleAttraction : MonoBehaviour
{
    public Transform Target;

    private ParticleSystem PORTIKULSYSTEM;

    private static ParticleSystem.Particle[] Particles = new ParticleSystem.Particle[1000];

    void Update()
    {
        if (PORTIKULSYSTEM == null)
        {
            PORTIKULSYSTEM = GetComponent<ParticleSystem>();
        }

        if (Target == null)
        {
            return;
        }
        int count = PORTIKULSYSTEM.GetParticles(Particles);

        for (int i = 0; i < count; i++)
        {
            var particle = Particles[i];

            //float distance = Vector3.Distance(Target.position, particle.position);

            //if (distance > 0.1f)
            //{

            //particle.position = Vector3.Lerp(particle.position, Target.position, Time.deltaTime / 2.0f);
            particle.position = Vector3.MoveTowards(particle.position, Target.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f)), 7.0f * Time.deltaTime);

            Particles[i] = particle;
            //}
        }

        PORTIKULSYSTEM.SetParticles(Particles, count);
    }
}