using System.Collections;
using System.Collections.Generic;

public class NoteAsset
{
	private string id;
	private int pos;
	private float time, dur;
	private bool canJudge;

	public NoteAsset(string id,  float time, int pos, float dur)
	{
		this.id = id;
		this.pos = pos;
		this.time = time;
		this.dur = dur;
		this.canJudge = false;
	}

	public string Id { get => id; set => id = value; }
	public int Pos { get => pos; set => pos = value; }
	public float Time { get => time; set => time = value; }
	public float Dur { get => dur; set => dur = value; }
	public bool CanJudge { get => canJudge; set => canJudge = value; }

	public override string ToString()
	{
		float dt = time - UnityEngine.Time.timeSinceLevelLoad - NoteController.noteDropTime;
		return "time=" + (time * 1000f) + ", deltaTime=" + dt + ", pos=" + pos + ", judge=";
	}
}
