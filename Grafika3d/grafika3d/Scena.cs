using System;
using System.Collections.Generic;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace grafika3d
{
    //wyglad okna, funkcje w oknie oraz wyswietlanie koordynatow i fps-ow
    abstract class Scena : Drawable
    {
        public Kamera mainCamera = new Kamera();
        public Transformacja worldTransform = Transformacja.Identity;

        public List<ZrodloSwiatla > Lampy = new List<ZrodloSwiatla >();
        public List<Rysowalne> drawables = new List<Rysowalne>();
        protected Time deltaTime = new Time();
        protected Indykator  axisIndicator = new Indykator ();

        protected static List<Keyboard.Key> pressedKeys = new List<Keyboard.Key>();

        protected Font font = new Font(@"..\..\..\Fonts\arial.ttf");

        public virtual void Update(Time deltaTime) { this.deltaTime = deltaTime; Keys(); }

        public void Draw(RenderTarget target, RenderStates states)
        {
            SilnikRenderujacy.Instance.RenderowanieSceny(this, target, states);
            target.Draw(axisIndicator);
            Vec3 p = mainCamera.Position;
            string coords = string.Format("Koordynaty:\nx:{0}\ny:{1}\nz:{2}", p.X, p.Y, p.Z);
            string debugText = string.Format("fps: {0}\n{1}", string.Format("{0:0.00}", 1f / deltaTime.AsSeconds()), coords);
            Text t = new Text(debugText, font)
            {
                CharacterSize = 14,
                FillColor = Color.Yellow,
                OutlineColor = Color.Blue,
                OutlineThickness = 0.5f,
                Style = Text.Styles.Regular,
            };
            target.Draw(t);
            t.Dispose();

        }

        public virtual void Window_MouseWheelScrolled(object sender, MouseWheelScrollEventArgs e) { }
        public virtual void Window_Closed(object sender, EventArgs e) { }
        public virtual void Window_Resized(object sender, SizeEventArgs e) { }
        public virtual void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e) { }
        public virtual void Window_MouseMoved(object sender, MouseMoveEventArgs e) { }
        public virtual void Keys() { }
        public virtual void Window_KeyReleased(object sender, KeyEventArgs e)
        {
            pressedKeys.Remove(e.Code);
        }
        public virtual void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                default:
                    pressedKeys.Add(e.Code);
                    break;
            }
        }
    }
}
