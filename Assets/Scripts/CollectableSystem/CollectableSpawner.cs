using UnityEngine;
using Random = UnityEngine.Random;

namespace CollectableSystem
{
    public class CollectableSpawner : MonoBehaviour
    {
        [SerializeField] private LayerMask blockingLayer;
        [SerializeField] private Transform prefab;
        [SerializeField] private float radius;
        [SerializeField] private int totalCount;
        [SerializeField] private int simultaneousCount;


        private Collider m_SpawnCollider;
        private int m_CountLeft;


        private void Awake()
        {
            m_CountLeft = totalCount;
            m_SpawnCollider = GetComponent<Collider>();

            Spawn(simultaneousCount);
        }


        public void Spawn(int count)
        {
            count = Mathf.Min(count, m_CountLeft);
            
            for (var i = 0; i < count; i++)
            {
                var position = GetSpawnPosition();
                var obj = Instantiate(prefab, transform);
                obj.position = position;
                m_CountLeft -= 1;
            }
        }
        
        
        private Vector3 GetSpawnPosition()
        {
            const int maxIteration = 1000;
            
            var bounds = m_SpawnCollider.bounds;

            var i = 0;
            
            while (true)
            {
                i += 1;
                
                if (i > maxIteration) return Vector3.zero;
                
                var position = GetRandomPositionInBounds(bounds);
                
                if (Physics.CheckSphere(position, radius, blockingLayer)) continue;

                return position;
            }
        }

        
        private static Vector3 GetRandomPositionInBounds(Bounds bounds)
        {
            var min = bounds.min;
            var max = bounds.max;
            var x = Random.Range(min.x, max.x);
            var y = Random.Range(min.y, max.y);
            var z = Random.Range(min.z, max.z);

            return new Vector3(x, y, z);
        }
    }
}