using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C55 RID: 11349
	public class CompDestroyAfterDelay : ThingComp
	{
		// Token: 0x17002812 RID: 10258
		// (get) Token: 0x0600FE7F RID: 65151 RVA: 0x0049C73B File Offset: 0x0049A93B
		public int DestructionTick
		{
			get
			{
				return this.spawnTick + this.Props.delayTicks;
			}
		}

		// Token: 0x17002813 RID: 10259
		// (get) Token: 0x0600FE80 RID: 65152 RVA: 0x0049C74F File Offset: 0x0049A94F
		public int TicksLeft
		{
			get
			{
				return this.DestructionTick - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17002814 RID: 10260
		// (get) Token: 0x0600FE81 RID: 65153 RVA: 0x0049C762 File Offset: 0x0049A962
		public CompProperties_DestroyAfterDelay Props
		{
			get
			{
				return (CompProperties_DestroyAfterDelay)this.props;
			}
		}

		// Token: 0x0600FE82 RID: 65154 RVA: 0x0049C76F File Offset: 0x0049A96F
		public override void CompTick()
		{
			base.CompTick();
			if (this.TicksLeft <= 0 && !this.parent.Destroyed)
			{
				this.parent.Destroy(this.Props.destroyMode);
			}
		}

		// Token: 0x0600FE83 RID: 65155 RVA: 0x0049C7A4 File Offset: 0x0049A9A4
		public override string CompInspectStringExtra()
		{
			if (this.Props.countdownLabel.NullOrEmpty())
			{
				return "";
			}
			int numTicks = Mathf.Max(0, this.spawnTick + this.Props.delayTicks - Find.TickManager.TicksGame);
			return this.Props.countdownLabel + ": " + numTicks.ToStringSecondsFromTicks();
		}

		// Token: 0x0600FE84 RID: 65156 RVA: 0x0049C808 File Offset: 0x0049AA08
		public override string TransformLabel(string label)
		{
			if (this.Props.displayCountdownOnLabel)
			{
				return base.TransformLabel(label) + " (" + this.TicksLeft.TicksToSeconds().ToString("F0") + "s)";
			}
			return base.TransformLabel(label);
		}

		// Token: 0x0600FE85 RID: 65157 RVA: 0x0049C858 File Offset: 0x0049AA58
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship)
			{
				this.spawnTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x0600FE86 RID: 65158 RVA: 0x0049C881 File Offset: 0x0049AA81
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
		}

		// Token: 0x0400A6C5 RID: 42693
		public int spawnTick;
	}
}
