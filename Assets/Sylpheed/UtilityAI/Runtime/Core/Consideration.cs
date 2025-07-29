using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sylpheed.UtilityAI
{
    public abstract class Consideration : ScriptableObject
    {
        [Tooltip("Consideration with higher priority are evaluated first. Higher value means higher priority.")]
        [SerializeField] private int _priority;
        
        [Header("Target")] 
        [Tooltip("When set, decisions will be evaluated per target based on this behavior.")]
        [SerializeField] private bool _requiresTarget;
        [Tooltip("When set, only evaluate targets with the specified tags.")]
        [SerializeField] private Tag[] _requiredTargetTags = Array.Empty<Tag>();

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
        
        /// <summary>
        /// Contains constants for consideration score
        /// </summary>
        public static class Result
        {
            /// <summary>
            /// Veto out the decision. Results to a score of 0.
            /// </summary>
            public static readonly float OptOut = 0f;
            /// <summary>
            /// Fully opt-in to the consideration, giving a maximum score of 1.
            /// </summary>
            public static readonly float OptIn = 1f;
        }
    }
}
