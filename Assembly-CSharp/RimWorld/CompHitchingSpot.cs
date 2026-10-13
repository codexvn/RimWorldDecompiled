using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CE2 RID: 11490
	public class CompHitchingSpot : ThingComp
	{
		// Token: 0x06010239 RID: 66105 RVA: 0x004AB773 File Offset: 0x004A9973
		public void AddPawn(Pawn ropedPawn)
		{
			this.ropedPawns.Add(ropedPawn);
		}

		// Token: 0x0601023A RID: 66106 RVA: 0x004AB781 File Offset: 0x004A9981
		public void RemovePawn(Pawn unropedPawn)
		{
			this.ropedPawns.Remove(unropedPawn);
		}

		// Token: 0x170028DD RID: 10461
		// (get) Token: 0x0601023B RID: 66107 RVA: 0x004AB790 File Offset: 0x004A9990
		private Graphic HitchGraphic
		{
			get
			{
				if (this.hitchGraphic == null)
				{
					this.hitchGraphic = GraphicDatabase.Get<Graphic_Single>("Things/Building/Misc/CaravanHitchingPin", ShaderDatabase.Cutout, Vector2.one, Color.white);
				}
				return this.hitchGraphic;
			}
		}

		// Token: 0x0601023C RID: 66108 RVA: 0x004AB7C0 File Offset: 0x004A99C0
		public override void PostDraw()
		{
			base.PostDraw();
			if (this.ropedPawns.Count > 0)
			{
				this.HitchGraphic.DrawFromDef(this.parent.DrawPos + CompHitchingSpot.HitchAdustment + Altitudes.AltIncVect * 0.25f, Rot4.North, null, 0f);
			}
		}

		// Token: 0x0601023D RID: 66109 RVA: 0x004AB820 File Offset: 0x004A9A20
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.parent.Faction == Faction.OfPlayer)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandFormCaravan".Translate(),
					defaultDesc = "CommandFormCaravanDesc".Translate() + "\n\n" + "CommandFormCaravanDescAtSpot".Translate(),
					icon = FormCaravanComp.FormCaravanCommand,
					action = delegate()
					{
						Find.WindowStack.Add(new Dialog_FormCaravan(this.parent.Map, false, null, false, new IntVec3?(this.parent.Position)));
					}
				};
			}
			yield break;
		}

		// Token: 0x0601023E RID: 66110 RVA: 0x004AB830 File Offset: 0x004A9A30
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Collections.Look<Pawn>(ref this.ropedPawns, "ropedPawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				foreach (Pawn pawn in this.ropedPawns)
				{
					if (pawn == null || pawn.roping == null || pawn.roping.RopedToHitchingSpot != this.parent)
					{
						this.RemovePawn(pawn);
					}
				}
			}
		}

		// Token: 0x0400A895 RID: 43157
		private static readonly Vector3 HitchAdustment = new Vector3(0f, 0f, 0.15f);

		// Token: 0x0400A896 RID: 43158
		private List<Pawn> ropedPawns = new List<Pawn>();

		// Token: 0x0400A897 RID: 43159
		private Graphic hitchGraphic;
	}
}
