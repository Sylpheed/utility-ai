using System;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sylpheed.UtilityAI.Sample.UI
{
    public class InGameView : MonoBehaviour
    {
        [SerializeField] private GameObject _unit;
        [SerializeField] private UIDocument _ui;
        
        private void Start()
        {
            var vm = new InGameViewModel(_unit);
            
            var root = _ui.rootVisualElement;
            root.dataSource = vm;
        }
    }
    
    public class InGameViewModel
    {
        [CreateProperty] public float HealthCurrent => _health.Current;
        [CreateProperty] public float HealthMax => _health.Max;
        [CreateProperty] public string HealthText => $"{_health.Current:N0} / {_health.Max:N0}";
            
        [CreateProperty] public float StaminaCurrent => _stamina.Current;
        [CreateProperty] public float StaminaMax => _stamina.Max;
        [CreateProperty] public string StaminaText => $"{_stamina.Current:N0} / {_stamina.Max:N0}";

        private Health _health;
        private Stamina _stamina;

        [CreateProperty] public ProgressBarVM Health { get; private set; }
        [CreateProperty] public ProgressBarVM Stamina { get; private set; }

        public InGameViewModel(GameObject gameObject)
        {
            _health = gameObject.GetComponent<Health>();
            _stamina = gameObject.GetComponent<Stamina>();
            
            Health = new ProgressBarVM(() =>  _health.Current, () => _health.Max);
            Stamina = new ProgressBarVM(() =>  _stamina.Current, () => _stamina.Max);
        }

        public class ProgressBarVM
        {
            [CreateProperty] public float Current => _getCurrent();
            [CreateProperty] public float Max => _getMax();
            [CreateProperty] public string Text => $"{Current:N0} / {Max:N0}";
            
            private readonly Func<float> _getCurrent;
            private readonly Func<float> _getMax;

            public ProgressBarVM(Func<float> getCurrent, Func<float> getMax)
            {
                _getCurrent = getCurrent;
                _getMax = getMax;
            }
        }
    }
}