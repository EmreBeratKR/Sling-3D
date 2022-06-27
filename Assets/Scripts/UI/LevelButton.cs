using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private Image icon;


        public void UpdateIcon(Sprite sprite)
        {
            icon.sprite = sprite;
        }

        public void Disable()
        {
            icon.gameObject.SetActive(false);
        }
        
        
        public int Index => transform.GetSiblingIndex();
    }
}
