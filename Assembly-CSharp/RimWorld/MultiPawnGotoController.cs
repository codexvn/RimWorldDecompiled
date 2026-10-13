using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020033DA RID: 13274
	[StaticConstructorOnStartup]
	public class MultiPawnGotoController
	{
		// Token: 0x170030D3 RID: 12499
		// (get) Token: 0x06012A6B RID: 76395 RVA: 0x00584C43 File Offset: 0x00582E43
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x06012A6C RID: 76396 RVA: 0x00584C4B File Offset: 0x00582E4B
		public void Deactivate()
		{
			this.active = false;
			this.pawns.Clear();
			this.dests.Clear();
			this.lastUpdateTicks = null;
		}

		// Token: 0x06012A6D RID: 76397 RVA: 0x00584C78 File Offset: 0x00582E78
		public void StartInteraction(IntVec3 mouseCell)
		{
			this.Deactivate();
			this.end = mouseCell;
			this.start = mouseCell;
			this.lastUpdateTicks = null;
		}

		// Token: 0x06012A6E RID: 76398 RVA: 0x00584CA7 File Offset: 0x00582EA7
		public void FinalizeInteraction()
		{
			this.RecomputeDestinations();
			this.IssueGotoJobs();
			this.Deactivate();
		}

		// Token: 0x06012A6F RID: 76399 RVA: 0x00584CBB File Offset: 0x00582EBB
		public void AddPawn(Pawn pawn)
		{
			this.active = true;
			this.pawns.Add(pawn);
			this.dests.Add(IntVec3.Invalid);
			this.lastUpdateTicks = null;
		}

		// Token: 0x06012A70 RID: 76400 RVA: 0x00584CEC File Offset: 0x00582EEC
		public void ProcessInputEvents()
		{
			IntVec3 intVec = UI.MouseCell();
			if (!this.Active || !this.pawns.Any<Pawn>() || !this.pawns[0].Spawned || !intVec.InBounds(this.pawns[0].Map))
			{
				return;
			}
			int ticksGame = Find.TickManager.TicksGame;
			if (!(intVec != this.end) && this.lastUpdateTicks != null)
			{
				float num = (float)ticksGame;
				int? num2 = this.lastUpdateTicks;
				float? num3 = (num2 != null) ? new float?((float)num2.GetValueOrDefault() + 10f) : null;
				if (!(num > num3.GetValueOrDefault() & num3 != null))
				{
					return;
				}
			}
			if (intVec != this.end)
			{
				SoundDefOf.DragGoto.PlayOneShotOnCamera(null);
			}
			this.end = intVec;
			this.lastUpdateTicks = new int?(ticksGame);
			this.RecomputeDestinations();
		}

		// Token: 0x06012A71 RID: 76401 RVA: 0x00584DE0 File Offset: 0x00582FE0
		public void RecomputeDestinations()
		{
			for (int i = 0; i < this.dests.Count; i++)
			{
				this.dests[i] = IntVec3.Invalid;
			}
			float num = (float)((this.pawns.Count > 1) ? (this.pawns.Count - 1) : 1);
			for (int j = 0; j < this.pawns.Count; j++)
			{
				Pawn pawn = this.pawns[j];
				if (pawn.Spawned)
				{
					IntVec3 root;
					if (pawn.Map.exitMapGrid.IsExitCell(this.end))
					{
						root = this.end;
					}
					else
					{
						float d = (float)j / num;
						root = (this.start.ToVector3() + (this.end.ToVector3() - this.start.ToVector3()) * d).ToIntVec3();
					}
					IntVec3 intVec = RCellFinder.BestOrderedGotoDestNear(root, pawn, (IntVec3 c) => this.<RecomputeDestinations>g__CanGoTo|27_0(pawn, c), true);
					if (ModsConfig.BiotechActive && pawn.IsColonyMech && !MechanitorUtility.InMechanitorCommandRange(pawn, intVec))
					{
						intVec = IntVec3.Invalid;
					}
					this.dests[j] = intVec;
				}
			}
		}

		// Token: 0x06012A72 RID: 76402 RVA: 0x00584F40 File Offset: 0x00583140
		private void IssueGotoJobs()
		{
			for (int i = 0; i < this.pawns.Count; i++)
			{
				Pawn pawn = this.pawns[i];
				IntVec3 gotoLoc = this.dests[i];
				if (gotoLoc.IsValid)
				{
					FloatMenuOptionProvider_DraftedMove.PawnGotoAction(this.end, pawn, gotoLoc);
				}
			}
			SoundDefOf.ColonistOrdered.PlayOneShotOnCamera(null);
			if (this.start == this.end)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.GroupGotoHereDragging, OpportunityType.GoodToKnow);
				return;
			}
			if ((float)this.start.DistanceToSquared(this.end) > 1.9f)
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GroupGotoHereDragging, KnowledgeAmount.SpecificInteraction);
			}
		}

		// Token: 0x06012A73 RID: 76403 RVA: 0x00584FE4 File Offset: 0x005831E4
		public void Draw()
		{
			if (!this.active)
			{
				return;
			}
			Vector3 s = new Vector3(1.7f, 1f, 1.7f);
			float num = AltitudeLayer.MetaOverlays.AltitudeFor();
			float addedAltitude = num + 0.03658537f;
			float addedAltitude2 = num - 0.03658537f;
			for (int i = 0; i < this.pawns.Count; i++)
			{
				Pawn pawn = this.pawns[i];
				IntVec3 c = this.dests[i];
				if (c.IsValid && pawn.Spawned && !c.Fogged(pawn.Map))
				{
					Vector3 drawLoc = c.ToVector3ShiftedWithAltitude(num);
					pawn.Drawer.renderer.RenderPawnAt(drawLoc, new Rot4?(Rot4.South), false);
					Vector3 pos = c.ToVector3ShiftedWithAltitude(addedAltitude);
					Graphics.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(pos, Quaternion.identity, s), MultiPawnGotoController.GotoCircleMaterial, 0);
				}
			}
			Vector3 a = this.start.ToVector3ShiftedWithAltitude(addedAltitude2);
			Vector3 b = this.end.ToVector3ShiftedWithAltitude(addedAltitude2);
			GenDraw.DrawLineBetween(a, b, MultiPawnGotoController.GotoBetweenLineMaterial, 0.9f);
		}

		// Token: 0x06012A74 RID: 76404 RVA: 0x00585104 File Offset: 0x00583304
		public void OnGUI()
		{
			for (int i = 0; i < this.pawns.Count; i++)
			{
				Pawn pawn = this.pawns[i];
				IntVec3 c = this.dests[i];
				if (c.IsValid && pawn.Spawned && !c.Fogged(pawn.Map))
				{
					Rect rect = c.ToUIRect();
					Vector2 pos = new Vector2(rect.center.x, rect.yMax + 5f);
					GenMapUI.DrawPawnLabel(pawn, pos, 0.5f, 9999f, null, GameFont.Tiny, true, true);
				}
			}
		}

		// Token: 0x06012A77 RID: 76407 RVA: 0x0058526D File Offset: 0x0058346D
		[CompilerGenerated]
		private bool <RecomputeDestinations>g__CanGoTo|27_0(Pawn pawn, IntVec3 c)
		{
			return !this.dests.Contains(c) && (!ModsConfig.BiotechActive || !pawn.IsColonyMech || MechanitorUtility.InMechanitorCommandRange(pawn, c));
		}

		// Token: 0x0400C1A8 RID: 49576
		private const float PawnLabelOffsetY = 5f;

		// Token: 0x0400C1A9 RID: 49577
		private const float PawnLabelAlpha = 0.5f;

		// Token: 0x0400C1AA RID: 49578
		private static readonly Color FeedbackColor = GenColor.FromBytes(153, 207, 135, 255);

		// Token: 0x0400C1AB RID: 49579
		private const float GotoCircleScale = 1.7f;

		// Token: 0x0400C1AC RID: 49580
		private const float GotoCircleAlpha = 0.4f;

		// Token: 0x0400C1AD RID: 49581
		private static readonly Color GotoCircleColor = MultiPawnGotoController.FeedbackColor * new Color(1f, 1f, 1f, 0.4f);

		// Token: 0x0400C1AE RID: 49582
		private const string GotoCircleTexPath = "UI/Overlays/Circle75Solid";

		// Token: 0x0400C1AF RID: 49583
		private static readonly Material GotoCircleMaterial = MaterialPool.MatFrom("UI/Overlays/Circle75Solid", ShaderDatabase.Transparent, MultiPawnGotoController.GotoCircleColor);

		// Token: 0x0400C1B0 RID: 49584
		private const float GotoBetweenLineWidth = 0.9f;

		// Token: 0x0400C1B1 RID: 49585
		private const float GotoBetweenLineAlpha = 0.18f;

		// Token: 0x0400C1B2 RID: 49586
		private const string GotoBetweenLineTexPath = "UI/Overlays/ThickLine";

		// Token: 0x0400C1B3 RID: 49587
		private static readonly Color GotoBetweenLineColor = MultiPawnGotoController.FeedbackColor * new Color(1f, 1f, 1f, 0.18f);

		// Token: 0x0400C1B4 RID: 49588
		private static readonly Material GotoBetweenLineMaterial = MaterialPool.MatFrom("UI/Overlays/ThickLine", ShaderDatabase.Transparent, MultiPawnGotoController.GotoBetweenLineColor);

		// Token: 0x0400C1B5 RID: 49589
		private const float RecomputeDestinationsFrequencyTicks = 10f;

		// Token: 0x0400C1B6 RID: 49590
		private bool active;

		// Token: 0x0400C1B7 RID: 49591
		private IntVec3 start;

		// Token: 0x0400C1B8 RID: 49592
		private IntVec3 end;

		// Token: 0x0400C1B9 RID: 49593
		private int? lastUpdateTicks;

		// Token: 0x0400C1BA RID: 49594
		private readonly List<Pawn> pawns = new List<Pawn>();

		// Token: 0x0400C1BB RID: 49595
		private readonly List<IntVec3> dests = new List<IntVec3>();
	}
}
