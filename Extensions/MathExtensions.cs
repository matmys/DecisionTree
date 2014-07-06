namespace DecisionTree.Extensions
{
    public static class MathExtensions
    {
        public static int RoundUp(double toRound, int multiplication)
        {
            return (int)((multiplication - toRound % multiplication) + toRound);
        }

        public static int RoundDown(double toRound, int multiplication)
        {
            return (int)(toRound - toRound % multiplication);
        }
    }
}
