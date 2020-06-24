namespace grafika3d
{
    class Program
    {
        // ruch WASD, obrot myszka, wejdz do okna LPM, wyjdz z okna PPM, do gory spacja, do dolu LShift
        // mozliwa zmiana rozdzielczosci okna
        static void Main()
        {
            Okno window = new Okno(1280, 720, "joystick i prototyp PS5");
            window.StartMainLoop();
        }
    }
}
