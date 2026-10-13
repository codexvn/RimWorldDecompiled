using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A9D RID: 6813
	public class MusicSequenceDef : Def
	{
		// Token: 0x06009891 RID: 39057 RVA: 0x002C8097 File Offset: 0x002C6297
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.workerType != null && !typeof(MusicSequenceWorker).IsAssignableFrom(this.workerType))
			{
				yield return "Music condition type is not a subclass of MusicSequenceWorker, type was: " + this.workerType.FullName;
			}
			if (this.song != null && this.songs != null && this.workerType == typeof(MusicSequenceWorker))
			{
				yield return "You are using both a single song and a list, multiple songs won't play with default worker implementation, use a custom worker or remove one of the two options. In sequence def: " + this.defName;
			}
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040066E4 RID: 26340
		public Type workerType = typeof(MusicSequenceWorker);

		// Token: 0x040066E5 RID: 26341
		public SongDef song;

		// Token: 0x040066E6 RID: 26342
		public List<SongDef> songs;

		// Token: 0x040066E7 RID: 26343
		public MusicSequenceDef nextSequence;

		// Token: 0x040066E8 RID: 26344
		public bool loop;

		// Token: 0x040066E9 RID: 26345
		public bool canBeInterrupted = true;

		// Token: 0x040066EA RID: 26346
		public bool useTransitionForLifetime = true;

		// Token: 0x040066EB RID: 26347
		public bool transitionOnDanger;

		// Token: 0x040066EC RID: 26348
		public bool transitionOnNoDanger;

		// Token: 0x040066ED RID: 26349
		public bool endOnNoDanger;

		// Token: 0x040066EE RID: 26350
		public bool endOnDanger;

		// Token: 0x040066EF RID: 26351
		public FloatRange loopDelayRange = FloatRange.Zero;

		// Token: 0x040066F0 RID: 26352
		public float fadeoutLastSongDuration = 10f;

		// Token: 0x040066F1 RID: 26353
		public float fadeInDuration;

		// Token: 0x040066F2 RID: 26354
		public bool loopFadeout;

		// Token: 0x040066F3 RID: 26355
		public float? overrideFadeout;

		// Token: 0x040066F4 RID: 26356
		public float? overrideVolume;

		// Token: 0x040066F5 RID: 26357
		public float? minTimeToPlay;

		// Token: 0x040066F6 RID: 26358
		public float? pausedVolumeFactor;
	}
}
