using UnityEngine;
using UnityEngine.AI;

namespace Sylpheed.UtilityAI.Samples
{
    [System.Serializable]
    public class PatrolAction : Action
    {
        [SerializeField] private float _radius = 5f;

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
            
            _navAgent.SetDestination(targetPos.Value);
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (_navAgent.pathStatus == NavMeshPathStatus.PathComplete)
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
                Vector3 dir = Random.insideUnitCircle * _radius;
                var pos = Agent.transform.position + dir;
                if (!NavMesh.SamplePosition(pos, out var hit, _radius, 1))
                    continue;
                
                return hit.position;
            }
            
            return null;
        }
    }
}