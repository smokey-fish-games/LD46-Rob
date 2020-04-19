public static class Settings
{
    private static bool music = true;
    private static int diff = 1;

    public enum DIFFICULTY {EASY, MEDIUM, HARD};

    public static void setMusic(bool b)
    {
        music = b;
    }

    public static bool getMusic()
    {
        return music;
    }

    public static void setDiff(int d)
    {
        diff = d;
    }

    public static int getDiff()
    {
        return diff;
    }
}
