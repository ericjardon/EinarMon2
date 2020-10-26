using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Para que aparezca en el inspector de Unity
[System.Serializable]
public class DialogLines
{
    [SerializeField] List<string> lines;

    public List<string> Lines {
        get { return lines; }
    }
}
