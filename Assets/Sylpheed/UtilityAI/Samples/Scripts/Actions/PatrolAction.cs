using UnityEngine;
using UnityEngine.AI;

namespace Sylpheed.UtilityAI.Samples
{
    [System.Serializable]
    public class PatrolAction : Action
    {
        [SerializeField] private float _minRadius = 10f;
        [SerializeField] private float _maxRadius = 20f;

        private NavMeshAgent _navAgent;
        
        protected override void OnEnter()
        {
            _navAgent = Agent.GetComponent<NavMeshAgent>();
            
            // Get a random patrol point
            var targetPos = GetRandomDestination();
            if (!targetPos.HasValue)
            {
                Complete();
                return;
            }

            _navAgent.isStopped = false;
            _navAgent.SetDestination(targetPos.Value);
        }

        protected override void OnUpdate(float deltaTime)
        {
            // Debug.Log($"Remaining distance: {_navAgent.remainingDistance}");
            if (_navAgent.remainingDistance <= _navAgent.stoppingDistance + 0.01f)
                Complete();
        }

        protected override void OnExit()
        {
            _navAgent.isStopped = true;
        }

        private Vector3? GetRandomDestination()
        {
            var maxRetries = 5;

            for (var i = 0; i < maxRetries; i++)
            {
                var radius = Random.Range(_minRadius, _maxRadius);
                var dir = Random.insideUnitCircle * radius;
                var pos = Agent.transform.position + new Vector3(dir.x, 0, dir.y);
                if (!NavMesh.SamplePosition(pos, out var hit, radius, 1))
                    continue;
                
                return hit.position;
            }
            
            return null;
        }
    }
}