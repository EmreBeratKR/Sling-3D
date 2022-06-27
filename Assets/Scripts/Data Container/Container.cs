using UnityEngine;

namespace Data_Container
{
    public abstract class Container<T> : ScriptableObject
    {
        [SerializeField] private T[] elements;


        public T this[int index] => elements[index];
    
        public T Random => elements[UnityEngine.Random.Range(0, elements.Length)];
    }
}