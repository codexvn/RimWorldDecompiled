using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016E2 RID: 5858
	public class LordToilData_AssaultColonyBreaching : LordToilData
	{
		// Token: 0x06008A41 RID: 35393 RVA: 0x00293C55 File Offset: 0x00291E55
		public LordToilData_AssaultColonyBreaching()
		{
		}

		// Token: 0x06008A42 RID: 35394 RVA: 0x00293C7E File Offset: 0x00291E7E
		public LordToilData_AssaultColonyBreaching(Lord lord)
		{
			this.breachingGrid = new BreachingGrid(lord.Map, lord);
		}

		// Token: 0x06008A43 RID: 35395 RVA: 0x00293CB9 File Offset: 0x00291EB9
		public void Reset()
		{
			this.breachDest = IntVec3.Invalid;
			this.breachStart = IntVec3.Invalid;
			this.currentTarget = null;
			this.soloAttacker = null;
			this.breachingGrid.Reset();
		}

		// Token: 0x06008A44 RID: 35396 RVA: 0x00293CEC File Offset: 0x00291EEC
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.breachDest, "breachDest", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.breachStart, "breachStart", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.preferMelee, "preferMelee", false, false);
			Scribe_Deep.Look<BreachingGrid>(ref this.breachingGrid, "breachingGrid", Array.Empty<object>());
			Scribe_References.Look<Thing>(ref this.currentTarget, "currentTarget", false);
			Scribe_References.Look<Pawn>(ref this.soloAttacker, "soloAttacker", false);
			Scribe_Values.Look<float>(ref this.maxRange, "maxRange", 0f, false);
		}

		// Token: 0x04005A1B RID: 23067
		public IntVec3 breachDest = IntVec3.Invalid;

		// Token: 0x04005A1C RID: 23068
		public IntVec3 breachStart = IntVec3.Invalid;

		// Token: 0x04005A1D RID: 23069
		public bool preferMelee;

		// Token: 0x04005A1E RID: 23070
		public BreachingGrid breachingGrid;

		// Token: 0x04005A1F RID: 23071
		public Thing currentTarget;

		// Token: 0x04005A20 RID: 23072
		public Pawn soloAttacker;

		// Token: 0x04005A21 RID: 23073
		public float maxRange = 12f;
	}
}
