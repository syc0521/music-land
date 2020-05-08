using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ReadList;

public class SongSelect : MonoBehaviour
{
    public Text text;
    public Song song;
    public void SwitchSong()
    {
        diff[0] = song.Ez;
        diff[1] = song.Adv;
        diff[2] = song.Hd;
        SelectedSong = song;
        _instance.PlayAudio(song);
    }

}
