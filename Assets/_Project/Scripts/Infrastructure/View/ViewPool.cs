using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.View
{
    public sealed class ViewPool
    {
        private readonly GameObject _prefab;
        private readonly Transform _root;
        private readonly Stack<GameObject> _pool = new();

        public ViewPool(GameObject prefab, Transform root)
        {
            _prefab = prefab;
            _root = root;
        }

        public GameObject Get()
        {
            if (_pool.Count <= 0) return Object.Instantiate(_prefab, _root);
            var go = _pool.Pop();
            go.SetActive(true);
            return go;
        }

        public void Release(GameObject go)
        {
            go.SetActive(false);
            go.transform.SetParent(_root);
            _pool.Push(go);
        }
    }
}