using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Transformacja=grafika3d.Transformacja;

namespace grafika3d
{
    class Bryly : Scena
    {
        bool IsMouseLockedCenter { get; set; } = false;

        public Bryly()
        {
            //wektory szescianu
            Vec3 v00 = new Vec3(0, 0, 0);
            Vec3 v01 = new Vec3(0, 0, 1);
            Vec3 v02 = new Vec3(0, 1, 0);
            Vec3 v03 = new Vec3(0, 1, 1);
            Vec3 v04 = new Vec3(1, 0, 0);
            Vec3 v05 = new Vec3(1, 0, 1);
            Vec3 v06 = new Vec3(1, 1, 0);
            Vec3 v07 = new Vec3(1, 1, 1);
            Vec3 v10 = new Vec3(0, 0, 0);
            Vec3 v11 = new Vec3(0, 0, 1);
            //wektory kolumny
            Vec3 v12 = new Vec3(0, 10, 0);
            Vec3 v13 = new Vec3(0, 10, 1);
            Vec3 v14 = new Vec3(1, 0, 0);
            Vec3 v15 = new Vec3(1, 0, 1);
            Vec3 v16 = new Vec3(1, 10, 0);
            Vec3 v17 = new Vec3(1, 10, 1);


            Wierzcholek w1 = new Wierzcholek(v00, (Vec4)(Color.Blue + Color.Green));
            Wierzcholek w2 = new Wierzcholek(v01, (Vec4)(Color.Blue + Color.Green));
            Wierzcholek w3 = new Wierzcholek(v02, (Vec4)(Color.Blue + Color.Green));
            Wierzcholek w4 = new Wierzcholek(v03, (Vec4)(Color.Blue + Color.Green));
            Wierzcholek w5 = new Wierzcholek(v04, (Vec4)(Color.Blue + Color.Green));
            Wierzcholek w6 = new Wierzcholek(v05, (Vec4)(Color.Blue + Color.Green));
            Wierzcholek w7 = new Wierzcholek(v06, (Vec4)(Color.Blue + Color.Green));
            Wierzcholek w8 = new Wierzcholek(v07, (Vec4)(Color.Blue + Color.Green));
            Mesh kolumna = new Mesh()
            {
                new Tri(v10,v12,v16,(Vec4)Color.White),
                new Tri(v10,v16,v14,(Vec4)Color.White),
                new Tri(v14,v16,v17,(Vec4)Color.White),
                new Tri(v14,v17,v15,(Vec4)Color.White),
                new Tri(v15,v17,v13,(Vec4)Color.White),
                new Tri(v15,v13,v11,(Vec4)Color.White),
                new Tri(v11,v13,v12,(Vec4)Color.White),
                new Tri(v11,v12,v10,(Vec4)Color.White),
                new Tri(v12,v13,v17,(Vec4)Color.White),
                new Tri(v12,v17,v16,(Vec4)Color.White),
                new Tri(v15,v11,v10,(Vec4)Color.White),
                new Tri(v15,v10,v15,(Vec4)Color.White)
            };
            Mesh szescian = new Mesh()
            {                               
                new Tri( w1, w3, w7),
                new Tri( w1, w7, w5),                              
		        new Tri( w5, w7, w8),
                new Tri( w5, w8, w6),                              
		        new Tri( w6, w8, w4),
                new Tri( w6, w4, w2),                            
		        new Tri( w2, w4, w3),
                new Tri( w2, w3, w1),                               
		        new Tri( w3, w4, w8),
                new Tri( w3, w8, w7),                             
		        new Tri( w6, w2, w1),
                new Tri( w6, w1, w5),
            };
            kolumna.Origin = new Vec3(0, 0, 0);
            szescian.Origin = new Vec3(0, 0, 0);
            Vec3 przenies = new Vec3(-1, 0, -1);
            szescian.Position = przenies;
            kolumna.Position = new Vec3(1, 0.5f, 1);
            szescian.Scale = new Vec3(5, 3, 5);
            //ps5
            Mesh lewastrona = new Mesh()
            {
                new Tri(v10,v12,v16,(Vec4)Color.White),
                new Tri(v10,v16,v14,(Vec4)Color.White),
                new Tri(v14,v16,v17,(Vec4)Color.White),
                new Tri(v14,v17,v15,(Vec4)Color.White),
                new Tri(v15,v17,v13,(Vec4)Color.White),
                new Tri(v15,v13,v11,(Vec4)Color.White),
                new Tri(v11,v13,v12,(Vec4)Color.White),
                new Tri(v11,v12,v10,(Vec4)Color.White),
                new Tri(v12,v13,v17,(Vec4)Color.White),
                new Tri(v12,v17,v16,(Vec4)Color.White),
                new Tri(v15,v11,v10,(Vec4)Color.White),
                new Tri(v15,v10,v15,(Vec4)Color.White)
            };
            lewastrona.Origin = new Vec3(0, 0, 0);
            lewastrona.Position = new Vec3(10.7f, 0, 10);
            lewastrona.Scale = new Vec3(0.3f, 1, 5);
            Mesh prawastrona = new Mesh()
            {
                new Tri(v10,v12,v16,(Vec4)Color.White),
                new Tri(v10,v16,v14,(Vec4)Color.White),
                new Tri(v14,v16,v17,(Vec4)Color.White),
                new Tri(v14,v17,v15,(Vec4)Color.White),
                new Tri(v15,v17,v13,(Vec4)Color.White),
                new Tri(v15,v13,v11,(Vec4)Color.White),
                new Tri(v11,v13,v12,(Vec4)Color.White),
                new Tri(v11,v12,v10,(Vec4)Color.White),
                new Tri(v12,v13,v17,(Vec4)Color.White),
                new Tri(v12,v17,v16,(Vec4)Color.White),
                new Tri(v15,v11,v10,(Vec4)Color.White),
                new Tri(v15,v10,v15,(Vec4)Color.White)
            };
            prawastrona.Origin = new Vec3(0, 0, 0);
            prawastrona.Position = new Vec3(12, 0, 10);
            prawastrona.Scale = new Vec3(0.3f, 1, 5);
            Color szary = new Color(20, 20, 20);
            Mesh srodek = new Mesh()
            {
                new Tri(v10,v12,v16,(Vec4)szary),
                new Tri(v10,v16,v14,(Vec4)szary),
                new Tri(v14,v16,v17,(Vec4)szary),
                new Tri(v14,v17,v15,(Vec4)szary),
                new Tri(v15,v17,v13,(Vec4)szary),
                new Tri(v15,v13,v11,(Vec4)szary),
                new Tri(v11,v13,v12,(Vec4)szary),
                new Tri(v11,v12,v10,(Vec4)szary),
                new Tri(v12,v13,v17,(Vec4)szary),
                new Tri(v12,v17,v16,(Vec4)szary),
                new Tri(v15,v11,v10,(Vec4)szary),
                new Tri(v15,v10,v15,(Vec4)szary)
            };
            srodek.Origin = new Vec3(0, 0, -0.2f);
            srodek.Position = new Vec3(11, 0, 10);
            srodek.Scale = new Vec3(1, 0.99f, 4f);

            //sfera
            Sfera sfera = new Sfera(1, 3, (Vec4)Color.Red);
            sfera.Origin = new Vec3(0, 0, 0);
            sfera.Position = new Vec3(1.5f, 10, 1.5f);
            sfera.Scale = new Vec3(2, 2, 2);

            //rysuj figury
            drawables.Add(szescian);
            drawables.Add(sfera);
            drawables.Add(kolumna);
            drawables.Add(lewastrona);
            drawables.Add(prawastrona);
            drawables.Add(srodek);
            //dodaj swiatlko
            Lampy.Add(new ZrodloSwiatla () { Position = new Vec3(-5, 10, -5), Intensywnosc = 2f });
            Lampy.Add(new ZrodloSwiatla () { Position = new Vec3(5, -3, 5), Intensywnosc = 1f });
            mainCamera.Position = new Vec3(-5, 0, -5);
        }

