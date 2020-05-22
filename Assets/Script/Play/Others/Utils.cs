using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
	private static int start;
	/// <summary>
	/// 插值运算
	/// </summary>
	/// <param name="time">场景时间</param>
	/// <param name="timeRangeL"></param>
	/// <param name="timeRangeR"></param>
	/// <param name="posRangeL"></param>
	/// <param name="posRangeR"></param>
	/// <returns></returns>
	public static float Lerp(float time, float timeRangeL, float timeRangeR, float posRangeL, float posRangeR)
	{
		return Mathf.LerpUnclamped(posRangeL, posRangeR, (time - timeRangeL) / (timeRangeR - timeRangeL));
	}
	public static bool KeyJudge(KeyCode key, NoteAsset note)
	{
		return Input.GetKeyDown(key) && note.CanJudge;
	}
	public static bool JoystickLJudge(NoteAsset note)//0
	{
		start = 0;
		int next = (int)Input.GetAxisRaw("LeftRight");
		if (next - start == -1)
		{
			return note.CanJudge;
		}
		return false;
	}
	public static bool JoystickUJudge(NoteAsset note)//1
	{
		return (Input.GetAxisRaw("UpDown") == 1) && note.CanJudge;
	}
	public static bool JoystickRJudge(NoteAsset note)//2
	{
		return (Input.GetAxisRaw("LeftRight") == 1) && note.CanJudge;
	}
	public static bool HoldJudge(KeyCode key, NoteAsset note)
	{
		return Input.GetKey(key) && note.CanJudge;
	}
	public static bool HoldUpJudge(KeyCode key, NoteAsset note)
	{
		float upTime = Time.timeSinceLevelLoad + note.Dur + NoteController.noteDropTime - 0.05f;
		return Input.GetKeyUp(key) && (Time.timeSinceLevelLoad <= upTime) && note.CanJudge;
	}
}
