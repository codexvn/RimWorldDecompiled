using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000790 RID: 1936
	public class InfectionPathway : IExposable
	{
		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x06002F9A RID: 12186 RVA: 0x000F4A1B File Offset: 0x000F2C1B
		public InfectionPathwayDef Def
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x06002F9B RID: 12187 RVA: 0x000F4A23 File Offset: 0x000F2C23
		public bool SourceWasPawn
		{
			get
			{
				return this.sourcePawnKind != null;
			}
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06002F9C RID: 12188 RVA: 0x000F4A2E File Offset: 0x000F2C2E
		public PawnKindDef SourcePawnKind
		{
			get
			{
				return this.sourcePawnKind;
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x06002F9D RID: 12189 RVA: 0x000F4A36 File Offset: 0x000F2C36
		public Pawn OwnerPawn
		{
			get
			{
				return this.ownerPawn;
			}
		}

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06002F9E RID: 12190 RVA: 0x000F4A3E File Offset: 0x000F2C3E
		public int AgeTicks
		{
			get
			{
				return this.ageTicks;
			}
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06002F9F RID: 12191 RVA: 0x000F4A46 File Offset: 0x000F2C46
		public bool Expired
		{
			get
			{
				return this.ageTicks > this.def.ExpiryTicks;
			}
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06002FA0 RID: 12192 RVA: 0x000F4A5B File Offset: 0x000F2C5B
		public float AgePercent
		{
			get
			{
				return Mathf.Clamp01((float)this.ageTicks / (float)this.def.ExpiryTicks);
			}
		}

		// Token: 0x06002FA1 RID: 12193 RVA: 0x000024DF File Offset: 0x000006DF
		public InfectionPathway()
		{
		}

		// Token: 0x06002FA2 RID: 12194 RVA: 0x000F4A76 File Offset: 0x000F2C76
		public InfectionPathway(InfectionPathwayDef def, Pawn ownerPawn, PawnKindDef sourcePawnKind = null)
		{
			this.def = def;
			this.sourcePawnKind = sourcePawnKind;
			this.ownerPawn = ownerPawn;
		}

		// Token: 0x06002FA3 RID: 12195 RVA: 0x000F4A94 File Offset: 0x000F2C94
		public string GetExplanation(HediffDef hediff)
		{
			foreach (HediffInfectionPathway hediffInfectionPathway in hediff.possiblePathways)
			{
				if (hediffInfectionPathway.PathwayDef == this.def)
				{
					if (this.sourcePawnKind != null)
					{
						return hediffInfectionPathway.Explanation.Formatted(this.sourcePawnKind.Named("SOURCEKIND"));
					}
					return hediffInfectionPathway.Explanation.Formatted(Array.Empty<NamedArgument>());
				}
			}
			Log.ErrorOnce("Failed to get an explanation for infection vector " + this.def.defName + " for hediff def " + hediff.defName, 85784634);
			return null;
		}

		// Token: 0x06002FA4 RID: 12196 RVA: 0x000F4B60 File Offset: 0x000F2D60
		public void TickInterval(int delta)
		{
			this.ageTicks += delta;
		}

		// Token: 0x06002FA5 RID: 12197 RVA: 0x000F4B70 File Offset: 0x000F2D70
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ageTicks, "ageTicks", 0, false);
			Scribe_Defs.Look<InfectionPathwayDef>(ref this.def, "def");
			Scribe_Defs.Look<PawnKindDef>(ref this.sourcePawnKind, "sourcePawnKind");
			Scribe_References.Look<Pawn>(ref this.ownerPawn, "ownerPawn", false);
		}

		// Token: 0x04002508 RID: 9480
		private InfectionPathwayDef def;

		// Token: 0x04002509 RID: 9481
		private int ageTicks;

		// Token: 0x0400250A RID: 9482
		private PawnKindDef sourcePawnKind;

		// Token: 0x0400250B RID: 9483
		private Pawn ownerPawn;
	}
}
