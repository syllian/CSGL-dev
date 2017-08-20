using System.Collections;

namespace CSGL
{
    public class Array<T>
    {
        private ArrayList list;

        public Array()
        {
            list = new ArrayList();
        }

        public void add(T element)
        {
            list.Add(element);
        }

        public T get(int index)
        {
            return (T)list[index];
        }

        public int size()
        {
            return list.Count;
        }
    }
}
