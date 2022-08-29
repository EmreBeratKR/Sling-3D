using UnityEngine;

namespace TubeSystem
{
    public class TubeEntrance : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Tube tube;


        public bool TryEnter()
        {
            return tube.TryEnter();
        }
    }
}
