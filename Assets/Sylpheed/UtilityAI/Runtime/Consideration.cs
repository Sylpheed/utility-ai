using System.Collections.Generic;
using UnityEngine;

namespace Sylpheed.UtilityAI
{
    public abstract class Consideration : ScriptableObject
    {
        [SerializeField] private AnimationCurve _curve;
        
        [Tooltip("Consideration with higher priority are evaluated first. Higher value means higher priority.")]
        [SerializeField] private int _priority;

        public int Priority => _priority;
        
        #region Overridables
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

        #region Curve Evaluation
        /// <summary>
        /// Evaluate curve using a normalized 0..1 value.
        /// </summary>
        /// <param name="normalizedValue"></param>
        /// <returns></returns>
        protected float EvaluateCurve(float normalizedValue) => Mathf.Clamp01(_curve.Evaluate(normalizedValue));
        /// <summary>
        /// Evaluate curve using a 0..max range.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        protected float EvaluateCurve(float value, float max) => EvaluateCurve(value / max);
        /// <summary>
        /// Evaluate curve using a min..max range. Use this for non-zero min value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        protected float EvaluateCurve(float value, float min, float max) => EvaluateCurve(value - min, max - min);
        #endregion
    }
}
