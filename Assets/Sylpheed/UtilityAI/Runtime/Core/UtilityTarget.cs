using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sylpheed.UtilityAI
{
    /// <summary>
    /// Attached to GameObjects that can be queried by the UtilityAgent as target.
    /// </summary>
    public class UtilityTarget : MonoBehaviour
    {
        [SerializeField] private List<Tag> _tags;
        
        private readonly HashSet<Tag> _runTimeTags = new();
        public IReadOnlyCollection<Tag> Tags => _runTimeTags;

        private void Awake()
        {
            AddTags(_tags.ToArray());
        }

        public void AddTags(params Tag[] tags)
        {
            foreach (var t in tags) 
                _runTimeTags.Add(t);
        }

        public void RemoveTags(params Tag[] tags)
        {
            foreach (var t in tags) 
                _runTimeTags.Add(t);
        }
        
        public float DistanceFromAgent(UtilityAgent agent) => Vector3.Distance(agent.transform.position, transform.position);
        public bool HasTags(IReadOnlyCollection<Tag> tags) => _runTimeTags.IsSupersetOf(tags);
    }
}