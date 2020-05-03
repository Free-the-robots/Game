using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleChooser : MonoBehaviour
{
    public Projectiles.Particle particle = null;
    public Projectiles.ParticleEvolutive particleEvo = null;
    public Projectiles.ParticleHomingForce particleHoming = null;
    public Projectiles.Laser laser = null;
    public Projectiles.LaserEvolutive laserevo = null;

    public Projectiles.Particle active = null;
}
