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
        [SerializeField] private List<string> _tags;
        
        private readonly HashSet<string> _runTimeTags = new();
        public IReadOnlyCollection<string> Tags => _runTimeTags;
        
        public void AddTags(params string[] tags)
        {
            foreach (var t in tags) 
                _runTimeTags.Add(t);
        }

        public void RemoveTags(params string[] tags)
        {
            
            foreach (var t in tags) 
                _runTimeTags.Add(t);
        }
        
        public float DistanceFromAgent(UtilityAgent agent) => Vector3.Distance(agent.transform.position, transform.position);
    }
}