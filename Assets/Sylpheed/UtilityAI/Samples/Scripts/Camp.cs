using System;
using UnityEngine;

namespace Sylpheed.UtilityAI.Sample
{
    public class Camp : MonoBehaviour
    {
        [SerializeField] private float _healthRegenRate = 1f;
        [SerializeField] private float _staminaRegenRate = 1f;
        
        public float HealthRegenRate => _healthRegenRate;
        public float StaminaRegenRate => _staminaRegenRate;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"OnTriggerEnter {other.name}");
        }

        // private void OnTriggerStay(Collider other)
        // {
        //     if (!other.CompareTag("Player")) return;
        //     
        //     var health = other.GetComponent<Health>();
        //     health?.TakeDamage(_healthRegenRate * Time.fixedDeltaTime);
        //     
        //     Debug.Log(other.gameObject.name);
        // }
    }
}