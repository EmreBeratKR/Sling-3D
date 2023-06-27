using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private Image icon;


        private void Awake()
        {
            var shadow = icon.gameObject.AddComponent<Shadow>();
            shadow.effectDistance = new Vector2(-10.5f, -10.5f);
        }


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
