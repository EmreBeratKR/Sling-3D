using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class TextSwitcherBehaviour : PlayableBehaviour
{
    public Color color = Color.white;
    public float fontSize = 5f;
    public FontStyles fontStyles = FontStyles.Bold;
    [ResizableTextArea]
    public string text;
}
