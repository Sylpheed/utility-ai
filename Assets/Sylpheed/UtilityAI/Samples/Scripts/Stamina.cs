using System;
using UnityEngine;

namespace Sylpheed.UtilityAI.Sample
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

        private void Start()
        {
            _current = _max;
        }
    }
}