using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class AlbumPlayer : MonoBehaviour
{
	/// <summary>
	/// 渲染纹理（和视频播放有关）
	/// </summary>
	public RenderTexture renderTexture;
	public static Song song;
	private float length;
	/// <summary>
	/// 遮罩动画
	/// </summary>
	public PlayableDirector mask;

	void Start()
	{
		StartCoroutine(PlayAudio(song.Path));
		StartCoroutine(PlayBGA(song.Path));
		StartCoroutine(JumpScene());
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			mask.Play();
			StartCoroutine(JumpScene(0.75f));
		}
	}
	/// <summary>
	/// 调用协程播放音乐
	/// </summary>
	/// <param name="path">路径</param>
	/// <returns></returns>
	private IEnumerator PlayAudio(string path)
	{
		AudioClip clip = Resources.Load<AudioClip>("Songs/" + path + "/bgm_final");
		AudioSource audiosource = gameObject.AddComponent<AudioSource>();
		audiosource.clip = clip;
		length = clip.length;
		audiosource.playOnAwake = false;
		audiosource.volume = 0.9f;
		yield return new WaitForSeconds(NoteController.noteDropTime + 0.01f);
		audiosource.Play();
	}
	/// <summary>
	/// 调用协程播放BGA
	/// </summary>
	/// <param name="path">路径</param>
	/// <returns></returns>
	private IEnumerator PlayBGA(string path)
	{
		VideoClip clip = Resources.Load<VideoClip>("Songs/" + path + "/bga");
		VideoPlayer videoPlayer = gameObject.AddComponent<VideoPlayer>();
		videoPlayer.clip = clip;
		videoPlayer.playOnAwake = false;
		videoPlayer.renderMode = VideoRenderMode.RenderTexture;
		videoPlayer.targetTexture = renderTexture;
		videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
		videoPlayer.waitForFirstFrame = true;
		yield return new WaitForSeconds(0.16f);
		videoPlayer.Play();
		yield break;
	}
	/// <summary>
	/// 跳转场景
	/// </summary>
	/// <returns></returns>
	private IEnumerator JumpScene()
	{
		yield return new WaitForSeconds(length);
		mask.Play();
		yield return new WaitForSeconds(0.65f);
		Transition.scene = "Album";
		SceneManager.LoadScene("Transition");
		yield break;
	}
	/// <summary>
	/// 中途手动停止后的跳转场景操作
	/// </summary>
	/// <param name="length">延迟时间</param>
	/// <returns></returns>
	private IEnumerator JumpScene(float length)
	{
		yield return new WaitForSeconds(length);
		Transition.scene = "Album";
		SceneManager.LoadScene("Transition");
		yield break;
	}
}
