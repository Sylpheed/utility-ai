using UnityEngine;

namespace Sylpheed.UtilityAI.Sample
{
    [System.Serializable]
    public class RestAction : Action
    {
        private Camp _camp;
        private Health _health;

        protected override void OnEnter()
        {
            _health = Agent.GetComponent<Health>();
            if (!_health)
            {
                Exit();
                return;
            } 
            
            _camp = Target.GetComponent<Camp>();
            if (!_camp)
            {
                Exit();
                return;
            } 
        }

        protected override void OnUpdate(float deltaTime)
        {
            _health.Heal(_camp.HealthRegenRate * Time.deltaTime);
        }
    }
}