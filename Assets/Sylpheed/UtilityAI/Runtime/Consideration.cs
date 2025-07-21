using UnityEngine;

namespace Sylpheed.UtilityAI
{
    public abstract class Consideration : ScriptableObject
    {
        [SerializeField] private AnimationCurve _curve;
        [Tooltip("Consideration with higher priority are evaluated first. Higher value means higher priority.")]
        [SerializeField] private int _priority;
        
        public int Priority => _priority;
        
        #region Overrides
        protected abstract float OnEvaluate(UtilityAgent agent, GameObject target);
        #endregion

        public float Evaluate(UtilityAgent agent, GameObject target)
        {
            var score = OnEvaluate(agent, target);
            return Mathf.Clamp(score, 0f, 1f);
        }

        protected float EvaluateCurve(float normalizedValue) => _curve.Evaluate(normalizedValue);
        protected float EvaluateCurve(float value, float max) => _curve.Evaluate(value / max);
    }
}
