using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Song
{
    private string name;
    private string artist;
    private string path;
    private int ez;
    private int adv;
    private int hd;
    private int ex;
    private int unlockLevel;

    public Song(string name, string artist, string path, int ez, int adv, int hd, int ex, int unlockLevel)
    {
        this.name = name;
        this.artist = artist;
        this.path = path;
        this.ez = ez;
        this.adv = adv;
        this.hd = hd;
        this.ex = ex;
        this.unlockLevel = unlockLevel;
    }

    public string Name { get => name; set => name = value; }
    public string Artist { get => artist; set => artist = value; }
    public int Ez { get => ez; set => ez = value; }
    public int Adv { get => adv; set => adv = value; }
    public int Hd { get => hd; set => hd = value; }
    public int Ex { get => ex; set => ex = value; }
    public string Path { get => path; set => path = value; }
    public int UnlockLevel { get => unlockLevel; set => unlockLevel = value; }
}
