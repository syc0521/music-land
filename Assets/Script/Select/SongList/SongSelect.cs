using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ReadList;

public class SongSelect : MonoBehaviour
{
    /// <summary>
    /// 文本
    /// </summary>
    public Text text;
    /// <summary>
    /// 歌曲
    /// </summary>
    public Song song;
    /// <summary>
    /// 切换歌曲
    /// </summary>
    public void SwitchSong()
    {
        diff[0] = song.Ez;
        diff[1] = song.Adv;
        diff[2] = song.Hd;
        SelectedSong = song;
        _instance.PlayAudio(song);
    }

}
