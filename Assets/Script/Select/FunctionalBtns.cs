using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class FunctionalBtns : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public AudioSource bgm;
	private static int isSetting = -1;
	/// <summary>
	/// 包围矩形
	/// </summary>
	public GameObject rect;
	/// <summary>
	/// 矩形实例
	/// </summary>
	private GameObject obj;
	/// <summary>
	/// 低通效果器
	/// </summary>
	public AudioLowPassFilter lpf;
	/// <summary>
	/// 遮罩动画
	/// </summary>
	public PlayableDirector mask;
	/// <summary>
	/// 切换效果器
	/// </summary>
	public void ChangeEffect()
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
	/// <summary>
	/// 点击按钮声音
	/// </summary>
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
	/// <summary>
	/// 隐藏难度按钮
	/// </summary>
	public void ToEX()
	{
		Transition.scene = "SelectEX";
		mask.Play();
		Invoke("Load", 0.65f);
		Settings.isExMode = true;
	}
	/// <summary>
	/// 普通难度按钮
	/// </summary>
	public void ToNormal()
	{
		Transition.scene = "Select";
		mask.Play();
		Invoke("Load", 0.65f);
		Settings.isExMode = false;
	}
	/// <summary>
	/// Album模式按钮
	/// </summary>
	public void ToAlbum()
	{
		Transition.scene = "Album";
		mask.Play();
		AlbumController.isEX = false;
		Invoke("Load", 0.55f);
	}
	/// <summary>
	/// 隐藏难度下的Album按钮
	/// </summary>
	public void ToAlbumEX()
	{
		Transition.scene = "Album";
		mask.Play();
		AlbumController.isEX = true;
		Invoke("Load", 0.55f);
	}
	/// <summary>
	/// 加载Loading场景
	/// </summary>
	private void Load()
	{
		SceneManager.LoadScene("Transition");
	}
}
