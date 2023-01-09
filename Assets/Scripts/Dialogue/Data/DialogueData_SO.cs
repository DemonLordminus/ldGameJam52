using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DialogueData_SO",menuName ="Dialogue/DialogueData_SO")]
public class DialogueData_SO : ScriptableObject
{
    [TextArea]
    public List<string> dialogueList;
}
