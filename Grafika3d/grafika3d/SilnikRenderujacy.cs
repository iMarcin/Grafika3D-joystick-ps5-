using System;
using System.Collections.Generic;
using SFML.Graphics;
using System.Linq;

namespace grafika3d
{
    //baza silnika renderujacego z pomoca tutorialu na youtube
    class SilnikRenderujacy
    {
        public static SilnikRenderujacy Instance { get; } = new SilnikRenderujacy();
        private SilnikRenderujacy()
        {
        }

        private float[,] Buffer { get; set; }
        private Color[,] Bitmap { get; set; }

        private Wierzcholek Linia(Vec3 planePoint, Vec3 planeNormal, Wierzcholek lineStart, Wierzcholek lineEnd)
        {
            planeNormal = planeNormal.Normal();

            float plane_d = -planeNormal.Dot(planePoint);
            float ad = lineStart.Position.Dot(planeNormal);
            float bd = lineEnd.Position.Dot(planeNormal);
            float t = (-plane_d - ad) / (bd - ad);
            Vec3 lineStartToEnd = lineEnd.Position - lineStart.Position;
            Vec4 colorStartToEnd = lineEnd.Color - lineStart.Color;
            Vec3 lineToIntersect = lineStartToEnd * t;
            Vec4 colorToIntersect = colorStartToEnd * t;
            return new Wierzcholek(lineStart.Position + lineToIntersect, lineStart.Color + colorToIntersect);
        }

