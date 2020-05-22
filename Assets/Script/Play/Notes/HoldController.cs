using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NoteController;

public class HoldController : Notes
{
	public NoteAsset note;
	public GameObject effect;
	private bool isJudged = false;
	private AudioSource key;
	[NonSerialized]
	public bool isPlayed = false;
	public bool isComplete = false;
	public GameObject start;
	public GameObject end;
	public GameObject fill;
	private readonly float basePos = 1.345f;
	private float endPos;
	private GameObject audioPlayer;
	private GameObject judgeFX;

	public bool IsHolded { get; set; } = false;
	public bool IsDestroyed { get; set; } = false;

	void Start()
	{
		IsHolded = false;
		endPos = (8.9f / noteDropTime) * note.Dur;
		audioPlayer = _instance.GetKey(note);
		key = audioPlayer.GetComponent<AudioSource>();
		start.transform.position = new Vector3(basePos * (note.Pos - 2), 5.6f + 0.08f);
		end.transform.position = new Vector3(basePos * (note.Pos - 2), 5.6f + endPos + 0.08f);
		fill.GetComponent<LineRenderer>().SetPosition(0, new Vector3(basePos * (note.Pos - 2), 5.6f + endPos));
		fill.GetComponent<LineRenderer>().SetPosition(1, new Vector3(basePos * (note.Pos - 2), 5.6f + 0.08f));
	}

