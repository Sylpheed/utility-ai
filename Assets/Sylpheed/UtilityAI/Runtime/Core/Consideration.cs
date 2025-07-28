using System.Collections.Generic;
using UnityEngine;

namespace Sylpheed.UtilityAI
{
    public abstract class Consideration : ScriptableObject
    {
        [SerializeField] private AnimationCurve _curve;
        [Tooltip("Consideration with higher priority are evaluated first. Higher value means higher priority.")]
        [SerializeField] private int _priority;
        
        [Header("Target")] 
        [Tooltip("When set, decisions will be evaluated per target based on this behavior.")]
        [SerializeField] private bool _requiresTarget;
        [Tooltip("When set, only evaluate targets with the specified tags.")]
        [SerializeField] private Tag[] _requiredTargetTags;

        public int Priority => _priority;
        public bool RequiresTarget => _requiresTarget;
        public IReadOnlyCollection<Tag> RequiredTargetTags => _requiredTargetTags;
        
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
            // Opt-out
            if (_requiresTarget && !decision.Target) return 0;
            if (_requiresTarget && !decision.Target.HasTags(_requiredTargetTags)) return 0;
            
            var score = OnEvaluate(decision);
            return Mathf.Clamp01(score);
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
