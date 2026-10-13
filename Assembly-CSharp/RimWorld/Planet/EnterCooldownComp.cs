using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E26 RID: 15910
	public class EnterCooldownComp : WorldObjectComp
	{
		// Token: 0x1700363C RID: 13884
		// (get) Token: 0x06015BAE RID: 89006 RVA: 0x0067FC9B File Offset: 0x0067DE9B
		public WorldObjectCompProperties_EnterCooldown Props
		{
			get
			{
				return (WorldObjectCompProperties_EnterCooldown)this.props;
			}
		}

		// Token: 0x1700363D RID: 13885
		// (get) Token: 0x06015BAF RID: 89007 RVA: 0x0067FCA8 File Offset: 0x0067DEA8
		public bool Active
		{
			get
			{
				return this.endTick > GenTicks.TicksGame;
			}
		}

		// Token: 0x1700363E RID: 13886
		// (get) Token: 0x06015BB0 RID: 89008 RVA: 0x0067FCB7 File Offset: 0x0067DEB7
		public bool BlocksEntering
		{
			get
			{
				return this.Active && !base.ParentHasMap;
			}
		}

		// Token: 0x1700363F RID: 13887
		// (get) Token: 0x06015BB1 RID: 89009 RVA: 0x0067FCCC File Offset: 0x0067DECC
		public int TicksLeft
		{
			get
			{
				if (!this.Active)
				{
					return 0;
				}
				return this.endTick - GenTicks.TicksGame;
			}
		}

		// Token: 0x17003640 RID: 13888
		// (get) Token: 0x06015BB2 RID: 89010 RVA: 0x0067FCE4 File Offset: 0x0067DEE4
		public float DaysLeft
		{
			get
			{
				return (float)this.TicksLeft / 60000f;
			}
		}

		// Token: 0x06015BB3 RID: 89011 RVA: 0x0067FCF4 File Offset: 0x0067DEF4
		public void Start(float? durationDays = null)
		{
			float num = durationDays ?? this.Props.durationDays;
			this.endTick = GenTicks.TicksGame + Mathf.RoundToInt(num * 60000f);
		}

		// Token: 0x06015BB4 RID: 89012 RVA: 0x0067FD39 File Offset: 0x0067DF39
		public void Stop()
		{
			this.endTick = 0;
		}

		// Token: 0x06015BB5 RID: 89013 RVA: 0x0067FD42 File Offset: 0x0067DF42
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			if (this.Active)
			{
				this.Stop();
			}
		}

		// Token: 0x06015BB6 RID: 89014 RVA: 0x0067FD58 File Offset: 0x0067DF58
		public override void PostMyMapRemoved()
		{
			base.PostMyMapRemoved();
			if (this.Props.autoStartOnMapRemoved)
			{
				this.Start(null);
			}
		}

		// Token: 0x06015BB7 RID: 89015 RVA: 0x0067FD88 File Offset: 0x0067DF88
		public override void PostExposeData()
		{
			base.PostExposeData();
			int num = 0;
			Scribe_Values.Look<int>(ref num, "ticksLeft", 0, false);
			Scribe_Values.Look<int>(ref this.endTick, "endTick", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && num > 0 && this.endTick == 0)
			{
				this.endTick = GenTicks.TicksGame + num;
			}
		}

		// Token: 0x06015BB8 RID: 89016 RVA: 0x0067FDDE File Offset: 0x0067DFDE
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Set enter cooldown to 1 hour",
					action = delegate()
					{
						this.endTick = GenTicks.TicksGame + 2500;
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Reset enter cooldown",
					action = delegate()
					{
						this.endTick = GenTicks.TicksGame;
					}
				};
			}
			yield break;
		}

		// Token: 0x0400F13A RID: 61754
		private int endTick;
	}
}
