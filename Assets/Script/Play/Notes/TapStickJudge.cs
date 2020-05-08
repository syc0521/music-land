using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapStickJudge : MonoBehaviour
{
    public bool Judge0(NoteAsset note)
    {
        if (Input.GetAxisRaw("LeftRight") == -1 && note.CanJudge)
            return true;
        return false;
    }
    public bool Judge1(NoteAsset note)
    {
        if (Input.GetAxisRaw("UpDown") == 1 && note.CanJudge)
            return true;
        return false;
    }
    public bool Judge2(NoteAsset note)
    {
        if (Input.GetAxisRaw("LeftRight") == 1 && note.CanJudge)
            return true;
        return false;
    }
}