        private List<Tri> Clipowanie(Vec3 planePoint, Vec3 planeNormal, Tri tri)
        {
            planeNormal = planeNormal.Normal();

            float dist(Vec3 point)
            {
                return planeNormal.Dot(point) - planeNormal.Dot(planePoint);
            }

            Wierzcholek[] insidePoints = new Wierzcholek[3]; int nInsidePointCount = 0;
            Wierzcholek[] outsidePoints = new Wierzcholek[3]; int nOutsidePointCount = 0;

            float d0 = dist(tri[0].Position);
            float d1 = dist(tri[1].Position);
            float d2 = dist(tri[2].Position);

            if (d0 >= 0) { insidePoints[nInsidePointCount++] = tri[0]; }
            else { outsidePoints[nOutsidePointCount++] = tri[0]; }
            if (d1 >= 0) { insidePoints[nInsidePointCount++] = tri[1]; }
            else { outsidePoints[nOutsidePointCount++] = tri[1]; }
            if (d2 >= 0) { insidePoints[nInsidePointCount++] = tri[2]; }
            else { outsidePoints[nOutsidePointCount++] = tri[2]; }

            if (nInsidePointCount == 0) return new List<Tri>();
            if (nInsidePointCount == 1 && nOutsidePointCount == 2)
            {
                Wierzcholek v0 = insidePoints[0];
                Wierzcholek v1 = Linia(planePoint, planeNormal, v0, outsidePoints[0]);
                Wierzcholek v2 = Linia(planePoint, planeNormal, v0, outsidePoints[1]);

                Tri t = new Tri(tri) { W0 = v0, W1 = v1, W2 = v2 };
                if (tri.NormalVector.Z * t.NormalVector.Z < 0)
                {
                    Wierzcholek tmp = t[1];
                    t[1] = t[2];
                    t[2] = tmp;
                }
                return new List<Tri>
                {
                    t,
                };
            }
            if (nInsidePointCount == 2 && nOutsidePointCount == 1)
            {
                Wierzcholek v00 = insidePoints[0];
                Wierzcholek v01 = insidePoints[1];
                Wierzcholek v02 = Linia(planePoint, planeNormal, v00, outsidePoints[0]);

                Wierzcholek v10 = insidePoints[1];
                Wierzcholek v11 = v02;
                Wierzcholek v12 = Linia(planePoint, planeNormal, v10, outsidePoints[0]);

                Tri t1 = new Tri(tri) { W0 = v00, W1 = v01, W2 = v02 };
                Tri t2 = new Tri(tri) { W0 = v10, W1 = v11, W2 = v12 };
                if (tri.NormalVector.Z * t1.NormalVector.Z < 0)
                {
                    Wierzcholek tmp = t1[1];
                    t1[1] = t1[2];
                    t1[2] = tmp;
                }
                if (tri.NormalVector.Z * t2.NormalVector.Z < 0)
                {
                    Wierzcholek tmp = t2[1];
                    t2[1] = t2[2];
                    t2[2] = tmp;
                }
                return new List<Tri>
                {
                    t1,
                    t2,
                };
            }
            if (nInsidePointCount == 3)
            {
                return new List<Tri>
                {
                    tri,
                };
            }
            return new List<Tri>();
        }
        //rysowanie trojkatow skierowanych do mnie z cieniem i przycieciem
        public void RenderowanieSceny(Scena scene, RenderTarget target, RenderStates states)
        {
            uint width = target.Size.X;
            uint height = target.Size.Y;
            Buffer = new float[width, height];
            Bitmap = new Color[width, height];

            Transformacja cameraInvMatrixAndWorld = scene.mainCamera.InverseTransform * scene.worldTransform;
            Transformacja matView = Transformacja.Identity.Translate(new Vec3(1, 1, 0)).Scale(new Vec3(width / 2, height / 2, 1));

            float fAspectRatio = height / (float)width;
            float fFovRad = 1.0f / (float)Math.Tan(scene.mainCamera.Fov * 0.5f / 180.0f * Math.PI);
            float[,] m = new float[4, 4];
            m[0, 0] = fAspectRatio * fFovRad;
            m[1, 1] = fFovRad;
            m[2, 2] = scene.mainCamera.Daleko / (scene.mainCamera.Daleko - scene.mainCamera.Blisko);
            m[3, 2] = (-scene.mainCamera.Daleko * scene.mainCamera.Blisko) / (scene.mainCamera.Daleko - scene.mainCamera.Blisko);
            m[2, 3] = 1.0f;
            m[3, 3] = 0.0f;
            Transformacja matProj = new Transformacja(m);

            Transformacja matProjAndView = matView * matProj;

            Queue<Tri> sceneTriangles = new Queue<Tri>();
            foreach (Rysowalne drawable in scene.drawables)
            {
                Mesh mesh = drawable.GetMesh();
                foreach (Tri tri in mesh)
                {
                    sceneTriangles.Enqueue(mesh.Transform * tri);
                }
            }
            int c = sceneTriangles.Count;
            for (int i = 0; i < c; i++)
            {
                Tri t = sceneTriangles.Dequeue();
                sceneTriangles.Enqueue(cameraInvMatrixAndWorld * t);
            }
            for (int i = 0; i < c; i++)
            {
                Tri t = sceneTriangles.Dequeue();
                List<Tri> clt = Clipowanie(new Vec3(0, 0, scene.mainCamera.Blisko), new Vec3(0, 0, 1), t);
                foreach (var clippedTriangle in clt)
                {
                    sceneTriangles.Enqueue(clippedTriangle);
                }
            }
            c = sceneTriangles.Count;
            for (int i = 0; i < c; i++)
            {
                Tri t = sceneTriangles.Dequeue();
                if (t.NormalVector.Dot(t[0].Position) < 0)
                    sceneTriangles.Enqueue(t);
            }
            c = sceneTriangles.Count;
            for (int i = 0; i < c; i++)
            {
                Tri t = sceneTriangles.Dequeue();
                float[] Is = new float[3] { 0, 0, 0 };
                Vec3 N = t.NormalVector;
                float kd = t.KD;
                float ks = t.KS;
                float n = t.N;
                foreach (ZrodloSwiatla lsrc in scene.Lampy)
                {
                    Vec3 lightPos = cameraInvMatrixAndWorld * lsrc.Position;
                    for (int j = 0; j < 3; j++)
                    {
                        Vec3 V = (-t[j].Position).Normal();
                        Vec3 L = (lightPos - t[j].Position).Normal();
                        Vec3 R = (-L - 2 * N.Dot(-L) * N).Normal();
                        float minus = (V.Dot(R) < 0) ? -1 : 1;
                        float I = lsrc.Intensywnosc * (kd * N.Dot(L) + ks * minus * (float)Math.Pow(V.Dot(R), n));
                        Math.Max(I, 0);
                        Is[j] += I;
                    }
                }
                for (int j = 0; j < 3; j++)
                {
                    Is[j] = Math.Max(Is[j], 0.2f);
                }
                Vec4 color0 = new Vec4(t[0].Color.R * Is[0], t[0].Color.G * Is[0], t[0].Color.B * Is[0], t[0].Color.A);
                Vec4 color1 = new Vec4(t[1].Color.R * Is[1], t[1].Color.G * Is[1], t[1].Color.B * Is[1], t[1].Color.A);
                Vec4 color2 = new Vec4(t[2].Color.R * Is[2], t[2].Color.G * Is[2], t[2].Color.B * Is[2], t[2].Color.A);

                Wierzcholek vert0 = new Wierzcholek(t[0].Position, color0);
                Wierzcholek vert1 = new Wierzcholek(t[1].Position, color1);
                Wierzcholek vert2 = new Wierzcholek(t[2].Position, color2);
                sceneTriangles.Enqueue(new Tri(vert0, vert1, vert2, t.KS, t.KD, t.N));
            }
            for (int i = 0; i < c; i++)
            {
                Tri t = sceneTriangles.Dequeue();
                sceneTriangles.Enqueue(matProjAndView * t);
            }
            for (int i = 0; i < c; i++)
            {
                Tri t = sceneTriangles.Dequeue();
                Queue<Tri> qTris = new Queue<Tri>();
                qTris.Enqueue(t);
                int nNewTriangles = 1;

                for (int p = 0; p < 4; p++)
                {
                    while (nNewTriangles > 0)
                    {
                        Tri t2 = qTris.Dequeue();
                        nNewTriangles--;

                        List<Tri> clippedTriangles = new List<Tri>();
                        switch (p)
                        {
                            case 0:
                                clippedTriangles = Clipowanie(new Vec3(), new Vec3(0, 1, 0), t2);
                                break;
                            case 1:
                                clippedTriangles = Clipowanie(new Vec3(0, height - 1, 0), new Vec3(0, -1, 0), t2);
                                break;
                            case 2:
                                clippedTriangles = Clipowanie(new Vec3(), new Vec3(1, 0, 0), t2);
                                break;
                            case 3:
                                clippedTriangles = Clipowanie(new Vec3(width - 1, 0, 0), new Vec3(-1, 0, 0), t2);
                                break;

                        }
                        foreach (var item in clippedTriangles)
                        {
                            qTris.Enqueue(item);
                        }
                    }
                    nNewTriangles = qTris.Count;
                }
                foreach (var item in qTris)
                {
                    sceneTriangles.Enqueue(item);
                }
            }
            while (sceneTriangles.Count > 0)
            {
                Tri t = sceneTriangles.Dequeue();
                RysujTrojkat(t, PrimitiveType.Triangles);
            }
            foreach (ZrodloSwiatla light in scene.Lampy)
            {
                Vec3 lpos = cameraInvMatrixAndWorld * light.Position;
                if (lpos.Z > 0)
                {
                    Vec3 lPosOnScreen = matProjAndView * lpos;
                    UstawPixel(lPosOnScreen, (Vec4)Color.White);
                    UstawPixel(lPosOnScreen + new Vec3(0, 1), (Vec4)Color.White);
                    UstawPixel(lPosOnScreen + new Vec3(1, 0), (Vec4)Color.White);
                    UstawPixel(lPosOnScreen + new Vec3(1, 1), (Vec4)Color.White);
                }
            }
            Image img = new Image(Bitmap);
            Texture texture = new Texture(img);
            Sprite s = new Sprite(texture);
            target.Draw(s, states);
            s.Dispose();
            texture.Dispose();
            img.Dispose();
        }
        private void RysujTrojkat(Tri triangle, PrimitiveType primitiveType)
        {
            if (primitiveType == PrimitiveType.Triangles)
            {
                void uzupelnijdol(Wierzcholek v1, Wierzcholek v2, Wierzcholek v3)
                {
                    float invslope1 = (v2.Position.X - v1.Position.X) / (v2.Position.Y - v1.Position.Y);
                    float invslope2 = (v3.Position.X - v1.Position.X) / (v3.Position.Y - v1.Position.Y);

                    float curx1 = v1.Position.X;
                    float curx2 = v1.Position.X;

                    for (int scanlineY = (int)v1.Position.Y; scanlineY <= v2.Position.Y; scanlineY++)
                    {
                        float z1 = v1.Position.Z + ((float)(scanlineY - v1.Position.Y) / (float)(v2.Position.Y - v1.Position.Y)) * (v2.Position.Z - v1.Position.Z);
                        float z2 = v1.Position.Z + ((float)(scanlineY - v1.Position.Y) / (float)(v3.Position.Y - v1.Position.Y)) * (v3.Position.Z - v1.Position.Z);
                        Vec4 c1 = v1.Color + ((float)(scanlineY - v1.Position.Y) / (float)(v2.Position.Y - v1.Position.Y)) * (v2.Color - v1.Color);
                        Vec4 c2 = v1.Color + ((float)(scanlineY - v1.Position.Y) / (float)(v3.Position.Y - v1.Position.Y)) * (v3.Color - v1.Color);
                        Vec3 from = new Vec3((float)Math.Round(curx1), scanlineY, z1);
                        Vec3 to = new Vec3((float)Math.Round(curx2), scanlineY, z2);
                        UstawLinie(from.X, from.Z, c1, to.X, to.Z, c2, scanlineY);
                        curx1 += invslope1;
                        curx2 += invslope2;
                    }
                }

                void uzupelnijgore(Wierzcholek v1, Wierzcholek v2, Wierzcholek v3)
                {
                    float invslope1 = (v3.Position.X - v1.Position.X) / (v3.Position.Y - v1.Position.Y);
                    float invslope2 = (v3.Position.X - v2.Position.X) / (v3.Position.Y - v2.Position.Y);

                    float curx1 = v3.Position.X;
                    float curx2 = v3.Position.X;

                    for (int scanlineY = (int)v3.Position.Y; scanlineY > v1.Position.Y; scanlineY--)
                    {
                        float z1 = v3.Position.Z + ((float)(scanlineY - v3.Position.Y) / (float)(v1.Position.Y - v3.Position.Y)) * (v1.Position.Z - v3.Position.Z);
                        float z2 = v3.Position.Z + ((float)(scanlineY - v3.Position.Y) / (float)(v2.Position.Y - v3.Position.Y)) * (v2.Position.Z - v3.Position.Z);
                        Vec3 from = new Vec3((float)Math.Round(curx1), scanlineY, z1);
                        Vec3 to = new Vec3((float)Math.Round(curx2), scanlineY, z2);
                        Vec4 c1 = v3.Color + ((float)(scanlineY - v3.Position.Y) / (float)(v1.Position.Y - v3.Position.Y)) * (v1.Color - v3.Color);
                        Vec4 c2 = v3.Color + ((float)(scanlineY - v3.Position.Y) / (float)(v2.Position.Y - v3.Position.Y)) * (v2.Color - v3.Color);
                        UstawLinie(from.X, from.Z, c1, to.X, to.Z, c2, scanlineY);
                        curx1 -= invslope1;
                        curx2 -= invslope2;
                    }
                }

                List<Wierzcholek> sortedVecs = new List<Wierzcholek>
                {
                    triangle[0],
                    triangle[1],
                    triangle[2],
                };
                sortedVecs = sortedVecs.OrderBy(i => i.Position.Y).ToList();
                Wierzcholek A = sortedVecs[0];
                Wierzcholek B = sortedVecs[1];
                Wierzcholek C = sortedVecs[2];
                A.Position = new Vec3((int)A.Position.X, (int)A.Position.Y, A.Position.Z);
                B.Position = new Vec3((int)B.Position.X, (int)B.Position.Y, B.Position.Z);
                C.Position = new Vec3((int)C.Position.X, (int)C.Position.Y, C.Position.Z);

                if (B.Position.Y == C.Position.Y)
                {
                    uzupelnijdol(A, B, C);
                }
                else if (A.Position.Y == B.Position.Y)
                {
                    uzupelnijgore(A, B, C);
                }
                else
                {
                    float z = A.Position.Z + ((float)(B.Position.Y - A.Position.Y) / (float)(C.Position.Y - A.Position.Y)) * (C.Position.Z - A.Position.Z);
                    Vec4 c = A.Color + ((float)(B.Position.Y - A.Position.Y) / (float)(C.Position.Y - A.Position.Y)) * (C.Color - A.Color);
                    Vec3 v4 = new Vec3(
                      (int)(A.Position.X + ((float)(B.Position.Y - A.Position.Y) / (float)(C.Position.Y - A.Position.Y)) * (C.Position.X - A.Position.X)),
                      B.Position.Y,
                      z
                      );
                    Wierzcholek v = new Wierzcholek(v4, c);

                    uzupelnijdol(A, B, v);
                    uzupelnijgore(B, v, C);
                }


            }
        }

