﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
	public static string scene;

	void Start()
	{
		StartCoroutine(LoadNextScene(scene));
	}

	IEnumerator LoadNextScene(string scene)
	{
		yield return new WaitForSeconds(0.65f);
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
		while (!asyncLoad.isDone)
		{
			yield return null;
		}
	}
}