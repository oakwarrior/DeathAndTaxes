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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGoodBadNeutral
{
    Good,
    Bad,
    Neutral
}

public class StatusParticle : MonoBehaviour
{
    [SerializeField]
    ParticleSystem PORTIKUL;
    [SerializeField]
    Texture SpriteGood;
    [SerializeField]
    Texture SpriteBad;
    [SerializeField]
    Texture SpriteNeutral;
    [SerializeField]
    Vector3 DownwardAcceleration;
    [SerializeField]
    float MaxSpeed = 4.0f;

    ParticleSystemRenderer PORTIKULRENDERER;
    EGoodBadNeutral CurrentStatus = EGoodBadNeutral.Neutral;

    // Start is called before the first frame update
    void Start()
    {
        if(PORTIKUL != null && PORTIKULRENDERER == null)
        {
            PORTIKULRENDERER = PORTIKUL.GetComponent<ParticleSystemRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PORTIKUL != null && PORTIKULRENDERER == null)
        {
            PORTIKULRENDERER = PORTIKUL.GetComponent<ParticleSystemRenderer>();
        }
    }

    public void PlayParticles()
    {
        if(!PORTIKUL.isPlaying)
        {
            PORTIKUL.Play();
        }
    }

    public void SetStatus(EGoodBadNeutral status)
    {
        CurrentStatus = status;
        if(PORTIKULRENDERER == null)
        {
            return;
        }

        switch (status)
        {
            case EGoodBadNeutral.Good:
                PORTIKULRENDERER.material.SetTexture("_MainTex", SpriteGood);
                if (!PORTIKUL.isPlaying)
                {
                    PORTIKUL.Play();
                }
                break;
            case EGoodBadNeutral.Bad:
                PORTIKULRENDERER.material.SetTexture("_MainTex", SpriteBad);
                if (!PORTIKUL.isPlaying)
                {
                    PORTIKUL.Play();
                }
                break;
            case EGoodBadNeutral.Neutral:
                if (SpriteNeutral == null)
                {
                    PORTIKUL.Stop();
                }
                else
                {
                    PORTIKULRENDERER.material.SetTexture("_MainTex", SpriteNeutral);
                    if (!PORTIKUL.isPlaying)
                    {
                        PORTIKUL.Play();
                    }
                }
                break;
        }
    }

    ParticleSystem.Particle[] Particles;

    Vector3 ParticleVelocity = Vector3.zero;

    public void SpeedUpParticles(float acceleration, Vector3 dir)
    {
        Particles = new ParticleSystem.Particle[PORTIKUL.main.maxParticles];

        int aliveParticlesCount = PORTIKUL.GetParticles(Particles);

        // Change only the particles that are alive
        for (int i = 0; i < aliveParticlesCount; i++)
        {
            if (Particles[i].velocity.magnitude == 0 && acceleration > 0.0f)
            {
                ParticleVelocity.x = Random.Range(-0.1f, 0.1f);
                ParticleVelocity.y = Random.Range(-0.1f, 0.1f);
                Particles[i].velocity = ParticleVelocity;
            }

            if (Particles[i].velocity.magnitude < MaxSpeed)
            {
                Particles[i].velocity = Particles[i].velocity + dir * acceleration;
            }

            if(Particles[i].position.magnitude > 7.5f)
            {
                Particles[i].position = Vector3.zero;
            }

            //particles[i].velocity = (particles[i].velocity.normalized + dir).normalized * (Mathf.Clamp(particles[i].velocity.magnitude, 0.0f, 5.0f) + acceleration);
        }
        PORTIKUL.SetParticles(Particles, aliveParticlesCount);
    }

    public void SlowDownParticles(float acceleration)
    {
        Particles = new ParticleSystem.Particle[PORTIKUL.main.maxParticles];

        int aliveParticlesCount = PORTIKUL.GetParticles(Particles);

        // Change only the particles that are alive
        for (int i = 0; i < aliveParticlesCount; i++)
        {
            Particles[i].velocity = (Particles[i].velocity.normalized).normalized * (Mathf.Clamp(Particles[i].velocity.magnitude, 0.0f, MaxSpeed) + acceleration);
        }
        PORTIKUL.SetParticles(Particles, aliveParticlesCount);
    }

    public void FallDownParticles()
    {
        Particles = new ParticleSystem.Particle[PORTIKUL.main.maxParticles];

        int aliveParticlesCount = PORTIKUL.GetParticles(Particles);

        // Change only the particles that are alive
        for (int i = 0; i < aliveParticlesCount; i++)
        {
            //if (particles[i].velocity.magnitude == 0)
            //{
            //    particles[i].velocity = new Vector3(0, Random.Range(-0.1f, -0.2f));
            //}
            if (Particles[i].position.y > gameObject.transform.localPosition.y - 0.5f && Particles[i].velocity.magnitude < MaxSpeed)
            {
                Particles[i].velocity = Particles[i].velocity + DownwardAcceleration;
            }
            else
            {
                //particles[i].velocity = particles[i].velocity.normalized * (Mathf.Clamp(particles[i].velocity.magnitude, 0.0f, 5.0f) - 2.0f);
            }
            //particles[i].velocity = particles[i].velocity.normalized * (Mathf.Clamp(particles[i].velocity.magnitude, 0.0f, 5.0f) + acceleration);
        }
        PORTIKUL.SetParticles(Particles, aliveParticlesCount);
    }
}
