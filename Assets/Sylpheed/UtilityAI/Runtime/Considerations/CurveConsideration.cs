using UnityEngine;

namespace Sylpheed.UtilityAI.Considerations
{
    public abstract class CurveConsideration : Consideration
    {
        [SerializeField] private AnimationCurve _curve;
        
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
    }
}