using System.Collections.Generic;
using UnityEngine;

namespace Sylpheed.UtilityAI
{
    public class UtilityAgent : MonoBehaviour
    {
        [SerializeField] private List<Behavior> _behaviors = new();
    }
}