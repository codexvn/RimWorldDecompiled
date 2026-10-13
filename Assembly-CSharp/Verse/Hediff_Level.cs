using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000750 RID: 1872
	public class Hediff_Level : HediffWithComps
	{
		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06002DC4 RID: 11716 RVA: 0x000EB96C File Offset: 0x000E9B6C
		public override string Label
		{
			get
			{
				if (!this.def.levelIsQuantity)
				{
					return this.def.label + " (" + "LevelNum".Translate(this.level).ToString() + ")";
				}
				return this.def.label + " x" + this.level.ToString();
			}
		}

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06002DC5 RID: 11717 RVA: 0x000EB9E4 File Offset: 0x000E9BE4
		public override bool ShouldRemove
		{
			get
			{
				return this.level == 0;
			}
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x000EB9EF File Offset: 0x000E9BEF
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (base.Part == null)
			{
				Log.Error(this.def.defName + " has null Part. It should be set before PostAdd.");
			}
		}

		// Token: 0x06002DC7 RID: 11719 RVA: 0x000EBA1A File Offset: 0x000E9C1A
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			this.Severity = (float)this.level;
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x000EBA30 File Offset: 0x000E9C30
		public virtual void ChangeLevel(int levelOffset)
		{
			this.level = (int)Mathf.Clamp((float)(this.level + levelOffset), this.def.minSeverity, this.def.maxSeverity);
		}

		// Token: 0x06002DC9 RID: 11721 RVA: 0x000EBA5D File Offset: 0x000E9C5D
		public virtual void SetLevelTo(int targetLevel)
		{
			if (targetLevel != this.level)
			{
				this.ChangeLevel(targetLevel - this.level);
			}
		}

		// Token: 0x06002DCA RID: 11722 RVA: 0x000EBA78 File Offset: 0x000E9C78
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.level, "level", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error(base.GetType().Name + " has null part after loading.");
				this.pawn.health.hediffSet.hediffs.Remove(this);
			}
		}

		// Token: 0x0400241E RID: 9246
		public int level = 1;
	}
}
