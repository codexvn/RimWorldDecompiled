using System.Collections.Generic;
using Verse;

namespace RimWorld;

public class MusicSequenceWorker
{
	private static readonly List<SongDef> songs = new List<SongDef>();

	public MusicSequenceDef def { get; private set; }

	public MusicManagerPlay musicManager { get; private set; }

	public int timesLooped { get; set; }

	public bool IsPlaying
	{
		get
		{
			if (musicManager.MusicSequenceWorker == this)
			{
				return musicManager.IsPlaying;
			}
			return false;
		}
	}

	public void InitializeWorker(MusicSequenceDef def, MusicManagerPlay musicManager)
	{
		this.def = def;
		this.musicManager = musicManager;
		songs.Clear();
		if (def.songs != null)
		{
			songs.AddRange(def.songs);
		}
		Shuffle();
	}

	public virtual bool ShouldTransition()
	{
		if ((IsPlaying || def.transitionOnDanger || def.transitionOnNoDanger) && (!def.transitionOnDanger || !musicManager.DangerMusicMode))
		{
			if (def.transitionOnNoDanger)
			{
				return !musicManager.DangerMusicMode;
			}
			return false;
		}
		return true;
	}

	public virtual bool ShouldLoop()
	{
		return def.loop;
	}

	public virtual bool ShouldEnd()
	{
		if (MinTimeSatisfied())
		{
			if (!def.endOnDanger || !musicManager.DangerMusicMode)
			{
				if (def.endOnNoDanger)
				{
					return !musicManager.DangerMusicMode;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public virtual bool CanBeInterrupted()
	{
		return def.canBeInterrupted;
	}

	public virtual SongDef SelectSong()
	{
		if (def.song != null)
		{
			return def.song;
		}
		if (timesLooped % songs.Count == 0)
		{
			Shuffle();
		}
		return songs[timesLooped % def.songs.Count];
	}

	private void Shuffle()
	{
		if (songs.Count > 2)
		{
			SongDef last = songs.GetLast();
			int num = songs.Count;
			while (num > 1)
			{
				num--;
				int num2 = Rand.Range(0, num + 1);
				List<SongDef> list = songs;
				int index = num2;
				List<SongDef> list2 = songs;
				int index2 = num;
				SongDef value = songs[num];
				SongDef value2 = songs[num2];
				list[index] = value;
				list2[index2] = value2;
			}
			if (songs[0] == last)
			{
				int num3 = Rand.Range(1, songs.Count);
				List<SongDef> list3 = songs;
				List<SongDef> list = songs;
				int index2 = num3;
				SongDef value2 = songs[num3];
				SongDef value = songs[0];
				list3[0] = value2;
				list[index2] = value;
			}
		}
	}

	public bool MinTimeSatisfied()
	{
		if (!def.minTimeToPlay.HasValue)
		{
			return true;
		}
		if (timesLooped <= 0)
		{
			return musicManager.SongTime >= def.minTimeToPlay.Value;
		}
		return true;
	}
}
