using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static NoteController;

public class TapController : Notes
{
	public NoteAsset note;
	public GameObject effect;
	private GameObject audioPlayer;
	private bool isDestroyed = false;
	private bool isJudged = false;
	private AudioSource key;
	[NonSerialized]
	public bool isNotePlayed = false;
	/// <summary>
	/// 十字键左
	/// </summary>
	private bool LeftArrow = false;
	/// <summary>
	/// 十字键上
	/// </summary>
	private bool UpArrow = false;
	/// <summary>
	/// 十字键右
	/// </summary>
	private bool RightArrow = false;
	
	void Start()
	{
		audioPlayer = _instance.GetKey(note);
		key = audioPlayer.GetComponent<AudioSource>();
		float basePos = 1.345f;
		transform.position = new Vector3(basePos * (note.Pos - 2), 5.6f);
	}

	void Update()
	{
		DropNote();
		if (!isJudged)
		{
			if (!ReadSong.isAutoPlay)
			{
				UserPlayMode();
			}
			else
			{
				AutoPlayMode();
			}
		}
	}

	public override void DropNote()
	{
		if (Time.timeSinceLevelLoad < note.Time + noteDropTime)
		{
			transform.position = new Vector3(transform.position.x,
				Utils.Lerp(Time.timeSinceLevelLoad, note.Time, noteDropTime + note.Time, 5.6f, -3.15f));
		}
		else if (!isDestroyed)
		{
			isDestroyed = true;
			StartCoroutine(DestroyNote());
		}
	}

	public override void AutoPlayMode()
	{
		if (Time.timeSinceLevelLoad >= note.Time + noteDropTime)
		{
			isJudged = true;
			isDestroyed = true;
			StartCoroutine(DestroyNote());
			if (!isNotePlayed)
			{
				StartCoroutine(PlaySingleKey(key));
			}
			CreateFX(JudgeType.Perfect);
			InputController.combo++;
			JudgeStatistics.perfect++;
			InputController._instance.ShowJudge(JudgeType.Perfect);
			InputController._instance.ShowCombo(JudgeType.Perfect);
		}
	}

	private void UserPlayMode()
	{
		bool isFirstNote = false;
		NoteAsset previous = InputController._instance.GetPreviousNote(note);
		if (previous == null)
		{
			isFirstNote = true;
		}
		if (Time.timeSinceLevelLoad <= note.Time + noteDropTime + goodTime &&
			Time.timeSinceLevelLoad >= note.Time + noteDropTime - badTime)
		{
			if (note.CanJudge && ((!previous.CanJudge) || !isFirstNote))
			{
				JudgeType returnType = NoteJudge(note);
				if (returnType != JudgeType.Poor && returnType != JudgeType.Bad)
				{
					isJudged = true;
					isDestroyed = true;
					CreateFX(returnType);
					InputController.combo++;
					InputController._instance.ShowJudge(returnType);
					InputController._instance.ShowCombo(returnType);
					InputController._instance.ShowFastSlow(returnType);
					StartCoroutine(DestroyNote());
				}
				else if (returnType == JudgeType.Bad)
				{
					InputController._instance.ShowCombo(returnType);
					InputController._instance.ShowFastSlow(returnType);
				}
			}
		}
		else if (Time.timeSinceLevelLoad > note.Time + noteDropTime + goodTime)
		{
			isJudged = true;
			isDestroyed = true;
			InputController.combo = 0;
			JudgeStatistics.poor++;
			if (JudgeStatistics.life >= 8)
			{
				JudgeStatistics.life -= 8f;
			}
			InputController._instance.ShowJudge(JudgeType.Poor);
			InputController._instance.ShowCombo(JudgeType.Poor);
			InputController._instance.ShowFastSlow(JudgeType.Poor);
			Debug.Log(note + "poor");
		}
	}
	public override void CreateFX(JudgeType returnType)
	{
		if ((int)returnType < 3)
		{
			GameObject judgeFX = Instantiate(effect) as GameObject;
			judgeFX.transform.position = new Vector3(transform.position.x + 0.4f, -3.44f);
		}
	}
	private IEnumerator DestroyNote()
	{
		if (!isJudged)
		{
			transform.position = new Vector3(transform.position.x, -3.25f);
			yield return new WaitForSeconds(goodTime);
		}
		else
		{
			note.CanJudge = false;
		}
		NoteAsset nextNote = InputController._instance.GetNextNote(note);
		yield return new WaitForSeconds(0.03f);
		if (nextNote != null)
		{
			nextNote.CanJudge = true;
		}
		DestroyImmediate(gameObject);
		yield break;
	}
	public override JudgeType NoteJudge(NoteAsset note)
	{
		AudioSource audio = key;
		int pos = note.Pos;
		if (InputController._instance.isControllerConnected)
		{
			if (Input.GetAxis("LeftRight") < 0 && pos == 0 && note.CanJudge)
			{
				if (!LeftArrow)
				{
					if (!isNotePlayed)
					{
						StartCoroutine(PlaySingleKey(audio));
					}
					JudgeType i = JudgeNote(note);
					if (i != JudgeType.Bad)
					{
						return i;
					}
				}
				LeftArrow = true;
			}
			else
			{
				LeftArrow = false;
			}
			if (Input.GetAxis("LeftRight") > 0 && pos == 2 && note.CanJudge)
			{
				if (!RightArrow)
				{
					if (!isNotePlayed)
					{
						StartCoroutine(PlaySingleKey(audio));
					}
					JudgeType i = JudgeNote(note);
					if (i != JudgeType.Bad)
					{
						return i;
					}
				}
				RightArrow = true;
			}
			else
			{
				RightArrow = false;
			}
			if (Input.GetAxis("UpDown") > 0 && pos == 1 && note.CanJudge)
			{
				if (!UpArrow)
				{
					if (!isNotePlayed)
					{
						StartCoroutine(PlaySingleKey(audio));
					}
					JudgeType i = JudgeNote(note);
					if (i != JudgeType.Bad)
					{
						return i;
					}
				}
				UpArrow = true;
			}
			else
			{
				UpArrow = false;
			}
			if ((Utils.KeyJudge(KeyCode.JoystickButton2, note) && pos == 2) ||
				(Utils.KeyJudge(KeyCode.JoystickButton3, note) && pos == 3) ||
				(Utils.KeyJudge(KeyCode.JoystickButton1, note) && pos == 4))
			{
				if (!isNotePlayed)
				{
					Debug.Log("play");
					StartCoroutine(PlaySingleKey(audio));
				}
				return JudgeNote(note);
			}
		}
		else
		{
			if ((Utils.KeyJudge(KeyCode.D, note) && pos == 0) || (Utils.KeyJudge(KeyCode.F, note) && pos == 1) ||
				(Utils.KeyJudge(KeyCode.Space, note) && pos == 2) || (Utils.KeyJudge(KeyCode.J, note) && pos == 3) ||
				(Utils.KeyJudge(KeyCode.K, note) && pos == 4))
			{
				JudgeType i = JudgeNote(note);
				if (!isNotePlayed && i != JudgeType.Bad)
				{
					Debug.Log("play");
					StartCoroutine(PlaySingleKey(audio));
				}
				return i;
			}
		}
		return JudgeType.Poor;
	}
	public IEnumerator PlaySingleKey(AudioSource audio)
	{
		if (!isNotePlayed)
		{
			audio.enabled = true;
			yield return new WaitForSeconds(ReadSong.delay);
			audio.Play();
			Debug.Log("audio is playing");
			isNotePlayed = true;
		}
		yield break;
	}
}