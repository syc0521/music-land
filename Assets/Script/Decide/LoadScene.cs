using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
	public static string scene;
	public SpriteRenderer sr;
	public static Song song;
	void Start()
	{
		JudgeStatistics.ClearJudge();
		StartCoroutine(LoadNextScene(scene));
		GetTitlePic(song);
	}
	/// <summary>
	/// 获取歌曲封面
	/// </summary>
	/// <param name="song">当前歌曲</param>
	private void GetTitlePic(Song song)
	{
		try
		{
			sr.sprite = Resources.Load<Sprite>("Songs/" + song.Path + "/title");
		}
		catch (Exception)
		{
			throw;
		}
	}
	/// <summary>
	/// 加载下一个场景
	/// </summary>
	/// <param name="scene">场景名称</param>
	/// <returns></returns>
	IEnumerator LoadNextScene(string scene)
	{
		yield return new WaitForSeconds(0.3f);//等待0.3秒
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);//异步加载场景
		while (!asyncLoad.isDone)//如果没加载完，则一直等待
		{
			yield return null;
		}
	}
}
