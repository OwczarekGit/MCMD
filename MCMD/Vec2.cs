namespace MCModDownloader
{
    public class Vec2
    {
        public int x;
        public int y;

        public Vec2(int w, int h)
        {
            x = w;
            y = h;
        }

        public bool isChanged(Vec2 other)
        {
            if (x == other.x && y == other.y)
                return false;

            return true;
        }
    }
}