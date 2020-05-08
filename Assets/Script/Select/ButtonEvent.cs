using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using static ReadSong;

public class ButtonEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public GameObject rect;
	private GameObject obj;
	public PlayableDirector mask;

	public void Click01()
	{
		diff = 0;
		PlaySong();
	}
	public void Click02()
	{
		diff = 1;
		PlaySong();
	}
	public void Click03()
	{
		diff = 2;
		PlaySong();
	}
	public void Click04()
	{
		diff = 3;
		List.PassSong(ReadList.SelectedSong);
		mask.Play();
		Invoke("Load", 0.65f);
	}
	private void PlaySong()
	{
		List.PassSong(ReadList.SelectedSong);
		mask.Play();
		Invoke("Load", 0.65f);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		obj = Instantiate(rect) as GameObject;
		obj.transform.position = new Vector3(transform.position.x, transform.position.y);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Destroy(obj);
	}
	private void Load()
	{
		SceneManager.LoadScene("Decide");
	}

}
