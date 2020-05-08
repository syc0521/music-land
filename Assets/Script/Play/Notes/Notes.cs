using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NoteController;

public abstract class Notes : MonoBehaviour
{
	public const int PERFECT = 0;
	public const int LGREAT = 1;
	public const int EGREAT = 2;
	public const int LGOOD = 3;
	public const int EGOOD = 4;
	public const int BAD = 5;
	public const int POOR = -1;

	public abstract void DropNote();
	public abstract int NoteJudge(NoteAsset note);
	public abstract void AutoPlayMode();
	public abstract void CreateFX(int returnType);
	public int JudgeNote(NoteAsset note)
	{
		float time = note.Time;
		float exactTime = time + noteDropTime + fixedTime;
		if (Time.timeSinceLevelLoad <= exactTime + perfectTime
			&& Time.timeSinceLevelLoad > exactTime - perfectTime)
		{
			Debug.Log(note + "perfect");
			JudgeStatistics.perfect++;
			if (JudgeStatistics.life <= 99.55f)
			{
				JudgeStatistics.life += 0.45f;
			}
			return PERFECT;
		}
		else if (Time.timeSinceLevelLoad < exactTime + greatTime && Time.timeSinceLevelLoad > exactTime + perfectTime)
		{
			Debug.Log(note + "Lgreat");
			JudgeStatistics.great++;
			if (JudgeStatistics.life <= 99.85f)
			{
				JudgeStatistics.life += 0.15f;
			}
			return LGREAT;
		}
		else if (Time.timeSinceLevelLoad > exactTime - greatTime && Time.timeSinceLevelLoad < exactTime - perfectTime)
		{
			Debug.Log(note + "Egreat");
			JudgeStatistics.great++;
			if (JudgeStatistics.life <= 99.85f)
			{
				JudgeStatistics.life += 0.15f;
			}
			return EGREAT;
		}
		else if (Time.timeSinceLevelLoad < exactTime + goodTime && Time.timeSinceLevelLoad > exactTime + greatTime)
		{
			Debug.Log(note + "Lgood");
			JudgeStatistics.good++;
			return LGOOD;
		}
		else if (Time.timeSinceLevelLoad > exactTime - goodTime && Time.timeSinceLevelLoad < exactTime - greatTime)
		{
			Debug.Log(note + "Egood");
			JudgeStatistics.good++;
			return EGOOD;
		}
		else if (Time.timeSinceLevelLoad < exactTime - goodTime && Time.timeSinceLevelLoad > exactTime - badTime)
		{
			Debug.Log(note + "Ebad");
			JudgeStatistics.bad++;
			JudgeStatistics.life -= 2f;
			return BAD;
		}
		else if (Time.timeSinceLevelLoad < exactTime - badTime)
		{
			return POOR;
		}
		return -2;
	}

}
