using System;
using System.Collections.Generic;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace grafika3d
{
    //funkcje okna przy pomocy biblioteki SFML https://www.sfml-dev.org/download/sfml.net/
    class Okno : RenderWindow
    {
        public Stack<Scena> sceny = new Stack<Scena>();
        Clock clock = new Clock();

        public Okno(uint width, uint height, string title) : base(new VideoMode(width, height), title)
        {
            SetFramerateLimit(90);
            SetKeyRepeatEnabled(false);
            KeyPressed += Window_KeyPressed;
            KeyReleased += Window_KeyReleased;
            Resized += Window_Resized;
            Closed += Window_Closed;
            MouseMoved += Window_MouseMoved;
            MouseButtonPressed += Window_MouseButtonPressed;
            MouseWheelScrolled += Window_MouseWheelScrolled;

            sceny.Push(new Bryly());
        }

        public void StartMainLoop()
        {
            while (IsOpen)
            {
                DispatchEvents();
                sceny.Peek().Update(clock.Restart());
                Clear();
                Draw(sceny.Peek());
                Display();
            }
        }

        void Window_MouseWheelScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            sceny.Peek().Window_MouseWheelScrolled(sender, e);
        }

        void Window_Closed(object sender, EventArgs e)
        {
            Close();
        }

        void Window_Resized(object sender, SizeEventArgs e)
        {
            View newView = new View(new FloatRect(0, 0, e.Width, e.Height));
            SetView(newView);
        }

        void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            sceny.Peek().Window_MouseButtonPressed(sender, e);
        }

        void Window_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            sceny.Peek().Window_MouseMoved(sender, e);
        }

        void Window_KeyReleased(object sender, KeyEventArgs e)
        {
            sceny.Peek().Window_KeyReleased(sender, e);
        }

        void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            sceny.Peek().Window_KeyPressed(sender, e);
        }
    }
}