        public override void Window_MouseWheelScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            float d = (float)Math.Abs(mainCamera.Blisko - Math.Round(mainCamera.Blisko)) / 2;
            mainCamera.Blisko += d * -e.Delta;
            mainCamera.Blisko = Math.Max(mainCamera.Blisko, 0.01f);
            mainCamera.Blisko = Math.Min(mainCamera.Blisko, 0.99f);
        }
        public override void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                IsMouseLockedCenter = true;
                ((Window)sender).SetMouseCursorVisible(false);
            }
            else if (e.Button == Mouse.Button.Right)
            {
                IsMouseLockedCenter = false;
                ((Window)sender).SetMouseCursorVisible(true);
            }
        }
        public override void Window_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            Window window = (Window)sender;
            if (IsMouseLockedCenter)
            {
                Vector2i windowCenter = (Vector2i)(window.Size / 2);
                Mouse.SetPosition(windowCenter, window);
                Vector2i delta = new Vector2i(e.X, e.Y) - windowCenter;

                float rotationScale = Opcje.Instance.SzybkoscRotacji;
                float angleXnoZ = -delta.X / (float)windowCenter.X * (float)Math.PI / 2 * rotationScale; //up down
                float angleYnoZ = -delta.Y / (float)windowCenter.Y * (float)Math.PI / 2 * rotationScale; //left right
                float sinZ = (float)Math.Sin(mainCamera.Rotation.Z);
                float cosZ = (float)Math.Cos(mainCamera.Rotation.Z);
                float angleX = angleXnoZ * cosZ + angleYnoZ * sinZ;
                float angleY = angleXnoZ * sinZ - angleYnoZ * cosZ;
                float finalAngleY = mainCamera.Rotation.X + angleY;
                finalAngleY = (float)Math.Max(finalAngleY, -Math.PI / 2);
                finalAngleY = (float)Math.Min(finalAngleY, Math.PI / 2);
                finalAngleY -= mainCamera.Rotation.X;

                mainCamera.Rotation += new Vec3(finalAngleY, -angleX, 0);
            }
        }
        public override void Keys()
        {
            Transformacja t = Transformacja.Identity.Rotate(new Vec3(0, mainCamera.Rotation.Y, 0));

            foreach (var key in pressedKeys)
            {
                switch (key)
                {
                    case Keyboard.Key.W:
                        mainCamera.Position += t * new Vec3(0, 0, Opcje.Instance.SzybkoscPoruszania * deltaTime.AsSeconds());
                        break;
                    case Keyboard.Key.S:
                        mainCamera.Position += t * new Vec3(0, 0, -Opcje.Instance.SzybkoscPoruszania * deltaTime.AsSeconds());
                        break;
                    case Keyboard.Key.A:
                        mainCamera.Position += t * new Vec3(Opcje.Instance.SzybkoscPoruszania * deltaTime.AsSeconds(), 0, 0);
                        break;
                    case Keyboard.Key.D:
                        mainCamera.Position += t * new Vec3(-Opcje.Instance.SzybkoscPoruszania * deltaTime.AsSeconds(), 0, 0);
                        break;
                    case Keyboard.Key.LShift:
                        mainCamera.Position += t * new Vec3(0, -Opcje.Instance.SzybkoscPoruszania * deltaTime.AsSeconds(), 0);
                        break;
                    case Keyboard.Key.Space:
                        mainCamera.Position += t * new Vec3(0, Opcje.Instance.SzybkoscPoruszania * deltaTime.AsSeconds(), 0);
                        break;
                }
            }
        }
    }
}
