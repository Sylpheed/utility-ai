using System.Collections.Generic;
using UnityEngine;

namespace Sylpheed.UtilityAI
{
    [CreateAssetMenu(fileName = "Behavior", menuName = "Utility AI/Behavior")]
    public class Behavior : ScriptableObject
    {
        [SerializeReference, SubclassSelector] private IAction _action;
        [SerializeField] private Consideration[] _considerations;
        [SerializeField] private float _weight = 1;

        [Header("Target")] 
        [Tooltip("When set, decisions will be evaluated per target based on this behavior.")]
        [SerializeField] private bool _requiresTarget;
        [Tooltip("When set, only evaluate targets with the specified tags.")]
        [SerializeField] private Tag[] _requiredTargetTags;
        
        public IAction Action => _action;
        public IReadOnlyList<Consideration> Considerations => _considerations;
        public float Weight => _weight;
        
        public bool RequiresTarget => _requiresTarget;
        public IReadOnlyCollection<Tag> RequiredTargetTags => _requiredTargetTags;
    }
}