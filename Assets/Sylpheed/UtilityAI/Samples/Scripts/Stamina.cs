using UnityEngine;

namespace Sylpheed.UtilityAI.Samples
{
    public class Stamina : MonoBehaviour
    {
        [SerializeField] public float _max;
        
        private float _current;
        
        public float Max => _max;
        public float Current
        {
            get => _current;
            set => _current = Mathf.Clamp(value, 0, _max);
        }
        
    }
}