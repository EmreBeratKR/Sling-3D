using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

[Serializable]
public class TextSwitcherBehaviour : PlayableBehaviour
{
    public Color color = Color.white;
    public float fontSize = 5f;
    [ResizableTextArea]
    public string text;
}
