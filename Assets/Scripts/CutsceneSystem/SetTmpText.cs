using TMPro;
using UnityEngine;

namespace CutsceneSystem
{
    public class SetTmpText : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmpText;
        [SerializeField, Multiline] private string text;
        [SerializeField] private int fontSize = 5;


        public void Set()
        {
            tmpText.text = text;
            tmpText.fontSize = fontSize;
        }
    }
}