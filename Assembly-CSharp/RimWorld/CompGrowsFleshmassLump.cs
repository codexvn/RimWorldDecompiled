using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CCD RID: 11469
	public class CompGrowsFleshmassLump : ThingComp
	{
		// Token: 0x170028C0 RID: 10432
		// (get) Token: 0x060101A1 RID: 65953 RVA: 0x004A8667 File Offset: 0x004A6867
		private CompProperties_GrowsFleshmassLump Props
		{
			get
			{
				return (CompProperties_GrowsFleshmassLump)this.props;
			}
		}

		// Token: 0x060101A2 RID: 65954 RVA: 0x004A8674 File Offset: 0x004A6874
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.lumpCellIndex, "lumpCellIndex", 0, false);
			Scribe_Values.Look<int>(ref this.nextSpawnTick, "nextSpawnTick", 0, false);
			Scribe_Collections.Look<IntVec3>(ref this.lumpCells, "lumpCells", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x060101A3 RID: 65955 RVA: 0x004A86B0 File Offset: 0x004A68B0
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.nextSpawnTick = Find.TickManager.TicksGame + this.Props.ticksBetweenFleshmassRange.RandomInRange;
				this.lumpCells = GridShapeMaker.IrregularLump(this.parent.Position, this.parent.Map, this.Props.maxFleshmassRange.RandomInRange, null).ToList<IntVec3>();
			}
		}

		// Token: 0x060101A4 RID: 65956 RVA: 0x004A8718 File Offset: 0x004A6918
		public override void CompTick()
		{
			if (this.lumpCellIndex < this.lumpCells.Count - 1 && Find.TickManager.TicksGame >= this.nextSpawnTick)
			{
				IntVec3 intVec = this.lumpCells[this.lumpCellIndex];
				if (intVec.Standable(this.parent.Map))
				{
					GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Fleshmass_Active, null), intVec, this.parent.Map, WipeMode.FullRefund).SetFaction(this.parent.Faction, null);
					EffecterDefOf.MeatExplosion.Spawn(intVec, this.parent.Map, 1f).Cleanup();
				}
				this.nextSpawnTick = Find.TickManager.TicksGame + this.Props.ticksBetweenFleshmassRange.RandomInRange;
				this.lumpCellIndex++;
			}
		}

		// Token: 0x0400A83A RID: 43066
		private int nextSpawnTick;

		// Token: 0x0400A83B RID: 43067
		private List<IntVec3> lumpCells;

		// Token: 0x0400A83C RID: 43068
		private int lumpCellIndex;
	}
}
