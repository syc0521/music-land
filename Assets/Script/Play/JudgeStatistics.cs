using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeStatistics : MonoBehaviour
{
	public static int perfect = 0;//+3
	public static int great = 0;//+2
	public static int good = 0;//+1
	public static int bad = 0;
	public static int poor = 0;
	public static int maxCombo = 0;
	public static float life = 100f;
	public static int score = 0;
	public static bool isDead = false;
	public static int totalScore;
	public static float realRate;
	/// <summary>
	/// 单例
	/// </summary>
	public static JudgeStatistics _instance;
	/// <summary>
	/// 数字sprite
	/// </summary>
	public Sprite[] numbers = new Sprite[10];
	public GameObject[] pfObj = new GameObject[4];
	public GameObject[] grObj = new GameObject[4];
	public GameObject[] gdObj = new GameObject[4];
	public GameObject[] bdObj = new GameObject[4];
	public GameObject[] prObj = new GameObject[4];
	public GameObject[] rtObj = new GameObject[3];
	
	private SpriteRenderer[] pfSprite = new SpriteRenderer[4];
	private SpriteRenderer[] grSprite = new SpriteRenderer[4];
	private SpriteRenderer[] gdSprite = new SpriteRenderer[4];
	private SpriteRenderer[] bdSprite = new SpriteRenderer[4];
	private SpriteRenderer[] prSprite = new SpriteRenderer[4];
	private SpriteRenderer[] rtSprite = new SpriteRenderer[3];
	/// <summary>
	/// 遮罩
	/// </summary>
	public GameObject mask;

	private void Start()
	{
		for (int i = 0; i < 4; i++)
		{
			pfSprite[i] = pfObj[i].GetComponent<SpriteRenderer>(); 
			grSprite[i] = grObj[i].GetComponent<SpriteRenderer>();
			gdSprite[i] = gdObj[i].GetComponent<SpriteRenderer>();
			bdSprite[i] = bdObj[i].GetComponent<SpriteRenderer>();
			prSprite[i] = prObj[i].GetComponent<SpriteRenderer>();

			pfSprite[i].enabled = false;
			grSprite[i].enabled = false;
			gdSprite[i].enabled = false;
			bdSprite[i].enabled = false;
			prSprite[i].enabled = false;
		}
		for (int i = 0; i < 3; i++)
		{
			rtSprite[i] = rtObj[i].GetComponent<SpriteRenderer>();
			rtObj[i].GetComponent<SpriteRenderer>().enabled = false;
		}
	}
	/// <summary>
	/// 清除之前的判定统计
	/// </summary>
	public static void ClearJudge()
	{
		perfect = 0;
		great = 0;
		good = 0;
		bad = 0;
		poor = 0;
		InputController.combo = 0;
		score = 0;
		life = 100;
	}
	void Update()
	{
		//if (life < 0)
		//{
		//	SceneManager.LoadScene("Result");
		//}
		if (InputController.combo >= maxCombo)
		{
			maxCombo = InputController.combo;
		}
		score = perfect * 3 + great * 2 + good;
		int rt = score * 100 / totalScore;
		realRate = score * 100.0f / totalScore;
		int[] pfNum = GetNumbers(perfect, 4);
		int[] grNum = GetNumbers(great, 4);
		int[] gdNum = GetNumbers(good, 4);
		int[] bdNum = GetNumbers(bad, 4);
		int[] prNum = GetNumbers(poor, 4);
		int[] scoreNum = GetNumbers(rt, 3);
		ShowNumbers(pfNum, pfSprite, perfect, 4);
		ShowNumbers(grNum, grSprite, great, 4);
		ShowNumbers(gdNum, gdSprite, good, 4);
		ShowNumbers(bdNum, bdSprite, bad, 4);
		ShowNumbers(prNum, prSprite, poor, 4);
		ShowNumbers(scoreNum, rtSprite, rt, 3);
		mask.transform.position = new Vector3(mask.transform.position.x, 0.144f + ((3.736f + 0.144f) / 100.0f) * life);
		mask.transform.localScale = new Vector3(mask.transform.localScale.x, 0.0352f + (1.1171f - 0.0352f) / 100.0f * (100 - life), 1);
	}
	/// <summary>
	/// 数字转数组
	/// </summary>
	/// <param name="num">数字</param>
	/// <param name="index">最大长度</param>
	/// <returns></returns>
	private int[] GetNumbers(int num, int index)
	{
		char[] tmp = num.ToString().ToCharArray();
		int[] nums = new int[index];
		for (int i = 0; i < nums.Length; i++)
		{
			nums[i] = 0;
		}
		int digit = tmp.Length;
		int numIndex = index - 1;
		while (digit > 0)
		{
			nums[numIndex] = tmp[digit - 1] - 48;
			numIndex--;
			digit--;
		}
		return nums;
	}
	/// <summary>
	/// 显示数组
	/// </summary>
	/// <param name="numArray">数组</param>
	/// <param name="objects"></param>
	/// <param name="num"></param>
	/// <param name="index">最大长度</param>
	private void ShowNumbers(int[] numArray, SpriteRenderer[] objects, int num, int index)
	{
		index--;
		int digit = num.ToString().Length;
		for (int i = 0; i < numArray.Length; i++)
		{
			objects[i].sprite = numbers[numArray[i]];
		}
		while (digit > 0)
		{
			objects[index].enabled = true;
			index--;
			digit--;
		}
	}
}
