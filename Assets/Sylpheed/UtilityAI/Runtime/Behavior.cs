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
        
        public IAction Action => _action;
        public IReadOnlyList<Consideration> Considerations => _considerations;
        public float Weight => _weight;
    }
}