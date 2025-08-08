using UnityEngine;

namespace Sylpheed.UtilityAI.Sample
{
    [System.Serializable]
    public class RestAction : Action
    {
        private Camp _camp;
        private Health _health;
        private Stamina _stamina;

        protected override bool OnEnter()
        {
            _health = Agent.GetComponent<Health>();
            _stamina = Agent.GetComponent<Stamina>();
            
            _camp = Target.GetComponent<Camp>();
            if (!_camp) return false;
            
            return true;
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (_health) _health.Heal(_camp.HealthRegenRate * deltaTime);
            if (_stamina) _stamina.Current += _camp.StaminaRegenRate * deltaTime;
        }
    }
}