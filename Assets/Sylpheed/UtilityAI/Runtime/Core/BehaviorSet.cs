using System.Collections.Generic;
using UnityEngine;

namespace Sylpheed.UtilityAI
{
    [CreateAssetMenu(fileName = "BehaviorSet", menuName = "Utility AI/Behavior Set")]
    public sealed class BehaviorSet : ScriptableObject
    {
        [SerializeField] private List<Behavior> _behaviors;
        
        public IReadOnlyCollection<Behavior> Behaviors => _behaviors;
    }
}