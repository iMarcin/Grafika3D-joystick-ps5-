using SFML.Graphics;
using SFML.System;

namespace grafika3d
{
    public class Indykator :Transformowalne,Drawable
    {

        public void Draw(RenderTarget target, RenderStates states)
        {
            Okno window = (Okno)target;
            Vec3 vX = new Vec3(-20, 0, 0);
            Vec3 vY = new Vec3(0, -20, 0);
            Vec3 vZ = new Vec3(0, 0, -20);
            Vec3 cameraCenter = new Vec3(target.Size.X / 2, target.Size.Y / 2, 0);

            Kamera sceneCamera = window.sceny.Peek().mainCamera;
            Position = sceneCamera.Position;
            Transformacja t = sceneCamera.InverseTransform * Transform;
            t.Translate(cameraCenter);
            vX = t * vX;
            vY = t * vY;
            vZ = t * vZ;
            Vertex[] vs = new Vertex[]
            {
                new Vertex(new Vector2f(vX.X,vX.Y),Color.Red),
                new Vertex(new Vector2f(cameraCenter.X,cameraCenter.Y),Color.Red),
                new Vertex(new Vector2f(vY.X,vY.Y),Color.Green),
                new Vertex(new Vector2f(cameraCenter.X,cameraCenter.Y),Color.Green),
                new Vertex(new Vector2f(vZ.X,vZ.Y),Color.Blue),
                new Vertex(new Vector2f(cameraCenter.X,cameraCenter.Y),Color.Blue),
            };
            target.Draw(vs, PrimitiveType.Lines);
        }
    }
}
