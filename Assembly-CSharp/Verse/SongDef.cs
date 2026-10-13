using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002E2 RID: 738
	public class SongDef : Def
	{
		// Token: 0x06001225 RID: 4645 RVA: 0x00054708 File Offset: 0x00052908
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.defName == "UnnamedDef")
			{
				string[] array = this.clipPath.Split(new char[]
				{
					'/',
					'\\'
				});
				this.defName = array[array.Length - 1];
				base.ResolveDefNameHash();
			}
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x0005475C File Offset: 0x0005295C
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.clip = ContentFinder<AudioClip>.Get(this.clipPath, true);
			});
		}

		// Token: 0x04001061 RID: 4193
		[NoTranslate]
		public string clipPath;

		// Token: 0x04001062 RID: 4194
		public float volume = 1f;

		// Token: 0x04001063 RID: 4195
		public bool playOnMap = true;

		// Token: 0x04001064 RID: 4196
		public float commonality = 1f;

		// Token: 0x04001065 RID: 4197
		public bool tense;

		// Token: 0x04001066 RID: 4198
		public TimeOfDay allowedTimeOfDay = TimeOfDay.Any;

		// Token: 0x04001067 RID: 4199
		public List<Season> allowedSeasons;

		// Token: 0x04001068 RID: 4200
		public RoyalTitleDef minRoyalTitle;

		// Token: 0x04001069 RID: 4201
		[Unsaved(false)]
		public AudioClip clip;
	}
}
