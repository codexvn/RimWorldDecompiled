using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CDD RID: 11485
	public class CompHasPawnSources : ThingComp
	{
		// Token: 0x170028D9 RID: 10457
		// (get) Token: 0x0601021F RID: 66079 RVA: 0x004AAEBF File Offset: 0x004A90BF
		public CompProperties_HasPawnSources Props
		{
			get
			{
				return (CompProperties_HasPawnSources)this.props;
			}
		}

		// Token: 0x06010220 RID: 66080 RVA: 0x004AAECC File Offset: 0x004A90CC
		public void AddSource(Pawn pawn)
		{
			if (pawn == null || this.pawnSources.Contains(pawn))
			{
				return;
			}
			Find.WorldPawns.AddPawnSource(pawn, this);
			this.pawnSources.Add(pawn);
		}

		// Token: 0x06010221 RID: 66081 RVA: 0x004AAEF8 File Offset: 0x004A90F8
		public override bool AllowStackWith(Thing other)
		{
			CompHasPawnSources compHasPawnSources = other.TryGetComp<CompHasPawnSources>();
			if (compHasPawnSources == null || compHasPawnSources.pawnSources.Count != this.pawnSources.Count)
			{
				return false;
			}
			foreach (Pawn item in this.pawnSources)
			{
				if (!compHasPawnSources.pawnSources.Contains(item))
				{
					return false;
				}
			}
			return base.AllowStackWith(other);
		}

		// Token: 0x06010222 RID: 66082 RVA: 0x004AAF84 File Offset: 0x004A9184
		public override string TransformLabel(string label)
		{
			if (!this.Props.affectLabel || this.pawnSources.NullOrEmpty<Pawn>() || this.pawnSources.Count > 2)
			{
				return label;
			}
			if (this.pawnSources.Count == 2)
			{
				return "ThingOfTwoSources".Translate(label, this.pawnSources[0].LabelShortCap, this.pawnSources[1].LabelShortCap);
			}
			return "ThingOfSource".Translate(label, this.pawnSources[0].LabelShortCap);
		}

		// Token: 0x06010223 RID: 66083 RVA: 0x004AB036 File Offset: 0x004A9236
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			Find.WorldPawns.RemovePawnSources(this.pawnSources, this);
		}

		// Token: 0x06010224 RID: 66084 RVA: 0x004AB054 File Offset: 0x004A9254
		public override void PostSplitOff(Thing piece)
		{
			CompHasPawnSources compHasPawnSources = piece.TryGetComp<CompHasPawnSources>();
			if (compHasPawnSources == null || piece == this.parent)
			{
				return;
			}
			foreach (Pawn pawn in this.pawnSources)
			{
				compHasPawnSources.AddSource(pawn);
			}
		}

		// Token: 0x06010225 RID: 66085 RVA: 0x004AB0BC File Offset: 0x004A92BC
		public override void PostExposeData()
		{
			Scribe_Collections.Look<Pawn>(ref this.pawnSources, "pawnSources", true, LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawnSources.RemoveAll((Pawn x) => x == null);
				foreach (Pawn pawn in this.pawnSources)
				{
					Find.WorldPawns.AddPawnSource(pawn, this);
				}
			}
		}

		// Token: 0x0400A88C RID: 43148
		public List<Pawn> pawnSources = new List<Pawn>();
	}
}
