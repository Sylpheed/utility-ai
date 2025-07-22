using JetBrains.Annotations;
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
        protected abstract float OnEvaluate(Decision decision);
        #endregion

        /// <summary>
        /// Evaluate the score for this consideration.
        /// </summary>
        /// <param name="decision"></param>
        /// <returns>Clamped to 0..1</returns>
        public float Evaluate(Decision decision)
        {
            var score = OnEvaluate(decision);
            return Mathf.Clamp(score, 0f, 1f);
        }

        protected float EvaluateCurve(float normalizedValue) => Mathf.Clamp01(_curve.Evaluate(normalizedValue));
        protected float EvaluateCurve(float value, float max) => EvaluateCurve(value / max);
    }
}
