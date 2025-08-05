using UnityEngine;

namespace Sylpheed.UtilityAI.Samples
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float _max = 100f;

        private float _current;
        
        public float Max => _max;

        public float Current
        {
            get => _current;
            set => _current = Mathf.Clamp(value, 0, _max);
        }

        public float Normalized => Current / Max;

        public void TakeDamage(float amount)
        {
            Current -= amount;
        }

        public void Heal(float amount)
        {
            Current += amount;
        }
    }
}