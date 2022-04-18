using System;
using System.Collections.Generic;

namespace ProjectGame.Utils
{
    public class ObjectPool<T> where T : class
    {
        private readonly Stack<T> _pool;
        private readonly Func<T> _factory;
        private readonly Action<T> _onRelease;
        private readonly Action<T> _onClear;

        public ObjectPool(Func<T> factory, Action<T> onRelease = null, Action<T> onClear = null)
        {
            _pool = new Stack<T>();
            _factory = factory;
            _onRelease = onRelease;
            _onClear = onClear;
        }

        public T Get()
        {
            return _pool.Count > 0 ? _pool.Pop() : _factory();
        }

        public void Release(T obj)
        {
            _onRelease?.Invoke(obj);
            _pool.Push(obj);
        }

        public void Clear()
        {
            if (_onClear == null) 
            {
                _pool.Clear();
                return;
            }
            while (_pool.Count > 0)
                _onClear.Invoke(_pool.Pop());
        }
    }
}
