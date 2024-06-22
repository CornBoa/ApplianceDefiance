using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue {	
	public Sentence[] sentences;
}
[System.Serializable]
public class Sentence
{
    [TextArea(3, 10)]
    public string text;
    public bool rightLeftSide = false; //Right = false,Left = true
    public bool choice = false;
    public Sprite portrait;
    public override string ToString()
    {
        return text;
    }
}