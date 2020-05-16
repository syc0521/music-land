using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NoteController;

public abstract class Notes : Judge
{
	/// <summary>
	/// 音符掉落
	/// </summary>
	public abstract void DropNote();
	/// <summary>
	/// 判定音符
	/// </summary>
	/// <param name="note">当前note</param>
	/// <returns>判定id</returns>
	public abstract JudgeType NoteJudge(NoteAsset note);
	/// <summary>
	/// 自动模式
	/// </summary>
	public abstract void AutoPlayMode();
	/// <summary>
	/// 创建note特效
	/// </summary>
	/// <param name="type">判定id</param>
	public abstract void CreateFX(JudgeType type);
	public JudgeType JudgeNote(NoteAsset note)
	{
		float sceneTime = Time.timeSinceLevelLoad;
		float time = note.Time;
		float exactTime = time + noteDropTime + fixedTime;
		if (sceneTime <= exactTime + perfectTime
			&& sceneTime > exactTime - perfectTime)
		{
			Debug.Log(note + "perfect");
			JudgeStatistics.perfect++;
			if (JudgeStatistics.life <= 99.55f)
			{
				JudgeStatistics.life += 0.45f;
			}
			return JudgeType.Perfect;
		}
		else if (sceneTime < exactTime + greatTime && sceneTime > exactTime + perfectTime)
		{
			Debug.Log(note + "Lgreat");
			JudgeStatistics.great++;
			if (JudgeStatistics.life <= 99.85f)
			{
				JudgeStatistics.life += 0.15f;
			}
			return JudgeType.LGreat;
		}
		else if (sceneTime > exactTime - greatTime && sceneTime < exactTime - perfectTime)
		{
			Debug.Log(note + "Egreat");
			JudgeStatistics.great++;
			if (JudgeStatistics.life <= 99.85f)
			{
				JudgeStatistics.life += 0.15f;
			}
			return JudgeType.EGreat;
		}
		else if (sceneTime < exactTime + goodTime && sceneTime > exactTime + greatTime)
		{
			Debug.Log(note + "Lgood");
			JudgeStatistics.good++;
			return JudgeType.LGood;
		}
		else if (sceneTime > exactTime - goodTime && sceneTime < exactTime - greatTime)
		{
			Debug.Log(note + "Egood");
			JudgeStatistics.good++;
			return JudgeType.EGood;
		}
		else if (sceneTime < exactTime - goodTime && sceneTime > exactTime - badTime)
		{
			Debug.Log(note + "Ebad");
			JudgeStatistics.bad++;
			JudgeStatistics.life -= 2f;
			return JudgeType.Bad;
		}
		else if (sceneTime < exactTime - badTime)
		{
			return JudgeType.Poor;
		}
		return JudgeType.Default;
	}

}
