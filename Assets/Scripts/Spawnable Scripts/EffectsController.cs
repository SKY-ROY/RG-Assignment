using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour
{
    public ParticleSystem particleEffect;

    public void PlayParticleEffect()
    {
        particleEffect.Play();
    }
}
