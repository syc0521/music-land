using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class ReadSong : MonoBehaviour
{
	public static ReadSong _instance;
	public static string path;
	public bool bgaExist;
	public static int diff;
	public static int bpm;
	public static bool isAutoPlay = false;
	public static float delay;
	public static float vol;
	public RenderTexture renderTexture;
	private readonly Dictionary<int, string> diffs = new Dictionary<int, string>();
	private int totalScore;
	private float length;
	public static Song song;
	/// <summary>
	/// 遮罩动画
	/// </summary>
	public PlayableDirector mask;

	void Awake()
	{
		_instance = this;
		diffs.Add(0, "easy");
		diffs.Add(1, "advanced");
		diffs.Add(2, "hard");
		diffs.Add(3, "expert");
		path = song.Path;
	}
	private IEnumerator PlayAudio(string path)
	{
		AudioClip clip = Resources.Load<AudioClip>("Songs/" + path + "/bgm_" + diff);
		AudioSource audiosource = gameObject.AddComponent<AudioSource>();
		audiosource.clip = clip;
		audiosource.playOnAwake = false;
		audiosource.volume = 0.9f;
		length = clip.length;
		yield return new WaitForSeconds(NoteController.noteDropTime + 0.01f);//0.23f
		audiosource.Play();
	}
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
	private IEnumerator PlayPic(string path)
	{
		Sprite sprite = Resources.Load<Sprite>("Songs/" + path + "/bga");
		SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = sprite;
		spriteRenderer.sortingOrder = 0;
		yield break;
	}
	private void GetMap(string path)
	{
		try
		{
			TextAsset text = Resources.Load<TextAsset>("Songs/" + path + "/" + diff);
			string[] lines = text.text.Split('\n');
			Debug.Log("Assets/Resources/Songs/" + path + "/" + diff + ".txt");
			bpm = int.Parse(lines[0].Substring(5));
			totalScore = 0;
			for (int i = 1; i < lines.Length; i++)
			{
				if (lines[i] != "")
				{
					string[] line = lines[i].Split(',');
					NoteAsset note = new NoteAsset(line[0], (int.Parse(line[1])) / 1000.0f, int.Parse(line[2]), int.Parse(line[3]) / 1000.0f);
					if ((int.Parse(line[3]) / 1000.0f) > 0)
					{
						totalScore += 2;
					}
					else
					{
						totalScore++;
					}
					NoteController._instance.noteList.Add(note);
				}
			}
			JudgeStatistics.totalScore = totalScore * 3;
		}
		catch (System.Exception)
		{
			Debug.LogError("File not exist.");
		}
	}
	private void GetSoundList(string path)
	{
		try
		{
			TextAsset text = Resources.Load<TextAsset>("Songs/" + path + "/" + "sound_" + diff);
			string[] lines = text.text.Split('\n');
			for (int i = 0; i < lines.Length; i++)
			{
				if (!lines[i].Equals(""))
				{
					string[] line = lines[i].Split(' ');
					string id = line[0].Substring(4, 2);
					string name = line[1].Substring(0, line[1].Length - 5);
					NoteController._instance.soundList.Add(id, name);
				}
			}
		}
		catch (System.Exception)
		{
			Debug.LogError("Sound file not exist.");
		}
	}
	private void Start()
	{
		GetFiles(path);
		SetParameters();
		Invoke("ShowMask", length);
	}
	private void Update()
	{
		StartCoroutine(JumpScene());
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			mask.Play();
			StartCoroutine(JumpScene(0.75f));
		}
	}

	private void GetFiles(string path)
	{
		GetMap(path);
		GetSoundList(path);
		StartCoroutine(PlayAudio(path));
		if (bgaExist)
		{
			StartCoroutine(PlayBGA(path));
		}
		else
		{
			StartCoroutine(PlayPic(path));
		}
	}

	private void SetParameters()
	{
		if (isAutoPlay)
		{
			delay = 0.010f;
			vol = 0.55f;
		}
		else
		{
			delay = 0.020f;
		}
	}
	private IEnumerator JumpScene()
	{
		yield return new WaitForSeconds(length);
		mask.Play();
		yield return new WaitForSeconds(0.65f);
		SceneManager.LoadScene("Result");
		yield break;
	}
	private IEnumerator JumpScene(float length)
	{
		yield return new WaitForSeconds(length);
        if (diff == 3)
        {
			Transition.scene = "SelectEX";
		}
        else
        {
			Transition.scene = "Select";
		}
		ReadList.SelectedSong = song;
		SceneManager.LoadScene("Transition");
		yield break;
	}
}
