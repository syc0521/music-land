using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class FunctionalBtns : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public AudioSource a1;
	private static int isSetting = -1;
	public GameObject rect;
	private GameObject obj;
	public AudioLowPassFilter lpf;
	public PlayableDirector mask;
	public void ChangeMusic()
	{
		isSetting = -isSetting;
		if (isSetting > 0)
		{
			lpf.enabled = true;
		}
		else
		{
			lpf.enabled = false;
		}
	}
	public void ClickFX()
	{
		GetComponent<AudioSource>().Play();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (rect != null)
		{
			obj = Instantiate(rect) as GameObject;
			obj.transform.position = new Vector3(transform.position.x, transform.position.y);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (obj != null)
		{
			Destroy(obj);
		}
	}
	public void ToEX()
	{
		Transition.scene = "SelectEX";
		mask.Play();
		Invoke("Load", 0.65f);
	}
	public void ToNormal()
	{
		Transition.scene = "Select";
		mask.Play();
		Invoke("Load", 0.65f);
	}
	public void ToAlbum()
	{
		Transition.scene = "Album";
		mask.Play();
		AlbumController.isEX = false;
		Invoke("Load", 0.55f);
	}
	public void ToAlbumEX()
	{
		Transition.scene = "Album";
		mask.Play();
		AlbumController.isEX = true;
		Invoke("Load", 0.55f);
	}
	private void Load()
	{
		SceneManager.LoadScene("Transition");
	}
}
