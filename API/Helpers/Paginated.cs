using System.Collections.Generic;

namespace API.Helpers
{
    public class Paginated<T> where T : class
    {
        public Paginated(int size, int index, int count, IReadOnlyList<T> data)
        {
            Size = size;
            Index = index;
            Count = count;
            Data = data;
        }

        public int Size { get; set; }
        public int Index { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
    }
}