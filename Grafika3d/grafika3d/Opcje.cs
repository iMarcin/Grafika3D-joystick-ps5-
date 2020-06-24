namespace grafika3d
{
    public class Opcje
    {
        //opcje szybkosci
        public float SzybkoscPoruszania { get; set; } = 5;
        public float SzybkoscRotacji { get; set; } = 1;
        public static Opcje Instance { get; } = new Opcje();
        private Opcje()
        {
        }
    }
}
