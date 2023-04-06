using System.Collections.Generic;

namespace DefaultNamespace
{
    public class ItemPool<T>
    {
        private readonly Queue<T> _itemQueue;
        public bool HasItems => _itemQueue.Count > 0;

        public ItemPool(int poolSize = 4)
        {
            _itemQueue = new Queue<T>(poolSize);
        }

        public T GetItem()
        {
            return _itemQueue.Count > 0 ? _itemQueue.Dequeue() : default(T);
        }
        public void PushItem(T item){
            _itemQueue.Enqueue(item);
        }
    }
}