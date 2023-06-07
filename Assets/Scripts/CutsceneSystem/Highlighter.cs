using UnityEngine;

namespace CutsceneSystem
{
    public class Highlighter : MonoBehaviour
    {
        private static readonly int PopUpHash = Animator.StringToHash("pop_up");
        
        
        [SerializeField] private Animator animator;


        public void PopUp()
        {
            animator.SetTrigger(PopUpHash);
        }
    }
}