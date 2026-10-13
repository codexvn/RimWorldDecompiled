using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002906 RID: 10502
	public class MusicSequenceWorker
	{
		// Token: 0x170022E8 RID: 8936
		// (get) Token: 0x0600E5C5 RID: 58821 RVA: 0x00430E25 File Offset: 0x0042F025
		// (set) Token: 0x0600E5C6 RID: 58822 RVA: 0x00430E2D File Offset: 0x0042F02D
		public MusicSequenceDef def { get; private set; }

		// Token: 0x170022E9 RID: 8937
		// (get) Token: 0x0600E5C7 RID: 58823 RVA: 0x00430E36 File Offset: 0x0042F036
		// (set) Token: 0x0600E5C8 RID: 58824 RVA: 0x00430E3E File Offset: 0x0042F03E
		public MusicManagerPlay musicManager { get; private set; }

		// Token: 0x170022EA RID: 8938
		// (get) Token: 0x0600E5C9 RID: 58825 RVA: 0x00430E47 File Offset: 0x0042F047
		// (set) Token: 0x0600E5CA RID: 58826 RVA: 0x00430E4F File Offset: 0x0042F04F
		public int timesLooped { get; set; }

		// Token: 0x170022EB RID: 8939
		// (get) Token: 0x0600E5CB RID: 58827 RVA: 0x00430E58 File Offset: 0x0042F058
		public bool IsPlaying
		{
			get
			{
				return this.musicManager.MusicSequenceWorker == this && this.musicManager.IsPlaying;
			}
		}

		// Token: 0x0600E5CC RID: 58828 RVA: 0x00430E75 File Offset: 0x0042F075
		public void InitializeWorker(MusicSequenceDef def, MusicManagerPlay musicManager)
		{
			this.def = def;
			this.musicManager = musicManager;
			MusicSequenceWorker.songs.Clear();
			if (def.songs != null)
			{
				MusicSequenceWorker.songs.AddRange(def.songs);
			}
			this.Shuffle();
		}

		// Token: 0x0600E5CD RID: 58829 RVA: 0x00430EB0 File Offset: 0x0042F0B0
		public virtual bool ShouldTransition()
		{
			return (!this.IsPlaying && !this.def.transitionOnDanger && !this.def.transitionOnNoDanger) || (this.def.transitionOnDanger && this.musicManager.DangerMusicMode) || (this.def.transitionOnNoDanger && !this.musicManager.DangerMusicMode);
		}

		// Token: 0x0600E5CE RID: 58830 RVA: 0x00430F18 File Offset: 0x0042F118
		public virtual bool ShouldLoop()
		{
			return this.def.loop;
		}

		// Token: 0x0600E5CF RID: 58831 RVA: 0x00430F28 File Offset: 0x0042F128
		public virtual bool ShouldEnd()
		{
			return this.MinTimeSatisfied() && ((this.def.endOnDanger && this.musicManager.DangerMusicMode) || (this.def.endOnNoDanger && !this.musicManager.DangerMusicMode));
		}

		// Token: 0x0600E5D0 RID: 58832 RVA: 0x00430F78 File Offset: 0x0042F178
		public virtual bool CanBeInterrupted()
		{
			return this.def.canBeInterrupted;
		}

		// Token: 0x0600E5D1 RID: 58833 RVA: 0x00430F88 File Offset: 0x0042F188
		public virtual SongDef SelectSong()
		{
			if (this.def.song != null)
			{
				return this.def.song;
			}
			if (this.timesLooped % MusicSequenceWorker.songs.Count == 0)
			{
				this.Shuffle();
			}
			return MusicSequenceWorker.songs[this.timesLooped % this.def.songs.Count];
		}

		// Token: 0x0600E5D2 RID: 58834 RVA: 0x00430FE8 File Offset: 0x0042F1E8
		private void Shuffle()
		{
			if (MusicSequenceWorker.songs.Count <= 2)
			{
				return;
			}
			SongDef last = MusicSequenceWorker.songs.GetLast<SongDef>();
			int i = MusicSequenceWorker.songs.Count;
			while (i > 1)
			{
				i--;
				int num = Rand.Range(0, i + 1);
				List<SongDef> list = MusicSequenceWorker.songs;
				int index = num;
				List<SongDef> list2 = MusicSequenceWorker.songs;
				int index2 = i;
				SongDef value = MusicSequenceWorker.songs[i];
				SongDef value2 = MusicSequenceWorker.songs[num];
				list[index] = value;
				list2[index2] = value2;
			}
			if (MusicSequenceWorker.songs[0] == last)
			{
				int num2 = Rand.Range(1, MusicSequenceWorker.songs.Count);
				List<SongDef> list3 = MusicSequenceWorker.songs;
				List<SongDef> list2 = MusicSequenceWorker.songs;
				int index2 = num2;
				SongDef value2 = MusicSequenceWorker.songs[num2];
				SongDef value = MusicSequenceWorker.songs[0];
				list3[0] = value2;
				list2[index2] = value;
			}
		}

		// Token: 0x0600E5D3 RID: 58835 RVA: 0x004310D8 File Offset: 0x0042F2D8
		public bool MinTimeSatisfied()
		{
			return this.def.minTimeToPlay == null || this.timesLooped > 0 || this.musicManager.SongTime >= this.def.minTimeToPlay.Value;
		}

		// Token: 0x040099BF RID: 39359
		private static readonly List<SongDef> songs = new List<SongDef>();
	}
}
