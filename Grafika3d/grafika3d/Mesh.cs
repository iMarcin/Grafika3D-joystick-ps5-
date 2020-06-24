using System.Collections.Generic;
using System.Collections;

namespace grafika3d
{
    //operacje na trojkatach
    public class Mesh : Transformowalne, IEnumerable<Tri>, Rysowalne
    {
        public List<Tri> Trojkaty { get; set; } = new List<Tri>();

        public void Add(Tri t)
        {
            Trojkaty.Add(t);
        }

        public IEnumerator<Tri> GetEnumerator()
        {
            foreach (var t in Trojkaty)
                yield return t;
        }

        public Mesh GetMesh()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
