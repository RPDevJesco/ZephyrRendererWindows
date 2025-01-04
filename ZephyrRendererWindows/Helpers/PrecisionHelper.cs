namespace ZephyrRendererWindows
{
    internal static class PrecisionHelper
    {
        public static int ToInt(double value)
        {
            return (int)Math.Round(value);
        }
    }
}