using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Complete
{
    public class TankCollision : MonoBehaviour
    {
        [SerializeField] private bool m_damageBoth = true;
        public LayerMask m_TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
        public ParticleSystem m_CollidingParticles;         // Reference to the particles that will play on explosion.
        public AudioSource m_CollidingAudio;                // Reference to the audio that will play on explosion.
        public float m_MaxDamage = 40f;                    // The amount of damage done if the explosion is centred on a tank.
        public Rigidbody m_RigidBody;                       // using rigidbody to comapare velocities
        public float m_MinVelocity;
        public float m_MaxVelocity;
        private TankHealth m_TankHealth;

        // Start is called before the first frame update
        void Start()
        {
            m_RigidBody = GetComponent<Rigidbody>();
            m_TankHealth = GetComponent<TankHealth>();
        }

        
        // Update is called once per frame
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "Player")
            {

               // Debug.Log("HIT Velocity" + m_RigidBody.velocity.magnitude);
                TankCollision enemy = other.gameObject.GetComponent<TankCollision>();

                Rigidbody enemyRigidBody = enemy.m_RigidBody;
                float enemyVelocity = enemyRigidBody.velocity.magnitude;
                float myVelocity = m_RigidBody.velocity.magnitude;

                // calculate enemy's realtive velocity and convert to relative damage
                float relativeHitForce = (myVelocity / m_MaxVelocity) * m_MaxDamage;

                if (myVelocity > m_MinVelocity)
                {
                    if (myVelocity > enemyVelocity)
                    {
                        enemy.m_TankHealth.TakeDamage(relativeHitForce);
                    }

                    else if (m_damageBoth)
                    {
                        enemy.m_TankHealth.TakeDamage(relativeHitForce / 2);
                    }

                    ParticleSystem smoke = Instantiate(m_CollidingParticles,enemy.transform.position,Quaternion.Euler(-90,0,0), parent:null);
                    Destroy(smoke.gameObject,2);
                }
                m_CollidingAudio.volume = myVelocity / m_MaxVelocity;
                m_CollidingAudio.Play();
            }


        }
    }
}