	void Update()
	{
		DropNote();
		if (!ReadSong.isAutoPlay)
		{
			HoldUpJudge();
		}
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
	private void HoldUpJudge()
	{
		int pos = note.Pos;
		bool isUp;
		if (InputController._instance.isControllerConnected)
		{
			isUp = (Utils.HoldUpJudge(KeyCode.JoystickButton2, note) && pos == 2) || (Utils.HoldUpJudge(KeyCode.JoystickButton3, note) && pos == 3) ||
				(Utils.HoldUpJudge(KeyCode.JoystickButton1, note) && pos == 4);
		}
		else
		{
			isUp = (Input.GetKeyUp(KeyCode.D) && pos == 0) || (Input.GetKeyUp(KeyCode.F) && pos == 1)
				|| (Input.GetKeyUp(KeyCode.Space) && pos == 2) || (Input.GetKeyUp(KeyCode.J) && pos == 3)
				|| (Input.GetKeyUp(KeyCode.K) && pos == 4);
		}
		if (isJudged && IsHolded && isUp)
		{
			IsHolded = false;
			StartCoroutine(DestroyNote());
			InputController.combo = 0;
			JudgeStatistics.bad++;
			InputController._instance.ShowJudge(JudgeType.Bad);
			InputController._instance.ShowFastSlow(JudgeType.Bad);
			InputController._instance.ShowCombo(JudgeType.Bad);
			Debug.Log("not hold");
			TransparentHold();
			key.Stop();
		}
	}

	private void TransparentHold()
	{
		start.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
		end.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
		fill.GetComponent<LineRenderer>().startColor = new Color(1, 1, 1, 0.3f);
		fill.GetComponent<LineRenderer>().endColor = new Color(1, 1, 1, 0.3f);
		StartCoroutine(FinishImmediate());
	}
	private IEnumerator FinishImmediate()
	{
        if (judgeFX != null)
        {
			judgeFX.GetComponent<HoldEffect>().Finish();
			yield return new WaitForSeconds(0.237f);
			judgeFX.SetActive(false);
			yield break;
		}
        else
        {
			yield return null;
        }
	}
	public override void DropNote()
	{
		float ndp = noteDropTime;
		if (Time.timeSinceLevelLoad < note.Time + ndp)//0上1下
		{
			start.transform.position = new Vector3(basePos * (note.Pos - 2),
				Utils.Lerp(Time.timeSinceLevelLoad, note.Time, ndp + note.Time, 5.6f, -3.2f + 0.08f));
			end.transform.position = new Vector3(basePos * (note.Pos - 2),
				Utils.Lerp(Time.timeSinceLevelLoad, note.Time, ndp + note.Time, 5.6f + endPos + 0.08f, -3.2f + endPos));
			fill.GetComponent<LineRenderer>().SetPosition(0, new Vector3(basePos * (note.Pos - 2),
				Utils.Lerp(Time.timeSinceLevelLoad, note.Time, ndp + note.Time, 5.6f + endPos, -3.2f + endPos)));
			fill.GetComponent<LineRenderer>().SetPosition(1, new Vector3(basePos * (note.Pos - 2),
				Utils.Lerp(Time.timeSinceLevelLoad, note.Time, ndp + note.Time, 5.6f - 0.08f, -3.2f - 0.08f)));
			transform.position = new Vector3(basePos * (note.Pos - 2),
				Utils.Lerp(Time.timeSinceLevelLoad, note.Time, ndp + note.Time, 5.6f, -3.2f));
		}
		else if (Time.timeSinceLevelLoad < note.Time + ndp + note.Dur)
		{
			end.transform.position = new Vector3(basePos * (note.Pos - 2),
				Utils.Lerp(Time.timeSinceLevelLoad, ndp + note.Time, ndp + note.Time + note.Dur, -3.2f + endPos, -3.2f));
			fill.GetComponent<LineRenderer>().SetPosition(0, new Vector3(basePos * (note.Pos - 2),
				Utils.Lerp(Time.timeSinceLevelLoad, ndp + note.Time, ndp + note.Time + note.Dur, -3.2f + endPos, -3.2f)));
			fill.GetComponent<LineRenderer>().SetPosition(1, new Vector3(basePos * (note.Pos - 2), -3.2f));
			transform.position = new Vector3(transform.position.x, -3.25f);
		}
		else if (Time.timeSinceLevelLoad >= note.Time + ndp + note.Dur)
		{
			start.GetComponent<SpriteRenderer>().enabled = false;
			end.GetComponent<SpriteRenderer>().enabled = false;
			fill.GetComponent<LineRenderer>().enabled = false;
			IsDestroyed = true;
			StartCoroutine(DestroyNote());
		}
	}
	public override void AutoPlayMode()
	{
		IsHolded = true;
		if (Time.timeSinceLevelLoad >= note.Time + noteDropTime)
		{
			isJudged = true;
			IsDestroyed = true;
			StartCoroutine(DestroyNote());
			StartCoroutine(PlaySingleKey(key));
			CreateFX(JudgeType.Perfect);
			InputController.combo++;
			JudgeStatistics.perfect++;
			Invoke("holdEnded", note.Dur);
			InputController._instance.ShowJudge(JudgeType.Perfect);
			InputController._instance.ShowCombo(JudgeType.Perfect);
		}
	}
	private void HoldEnded()
	{
		if (IsHolded)
		{
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
		if (Time.timeSinceLevelLoad <= note.Time + noteDropTime + goodTime)
		{
			if (note.CanJudge && ((!previous.CanJudge) || !isFirstNote))
			{
				JudgeType returnType = NoteJudge(note);
				if (returnType != JudgeType.Poor && returnType != JudgeType.Bad)
				{
					isJudged = true;
					StartCoroutine(DestroyNote());
					CreateFX(returnType);
					InputController.combo++;
					InputController._instance.ShowJudge(returnType);
					InputController._instance.ShowCombo(returnType);
					InputController._instance.ShowFastSlow(returnType);
					Invoke("HoldEnded", note.Dur);
				}
				else if (returnType == JudgeType.Bad && returnType == JudgeType.Poor)
				{
					InputController._instance.ShowCombo(returnType);
					InputController._instance.ShowFastSlow(returnType);
				}
			}
		}
		else
		{
			TransparentHold();
			isJudged = true;
			IsDestroyed = true;
			InputController.combo = 0;
			JudgeStatistics.poor += 2;
			if (JudgeStatistics.life >= 9)
			{
				JudgeStatistics.life -= 8f;
			}
			InputController._instance.ShowJudge(JudgeType.Poor);
			InputController._instance.ShowCombo(JudgeType.Poor);
			InputController._instance.ShowFastSlow(JudgeType.Poor);
			Debug.Log(note + "poor");
		}
	}
	public override JudgeType NoteJudge(NoteAsset note)
	{
		AudioSource audio = key;
		int pos = note.Pos;
		if (InputController._instance.isControllerConnected)
		{
			if ((Utils.JoystickLJudge(note) && pos == 0) || (Utils.JoystickUJudge(note) && pos == 1) ||
				(Utils.HoldJudge(KeyCode.JoystickButton2, note) && pos == 2) || (Utils.HoldJudge(KeyCode.JoystickButton3, note) && pos == 3) ||
				(Utils.HoldJudge(KeyCode.JoystickButton1, note) && pos == 4) || (Utils.JoystickRJudge(note) && pos == 2))
			{
				StartCoroutine(PlaySingleKey(audio));
				IsHolded = true;
				return JudgeNote(note);
			}
			//else if ((Utils.HoldUpJudge(KeyCode.JoystickButton2, note) && pos == 2) || (Utils.HoldUpJudge(KeyCode.JoystickButton3, note) && pos == 3) ||
			//	(Utils.HoldUpJudge(KeyCode.JoystickButton1, note) && pos == 4))
			//{
			//	IsHolded = false;
			//	IsDestroyed = true;
			//	isComplete = false;
			//}
		}
		else
		{
			JudgeType judge = JudgeType.Poor;
			if ((Utils.HoldJudge(KeyCode.D, note) && pos == 0) || (Utils.HoldJudge(KeyCode.F, note) && pos == 1) ||
				(Utils.HoldJudge(KeyCode.Space, note) && pos == 2) || (Utils.HoldJudge(KeyCode.J, note) && pos == 3) ||
				(Utils.HoldJudge(KeyCode.K, note) && pos == 4))
			{
				judge = JudgeNote(note);
				IsHolded = true;
				StartCoroutine(Timer());
				StartCoroutine(PlaySingleKey(audio));
			}
			else
			{
				IsHolded = false;
			}
			return judge;
		}
		return JudgeType.Poor;
	}

	private IEnumerator Timer()
	{
		while (IsHolded)
		{
			//dt += Time.deltaTime;
			yield return null;
		}
	}
	private IEnumerator DestroyNote()
	{
		float duration = note.Dur;
		if (!isJudged)
		{
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
		yield return new WaitForSeconds(duration);
		Destroy(gameObject);
		yield break;
	}
	private IEnumerator PlaySingleKey(AudioSource audio)
	{
		if (!isPlayed)
		{
			audio.enabled = true;
			yield return new WaitForSeconds(ReadSong.delay);
			if (IsHolded)
			{
				audio.Play();
			}
			else
			{
				audio.Stop();
			}
			isPlayed = true;
		}
		yield return new WaitForSeconds(audio.time / 1000.0f);
		yield break;
	}
	public override void CreateFX(JudgeType type)
	{
		if ((int)type < 3)
		{
			judgeFX = Instantiate(effect) as GameObject;
			judgeFX.GetComponent<HoldEffect>().duration = note.Dur;
			judgeFX.transform.position = new Vector3(transform.position.x + 0.4f, -3.44f);
		}
	}
}
