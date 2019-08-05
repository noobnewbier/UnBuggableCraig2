using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Assets/DialogueData")]
public class DialogueData : ScriptableObject
{
    [SerializeField] string[] _dialogues;
    public string[] Dialogues { get { return _dialogues; } }
}
