using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct DialogueSentence
{
    public string name;
    public Sprite image;
    [TextArea(3, 10)]
    public string text;
}

[System.Serializable]
public class Dialogue
{
    public string id;
    public DialogueSentence[] sentences;
}
