using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ReadSong;

public class InputController : Judge
{
	public SpriteRenderer[] padEffects = new SpriteRenderer[5];
	public static int combo = 0;
	public static InputController _instance;
	public GameObject judgeObj;
	public Sprite[] judgeTypeSprite = new Sprite[5];
	public Sprite[] perfectNumbers = new Sprite[10];
	public Sprite[] greatNumbers = new Sprite[10];
	public GameObject[] judgeCombo = new GameObject[4];
	public GameObject bpmBar;
	public GameObject fastSlowObj;
	public SpriteRenderer[] lightEffect = new SpriteRenderer[5];
	public Sprite[] fastSlow = new Sprite[2];
	[NonSerialized]
	public bool isControllerConnected = false;
	private readonly Dictionary<int, KeyCode> keyValuePairs = new Dictionary<int, KeyCode>();
	void Start()
	{
		try
		{
			string[] s = Input.GetJoystickNames();
			if (s[0].Equals("Controller (Xbox One For Windows)") && s[0] != null)
			{
				isControllerConnected = true;
			}
		}
		catch (Exception)
		{
			isControllerConnected = false;
		}
		if (isControllerConnected)
		{
			keyValuePairs.Add(2, KeyCode.JoystickButton2);
			keyValuePairs.Add(3, KeyCode.JoystickButton3);
			keyValuePairs.Add(4, KeyCode.JoystickButton1);
		}
		else
		{
			keyValuePairs.Add(0, KeyCode.D);
			keyValuePairs.Add(1, KeyCode.F);
			keyValuePairs.Add(2, KeyCode.Space);
			keyValuePairs.Add(3, KeyCode.J);
			keyValuePairs.Add(4, KeyCode.K);
		}
		_instance = this;
	}
	void Update()
	{
		if (isControllerConnected)
		{
			StickLightEffect();
		}
		else
		{
			KeyPadLightEffect();
		}
		BpmBarChange(bpm);
	}
	private void StickLightEffect()
	{
		if (Input.GetAxis("LeftRight") < 0)
		{
			lightEffect[0].enabled = true;
			padEffects[0].enabled = true;
		}
		if (Input.GetAxis("UpDown") > 0)
		{
			padEffects[1].enabled = true;
			lightEffect[1].enabled = true;
		}
		if (Input.GetAxis("LeftRight") > 0 || Input.GetKey(KeyCode.JoystickButton2))
		{
			padEffects[2].enabled = true;
			lightEffect[2].enabled = true;
		}
		if (Input.GetKey(KeyCode.JoystickButton3))
		{
			padEffects[3].enabled = true;
			lightEffect[3].enabled = true;
		}
		if (Input.GetKey(KeyCode.JoystickButton1))
		{
			padEffects[4].enabled = true;
			lightEffect[4].enabled = true;
		}
		if (Input.GetAxis("LeftRight") == 0)
		{
			padEffects[0].enabled = false;
			lightEffect[0].enabled = false;
		}
		if (Input.GetAxis("UpDown") == 0)
		{
			padEffects[1].enabled = false;
			lightEffect[1].enabled = false;
		}
		if (Input.GetAxis("LeftRight") == 0)
		{
			padEffects[2].enabled = false;
			lightEffect[2].enabled = false;
			if (Input.GetKey(KeyCode.JoystickButton2))
			{
				padEffects[2].enabled = true;
				lightEffect[2].enabled = true;
			}
		}
		if (Input.GetKeyUp(KeyCode.JoystickButton3))
		{
			padEffects[3].enabled = false;
			lightEffect[3].enabled = false;
		}
		if (Input.GetKeyUp(KeyCode.JoystickButton1))
		{
			padEffects[4].enabled = false;
			lightEffect[4].enabled = false;
		}
	}
	private void KeyPadLightEffect()
	{
		for (int i = 0; i < 5; i++)
		{
			if (Input.GetKeyDown(keyValuePairs[i]))
			{
				padEffects[i].enabled = true;
				lightEffect[i].enabled = true;
			}
			if (Input.GetKeyUp(keyValuePairs[i]))
			{
				padEffects[i].enabled = false;
				lightEffect[i].enabled = false;
			}
		}
	}
	private void BpmBarChange(int bpm)
	{
		Color colorStart = Color.white;
		Color colorEnd = new Color(1, 1, 1, 0);
		float duration = 60.0f / bpm / 2;
		float lerp = Mathf.PingPong(Time.timeSinceLevelLoad + NoteController.noteDropTime - 0.07f, duration) / duration;
		Color color = Color.Lerp(colorStart, colorEnd, lerp);
		bpmBar.GetComponent<SpriteRenderer>().color = color;
	}
	public NoteAsset GetNextNote(NoteAsset note)
	{
		List<NoteAsset> noteList = NoteController._instance.noteList;
		int index = noteList.IndexOf(note);
		try
		{
			int nextIndex = noteList.FindIndex(index + 1,
			delegate (NoteAsset note1)
			{
				return note1.Pos == note.Pos;
			});
			return noteList[nextIndex];
		}
		catch (Exception)
		{
			return null;
		}

	}
	public NoteAsset GetPreviousNote(NoteAsset note)
	{
		List<NoteAsset> noteList = NoteController._instance.noteList;
		int index = noteList.IndexOf(note);
		int nextIndex = noteList.FindLastIndex(index,
			delegate (NoteAsset note1)
			{
				return note1.Pos == note.Pos;
			});
		return noteList[nextIndex];
	}
	public void ShowCombo(JudgeType type)
	{
		int typeConverted = (int)type;
		if (type != JudgeType.Poor)
		{
			typeConverted = (typeConverted + 1) / 2;
		}
		char[] comboChar = Convert.ToString(combo).ToCharArray();
		GameObject[] comboList = judgeCombo;
		if (typeConverted == -1 || typeConverted == 3 || combo == 0)
		{
			for (int i = 0; i < 4; i++)
			{
				comboList[i].GetComponent<SpriteRenderer>().enabled = false;
			}
		}
		else
		{
			if (combo >= 1000)
			{
				UpdateNumbers((JudgeType)typeConverted, comboChar);
				comboList[0].transform.position = new Vector3(0.42f, -0.81f);
				comboList[1].transform.position = new Vector3(1.13f, -0.81f);
				comboList[2].transform.position = new Vector3(1.91f, -0.81f);
				comboList[3].transform.position = new Vector3(2.65f, -0.81f);
			}
			else if (combo >= 100)
			{
				UpdateNumbers((JudgeType)typeConverted, comboChar);
				comboList[0].transform.position = new Vector3(0.78f, -0.81f);
				comboList[1].transform.position = new Vector3(1.56f, -0.81f);
				comboList[2].transform.position = new Vector3(2.3f, -0.81f);
			}
			else if (combo >= 10)
			{
				UpdateNumbers((JudgeType)typeConverted, comboChar);
				comboList[0].transform.position = new Vector3(1.08f, -0.81f);
				comboList[1].transform.position = new Vector3(1.82f, -0.81f);
			}
			else if (combo >= 1)
			{
				UpdateNumbers((JudgeType)typeConverted, comboChar);
				comboList[0].transform.position = new Vector3(1.39f, -0.81f);
			}
		}
	}
	private void UpdateNumbers(JudgeType type, char[] comboChar)
	{
		for (int i = 0; i < comboChar.Length; i++)
		{
			GameObject[] comboList = judgeCombo;
			if (type == JudgeType.Perfect)
			{
				comboList[i].GetComponent<SpriteRenderer>().sprite = perfectNumbers[comboChar[i] - 48];
			}
			else if (type == JudgeType.LGreat || type == JudgeType.EGreat)
			{
				comboList[i].GetComponent<SpriteRenderer>().sprite = greatNumbers[comboChar[i] - 48];
			}
		}
		for (int i = 0; i < comboChar.Length; i++)
		{
			judgeCombo[i].GetComponent<SpriteRenderer>().enabled = true;
		}
	}
	public void ShowJudge(JudgeType type)
	{
		int typeConverted = (int)type;
		if (type != JudgeType.Poor)
		{
			typeConverted = (typeConverted + 1) / 2;
		}
		GameObject judgeOBJ = judgeObj;
		judgeOBJ.transform.position = new Vector3(-1.59f, -0.79f);
		judgeOBJ.transform.localScale = new Vector3(1.595f, 1.595f, 1.595f);
		if (judgeOBJ.GetComponent<SpriteRenderer>() == null)
		{
			judgeOBJ.AddComponent<SpriteRenderer>();
		}
		SpriteRenderer sr = judgeOBJ.GetComponent<SpriteRenderer>();
		sr.enabled = true;
		if (typeConverted == -1)
		{
			sr.sprite = judgeTypeSprite[4];
			judgeOBJ.transform.position = new Vector3(0f, judgeOBJ.transform.position.y);
		}
		else if (typeConverted == 3)
		{
			sr.sprite = judgeTypeSprite[typeConverted];
			judgeOBJ.transform.position = new Vector3(0f, judgeOBJ.transform.position.y);
		}
		else
		{
			sr.sprite = judgeTypeSprite[typeConverted];
		}
		sr.sortingOrder = 7;
	}
	public void ShowFastSlow(JudgeType type)
	{
		if (type == JudgeType.EGreat || type == JudgeType.EGood)
		{
			fastSlowObj.GetComponent<SpriteRenderer>().sprite = fastSlow[0];
			fastSlowObj.GetComponent<SpriteRenderer>().enabled = true;
		}
		else if (type == JudgeType.LGreat || type == JudgeType.LGood)
		{
			fastSlowObj.GetComponent<SpriteRenderer>().sprite = fastSlow[1];
			fastSlowObj.GetComponent<SpriteRenderer>().enabled = true;
		}
		else
		{
			fastSlowObj.GetComponent<SpriteRenderer>().sprite = null;
			fastSlowObj.GetComponent<SpriteRenderer>().enabled = false;
		}
	}

}
