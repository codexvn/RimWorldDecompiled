using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200072F RID: 1839
	public class HediffWithParents : HediffWithComps
	{
		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06002CD2 RID: 11474 RVA: 0x000E7E07 File Offset: 0x000E6007
		public Pawn Father
		{
			get
			{
				return this.father;
			}
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06002CD3 RID: 11475 RVA: 0x000E7E0F File Offset: 0x000E600F
		public Pawn Mother
		{
			get
			{
				return this.mother;
			}
		}

		// Token: 0x06002CD4 RID: 11476 RVA: 0x000E7E17 File Offset: 0x000E6017
		public void SetParents(Pawn mother, Pawn father, GeneSet geneSet)
		{
			this.mother = mother;
			this.father = father;
			this.geneSet = geneSet;
			Find.WorldPawns.AddPreservedPawnHediff(mother, this);
			Find.WorldPawns.AddPreservedPawnHediff(father, this);
		}

		// Token: 0x06002CD5 RID: 11477 RVA: 0x000E7E46 File Offset: 0x000E6046
		public override void PreRemoved()
		{
			base.PreRemoved();
			Find.WorldPawns.RemovePreservedPawnHediff(this.mother, this);
			Find.WorldPawns.RemovePreservedPawnHediff(this.father, this);
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x000E7E70 File Offset: 0x000E6070
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.geneSet != null)
			{
				foreach (StatDrawEntry statDrawEntry2 in this.geneSet.SpecialDisplayStats(null))
				{
					yield return statDrawEntry2;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x000E7E88 File Offset: 0x000E6088
		public override void CopyFrom(Hediff other)
		{
			base.CopyFrom(other);
			HediffWithParents hediffWithParents = other as HediffWithParents;
			if (hediffWithParents != null)
			{
				Pawn pawn = hediffWithParents.Mother;
				if (hediffWithParents.Mother == hediffWithParents.pawn)
				{
					pawn = this.pawn;
				}
				Pawn pawn2 = pawn;
				Pawn pawn3 = hediffWithParents.Father;
				GeneSet geneSet = hediffWithParents.geneSet;
				this.SetParents(pawn2, pawn3, (geneSet != null) ? geneSet.Copy() : null);
			}
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x000E7EE1 File Offset: 0x000E60E1
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (ModsConfig.BiotechActive && !this.pawn.Drafted)
			{
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "InspectBabyGenes".Translate() + "...";
				command_Action.defaultDesc = "InspectGenesHediffDesc".Translate();
				command_Action.icon = GeneSetHolderBase.GeneticInfoTex.Texture;
				command_Action.action = delegate()
				{
					InspectPaneUtility.OpenTab(typeof(ITab_GenesPregnancy));
				};
				yield return command_Action;
			}
			yield break;
			yield break;
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x000E7EF4 File Offset: 0x000E60F4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.father, "father", true);
			Scribe_References.Look<Pawn>(ref this.mother, "mother", true);
			Scribe_Deep.Look<GeneSet>(ref this.geneSet, "geneSet", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Find.WorldPawns.AddPreservedPawnHediff(this.mother, this);
				Find.WorldPawns.AddPreservedPawnHediff(this.father, this);
			}
		}

		// Token: 0x040023B8 RID: 9144
		private Pawn father;

		// Token: 0x040023B9 RID: 9145
		private Pawn mother;

		// Token: 0x040023BA RID: 9146
		public GeneSet geneSet;
	}
}
