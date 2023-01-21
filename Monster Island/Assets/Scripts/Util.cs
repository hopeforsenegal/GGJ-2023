
public static class Util
{
    // Simply switches on every call
    public static bool Switch(ref bool @switch)
    {
        @switch = !@switch;
        return @switch;
    }

    // Increments on every call than start back at zero
    public static int IncrementLoop(ref int val, int max)
    {
        val++;
        try {
            return val;
        }
        finally {
            if (val == max) {
                val = 0;
            }
        }
    }
}
