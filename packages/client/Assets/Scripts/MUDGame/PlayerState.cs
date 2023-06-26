using System;
using UniRx;
using DefaultNamespace;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using ObservableExtensions = UniRx.ObservableExtensions;

namespace mud.Unity
{

    public class PlayerState : MonoBehaviour
    {
        public float m_StartingHealth = 100f;
        private float m_CurrentHealth;

        // TODO: Get PlayerSync

        private ParticleSystem m_ExplosionParticles;
        private bool m_Dead;
        private CompositeDisposable _disposable = new();


        private void Awake()
        {
            
        }


        private void OnEnable()
        {
            m_CurrentHealth = m_StartingHealth;
            m_Dead = false;

            Init();
        }

        // TODO: Callback for HealthTable update
        // private void OnHealthChange(HealthTableUpdate update)
        // {
        // }

        // TODO: Callback for HealthTable deletion
        // private void OnPlayerDeath(HealthTableUpdate update)
        // {
        // }

        private void Init()
        {
         
        }

        private void OnDeath()
        {
            
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}