using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sylpheed.UtilityAI
{
    [CreateAssetMenu(fileName = "Behavior", menuName = "Utility AI/Behavior")]
    public class Behavior : ScriptableObject
    {
        [SerializeReference, SubclassSelector] private IAction _action;
        
        [Header("Decision")]
        [SerializeField] private Consideration[] _considerations;
        [SerializeField] private float _weight = 1;
        
        public IAction Action => _action;
        public IReadOnlyList<Consideration> Considerations { get; private set; }
        public float Weight => _weight;
        public bool RequiresTarget { get; private set; }
        public IReadOnlyCollection<Tag> RequiredTargetTags { get; private set; }

        private void OnEnable()
        {
            RebuildCache();
        }

        private void OnValidate()
        {
            RebuildCache();
        }

        private void RebuildCache()
        {
            RequiresTarget = _considerations.Any(c => c.RequiresTarget);
            RequiredTargetTags = _considerations.SelectMany(c => c.RequiredTargetTags).Distinct().ToList();
            Considerations = _considerations.OrderByDescending(c => c.Priority).ToArray();
        }
    }
}