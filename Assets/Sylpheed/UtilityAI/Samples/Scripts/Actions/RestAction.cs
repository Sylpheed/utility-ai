using UnityEngine;

namespace Sylpheed.UtilityAI.Sample
{
    [System.Serializable]
    public class RestAction : Action
    {
        private Camp _camp;
        private Health _health;

        protected override bool OnEnter()
        {
            _health = Agent.GetComponent<Health>();
            if (!_health) return false;
            
            _camp = Target.GetComponent<Camp>();
            if (!_camp) return false;
            
            return true;
        }

        protected override void OnUpdate(float deltaTime)
        {
            _health.Heal(_camp.HealthRegenRate * deltaTime);
        }
    }
}