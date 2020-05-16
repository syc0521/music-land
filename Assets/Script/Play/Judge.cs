using System;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour
{
	/// <summary>
	/// 判定种类
	/// </summary>
	public enum JudgeType
	{
		Perfect, LGreat, EGreat, LGood, EGood, Bad, Poor = -1, Default = -2
	}
	
}
