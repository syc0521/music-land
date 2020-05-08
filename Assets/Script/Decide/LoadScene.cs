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
	IEnumerator LoadNextScene(string scene)
	{
		yield return new WaitForSeconds(0.3f);
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
		while (!asyncLoad.isDone)
		{
			yield return null;
		}
	}
}
