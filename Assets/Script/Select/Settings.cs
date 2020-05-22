using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
	public GameObject selectCanvas;
	public Text level;
	public static int playerLevel = 0;
	public Slider speed;
	public Slider volume;
	public Toggle auto;
	public Text spdText, volText;
	public GameObject canvas;
	public static float vol;
	private int skinIndex;
	public GameObject[] btns = new GameObject[4];
	public Toggle[] skins = new Toggle[2];
	private readonly Dictionary<int, string> skinScene = new Dictionary<int, string>();
	public GameObject exmode;
	public GameObject album;
	public static Settings _instance;
	void Start()
	{
#if UNITY_EDITOR
		playerLevel = 2;
#endif
		_instance = this;
		if (playerLevel < 2)
		{
			exmode.SetActive(false);
			album.SetActive(false);
		}
		else
		{
			exmode.SetActive(true);
			album.SetActive(true);
		}
		auto.isOn = false;
		canvas.SetActive(false);
		level.text = "Level : " + playerLevel.ToString();
		skinScene.Add(0, "Play 1");
		skinScene.Add(1, "Play");
	}
	void Update()
	{
        if (Input.GetKeyDown(KeyCode.U))
        {
			playerLevel = 2;
			Transition.scene = "Select";
			SceneManager.LoadScene("Transition");
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#endif
		}
		UpdateText();
		GetSkin();
	}
	public void SaveSettings()
	{
		selectCanvas.SetActive(true);
		PlayerPrefs.SetInt("Speed", (int)speed.value);
		PlayerPrefs.SetFloat("Volume", volume.value);
		PlayerPrefs.SetInt("Skin", skinIndex);
		PlayerPrefs.Save();
	}

	public void GetSettings()
	{
		selectCanvas.SetActive(false);
		speed.value = PlayerPrefs.GetInt("Speed", 6);
		volume.value = PlayerPrefs.GetFloat("Volume", 1.2f);
		skinIndex = PlayerPrefs.GetInt("Skin", 0);
		skins[skinIndex].isOn = true;
	}
	private void UpdateText()
	{
		spdText.text = ((int)speed.value).ToString();
		NoteController.noteDropTime = -0.07f * ((int)speed.value - 6) + 0.65f;
		volText.text = volume.value.ToString();
		vol = 0.55f * volume.value;
		ReadSong.isAutoPlay = auto.isOn;
	}
	public void CloseSetting()
	{
		foreach (GameObject g in btns)
		{
			Image image = g.GetComponent<Image>();
			if (image != null)
			{
				image.enabled = true;
			}
		}
		canvas.SetActive(false);
	}
	public void EnterSetting()
	{
		foreach (GameObject g in btns)
		{
			Image image = g.GetComponent<Image>();
			if (image != null)
			{
				image.enabled = false;
			}
		}
		canvas.SetActive(true);
	}
	public void GetSkin()
	{
		for (int i = 0; i < skins.Length; i++)
		{
			if (skins[i].isOn)
			{
				LoadScene.scene = skinScene[i];
				skinIndex = i;
			}
		}
	}
}
