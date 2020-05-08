using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class AlbumController : MonoBehaviour
{
    public Transform Content;
    public GameObject album;
    public PlayableDirector mask;
    public static AlbumController _instance;
    public static bool isEX;

    private void Start()
    {
        _instance = this;
        ShowList();
        //if (Settings.playerLevel < 2)
        //{
        //    Settings.playerLevel++;
        //}
    }
    public void ReturnScene()
    {
        PlayMask();
        Invoke("Load", 0.65f);
    }
    public void PlayMask()
    {
        mask.Play();
    }
    private void Load()
    {
        if (isEX)
        {
            mask.Play();
            Transition.scene = "SelectEX";
            SceneManager.LoadScene("Transition");
        }
        else
        {
            mask.Play();
            Transition.scene = "Select";
            SceneManager.LoadScene("Transition");
        }
    }
    private void ShowList()
    {
        foreach (Song s in List.songList)
        {
            GameObject button = Instantiate(album) as GameObject;
            button.transform.SetParent(Content, false);
            button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            button.GetComponent<AlbumButton>().songName.text = s.Name;
            button.GetComponent<AlbumButton>().songArtist.text = s.Artist;
            button.GetComponent<AlbumButton>().song = s;
        }
    }
}
