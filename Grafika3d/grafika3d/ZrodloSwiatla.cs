using System;

namespace grafika3d
{
    //regulacja swiatla
    public class ZrodloSwiatla : Transformowalne
    {
        private float intensywnosc=1;
        public float Intensywnosc { get => intensywnosc; set => Math.Min(1, Math.Max(0, value)); }
    }
}
