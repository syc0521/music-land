using System.Collections;
using System.Collections.Generic;

public class NoteAsset
{
    public NoteAsset(string id,  float time, int pos, float dur)
	{
		Id = id;
		Pos = pos;
		Time = time;
		Dur = dur;
		CanJudge = false;
	}

    public string Id { get; set; }
    public int Pos { get; set; }
    public float Time { get; set; }
    public float Dur { get; set; }
    public bool CanJudge { get; set; }

    public override string ToString()
	{
		float dt = Time - UnityEngine.Time.timeSinceLevelLoad - NoteController.noteDropTime;
		return "time=" + (Time * 1000f) + ", deltaTime=" + dt + ", pos=" + Pos + ", judge=";
	}
}
