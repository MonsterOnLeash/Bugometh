namespace BossOne {
    public enum BossOneBehaviour
    {
        STAY_STILL,
        WALK_SIDEWAYS,
        HOP_SIDEWAYS,
        HOP_AND_ATTACK
    }

    [System.Serializable]
    public struct BossOneBehaviourThreshold
    {
        public int threshold;
        public BossOneBehaviour behaviour;
    }
}