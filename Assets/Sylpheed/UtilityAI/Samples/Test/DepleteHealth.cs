using System;
using UnityEngine;
using UnityEngine.AI;

namespace Sylpheed.UtilityAI.Sample
{
    public class DepleteHealth : MonoBehaviour
    {
        [SerializeField] private float _depletionRate;
        [SerializeField] private bool _depleteOnMoveOnly;
        
        private Health _health;
        private NavMeshAgent _agent;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (_depleteOnMoveOnly)
            {
                if (_agent.remainingDistance > _agent.stoppingDistance + 0.01f)
                {
                    _health.TakeDamage(_depletionRate * Time.deltaTime);
                }
            }
            else
            {
                _health.TakeDamage(_depletionRate * Time.deltaTime);
            }
        }
    }
}