namespace grafika3d
{
    //operacje na wierzcholkach trojkatow
    public struct Wierzcholek
    {
        public Vec3 Position { get; set; }
        public Vec4 Color { get; set; }

        public Wierzcholek(Vec3 position, Vec4 color)
        {
            Position = position;
            Color = color;
        }
    }
}
