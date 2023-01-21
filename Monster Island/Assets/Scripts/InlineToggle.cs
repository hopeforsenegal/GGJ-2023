namespace MoonlitSystem.Util
{
    public static class InlineToggle
    {
        // Simply switches on every call
        public static bool Switch(ref bool @switch)
        {
            @switch = !@switch;
            return @switch;
        }
    }
}