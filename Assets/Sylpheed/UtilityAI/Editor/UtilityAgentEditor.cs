using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Sylpheed.UtilityAI.Editor
{
    [CustomEditor(typeof(UtilityAgent))]
    public class UtilityAgentEditor : UnityEditor.Editor
    {
        private struct LabelColors
        {
            public static readonly Color Best = Color.green;
            public static readonly Color Scored = Color.orange;
            public static readonly Color Skipped = Color.red;
            public static readonly Color Unscored = Color.gray;
        }
        
        private static readonly Dictionary<float, bool> DecisionResultFoldoutStates = new();
        
        private UtilityAgent _agent;
        private Color _defaultLabelColor;
        
        private void OnEnable()
        {
            _agent = (UtilityAgent)target;
            _defaultLabelColor = GUI.color;
            RequiresConstantRepaint();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI(); // Draw the default inspector

            EditorGUILayout.Space();
            if (Application.isPlaying)
            {
                // Sort results
                var results = _agent.DecisionResults
                    .OrderByDescending(d => d.Best)
                    .ThenByDescending(d => d.Scored)
                    .ThenBy(d => d.Skipped)
                    .ThenByDescending(d => d.Score)
                    .ToList();

                foreach (var result in results)
                {
                    DrawDecisionResult(result);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Enter Play mode to view utility values.", MessageType.Info);
            }
        }

        private void DrawDecisionResult(DecisionResult result)
        {
            if (result.Best) GUI.color = LabelColors.Best;
            else if (result.Scored)
            {
                if (result.Skipped) GUI.color = LabelColors.Skipped;
                else GUI.color = LabelColors.Scored;
            }
            else GUI.color = LabelColors.Unscored;

            var text = $"[{result.Decision.Score * 100:N0}] {result.Decision.Behavior.name}\t";
            if (result.Decision.Target) text += $" Target: {result.Decision.Target.name}";
            if (result.Decision.Data != null) text += $" Data: {result.Decision.Data}";
            // EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
            
            // Check foldout state cache. Create a new entry if it doesn't exist yet.
            if (!DecisionResultFoldoutStates.TryGetValue(result.Hash, out _))
                DecisionResultFoldoutStates[result.Hash] = false;
            DecisionResultFoldoutStates[result.Hash] = EditorGUILayout.Foldout(DecisionResultFoldoutStates[result.Hash], text, true);
                    
            GUI.color = _defaultLabelColor;

            // Expand foldout
            if (DecisionResultFoldoutStates[result.Hash])
            {
                // Same decision
                if (result.IsSameDecision && !Mathf.Approximately(_agent.SameDecisionScoreBonus, 1f))
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.LabelField($"[{_agent.SameDecisionScoreBonus * 100:N0}] Same Decision Bonus");
                    EditorGUI.indentLevel--;
                }
            
                // Draw considerations
                EditorGUI.indentLevel++;
                foreach (var consideration in result.Decision.Behavior.Considerations)
                {
                    DrawConsideration(consideration, result.Decision);
                }
                EditorGUI.indentLevel--;
            }
        }

        private void DrawConsideration(IConsideration consideration, Decision decision)
        {
            var text = $"[{_agent.GetCachedConsiderationScore(decision, consideration) * 100:N0}] {consideration.Name}";
            EditorGUILayout.LabelField(text);

            // Draw child recursively
            if (consideration.Children != null)
            {
                EditorGUI.indentLevel++;
                foreach (var child in consideration.Children)
                {
                    DrawConsideration(child, decision);
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}