        private void UstawPixel(Vec3 pixel, Vec4 color)
        {
            int screenX = (int)pixel.X;
            int screenY = (int)pixel.Y;
            if (screenX < 0 || screenX >= Bitmap.GetLength(0)) return;
            if (screenY < 0 || screenY >= Bitmap.GetLength(1)) return;
            if (pixel.Z <= Buffer[screenX, screenY])
            {
                Buffer[screenX, screenY] = pixel.Z;
                Bitmap[screenX, screenY] = (Color)color;
            }
        }

        private void UstawPixel(Vec2 pixel, float z, Vec4 color)
        {
            UstawPixel(new Vec3(pixel.X, pixel.Y, z), color);
        }
        private void UstawLinie(float fromX, float fromZ, Vec4 color0, float toX, float toZ, Vec4 color1, int y)
        {
            static void zamianaInt(ref int o1, ref int o2)
            {
                int tmp = o1;
                o1 = o2;
                o2 = tmp;
            }
            static void zamianaFloat(ref float o1, ref float o2)
            {
                float tmp = o1;
                o1 = o2;
                o2 = tmp;
            }
            static void zamianaKolor(ref Vec4 o1, ref Vec4 o2)
            {
                Vec4 tmp = o1;
                o1 = o2;
                o2 = tmp;
            }
            int rFromX = (int)Math.Round(fromX);
            int rToX = (int)Math.Round(toX);

            if (rFromX > rToX)
            {
                zamianaInt(ref rFromX, ref rToX);
                zamianaFloat(ref fromZ, ref toZ);
                zamianaKolor(ref color0, ref color1);
            }
            for (int x = rFromX; x <= rToX; x++)
            {
                Vec2 v = new Vec2(x, y);
                float t = (rFromX == rToX) ? 0.5f : (x - rFromX) / (float)(rToX - rFromX);
                Vec4 c = (1 - t) * color0 + t * color1;
                float z = (1 - t) * fromZ + t * toZ;
                UstawPixel(v, z, c);
            }
        }
    }
}
