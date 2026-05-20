namespace Sylpheed.UtilityAI
{
    public sealed class DecisionResult
    {
        public Decision Decision { get; set; }
        public bool Best { get; set; }
        public bool IsSameDecision { get; set; }
        public float WeightedScore { get; set; }
        
        public bool Skipped => Decision.Skipped;
        public bool Scored => Decision.Scored;
        public float Score => Decision.Score;
        public int Hash => Decision.Hash;
    }
}