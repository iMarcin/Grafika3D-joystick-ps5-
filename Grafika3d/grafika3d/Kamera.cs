namespace grafika3d
{
    //odleglosci kamery i zasieg widzenia
    public class Kamera : Transformowalne
    {
        public float Blisko { get; set; } = 1.0f;
        public float Daleko { get; set; } = 50.0f;
        public float Fov { get; set; } = 80.0f;
    }
}
