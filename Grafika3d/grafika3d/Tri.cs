using System;
using System.Collections.Generic;
using System.Collections;

namespace grafika3d
{
    //operacje na trojkatach
    public struct Tri : IEnumerable<Wierzcholek>
    {
        public Wierzcholek W0 { get; set; }
        public Wierzcholek W1 { get; set; }
        public Wierzcholek W2 { get; set; }

        public float KD { get; set; }
        public float KS { get; set; }
        public float N { get; set; }


        public Vec3 NormalVector
        {
            get
            {
                Vec3 l0 = W1.Position - W0.Position;
                Vec3 l1 = W2.Position - W0.Position;
                return l0.Cross(l1).Normal();
            }
        }
        public Vec3 Center
        {
            get
            {
                return (W0.Position + W1.Position + W2.Position) / 3;
            }
        }

        public Wierzcholek this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return W0;
                    case 1: return W1;
                    case 2: return W2;
                    default: throw new IndexOutOfRangeException("index must be 0, 1 or 2");
                }
            }
            set
            {
                switch (index)
                {
                    case 0: W0 = value; break;
                    case 1: W1 = value; break;
                    case 2: W2 = value; break;
                    default: throw new IndexOutOfRangeException("index must be 0, 1 or 2");
                }
            }
        }

        public Tri(Wierzcholek v0, Wierzcholek v1, Wierzcholek v2, float ks = 1, float kd = 1, float n = 10)
        {
            this.W0 = v0;
            this.W1 = v1;
            this.W2 = v2;
            this.KS = ks;
            this.KD = kd;
            this.N = n;
        }
        public Tri(Tri t)
        {
            W0 = t.W0;
            W1 = t.W1;
            W2 = t.W2;
            KS = t.KS;
            KD = t.KD;
            N = t.N;
        }
        public Tri(Vec3 v0, Vec3 v1, Vec3 v2, Vec4 color)
            : this(new Wierzcholek(v0, color), new Wierzcholek(v1, color), new Wierzcholek(v2, color))
        {

        }
        public IEnumerator<Wierzcholek> GetEnumerator()
        {
            yield return W0;
            yield return W1;
            yield return W2;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
