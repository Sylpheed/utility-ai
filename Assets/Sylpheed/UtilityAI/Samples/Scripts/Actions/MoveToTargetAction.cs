using UnityEngine.AI;

namespace Sylpheed.UtilityAI.Sample
{
    [System.Serializable]
    public class MoveToTargetAction : Action
    {
        private NavMeshAgent _navAgent;
        
        protected override void OnEnter()
        {
            _navAgent = Agent.GetComponent<NavMeshAgent>();
            if (!_navAgent)
            {
                Exit();
                return;
            }
            
            _navAgent.isStopped = false;
            _navAgent.SetDestination(Decision.Target.transform.position);
        }
        
        protected override bool ShouldExit()
        {
            return _navAgent.remainingDistance <= _navAgent.stoppingDistance + 0.01f;
        }

        protected override void OnExit()
        {
            _navAgent.isStopped = true;
        }
    }
}