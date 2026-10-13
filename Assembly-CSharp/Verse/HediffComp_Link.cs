using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006DD RID: 1757
	public class HediffComp_Link : HediffComp
	{
		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06002B82 RID: 11138 RVA: 0x000E358A File Offset: 0x000E178A
		public HediffCompProperties_Link Props
		{
			get
			{
				return (HediffCompProperties_Link)this.props;
			}
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x06002B83 RID: 11139 RVA: 0x000E3597 File Offset: 0x000E1797
		public Pawn OtherPawn
		{
			get
			{
				return (Pawn)this.other;
			}
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x06002B84 RID: 11140 RVA: 0x000E35A4 File Offset: 0x000E17A4
		public override bool CompShouldRemove
		{
			get
			{
				if (base.CompShouldRemove)
				{
					return true;
				}
				if (this.other == null || !this.parent.pawn.SpawnedOrAnyParentSpawned || !this.other.SpawnedOrAnyParentSpawned)
				{
					return true;
				}
				if (this.Props.maxDistance > 0f && !this.parent.pawn.PositionHeld.InHorDistOf(this.other.PositionHeld, this.Props.maxDistance))
				{
					return true;
				}
				if (this.Props.requireLinkOnOtherPawn)
				{
					Pawn pawn = this.other as Pawn;
					if (pawn != null)
					{
						foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
						{
							HediffWithComps hediffWithComps = hediff as HediffWithComps;
							if (hediffWithComps != null && hediffWithComps.comps.FirstOrDefault(delegate(HediffComp c)
							{
								HediffComp_Link hediffComp_Link = c as HediffComp_Link;
								return hediffComp_Link != null && hediffComp_Link.other == this.parent.pawn && hediffComp_Link.parent.def == this.parent.def;
							}) != null)
							{
								return false;
							}
						}
						return true;
					}
					Log.Error("HediffComp_Link requires link on other pawn, but other thing is not a pawn!");
					return true;
				}
				return false;
			}
		}

		// Token: 0x06002B85 RID: 11141 RVA: 0x000E36C8 File Offset: 0x000E18C8
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (!this.drawConnection || this.other.MapHeld != this.parent.pawn.MapHeld)
			{
				return;
			}
			ThingDef moteDef = this.Props.customMote ?? ThingDefOf.Mote_PsychicLinkLine;
			if (this.mote == null)
			{
				this.mote = MoteMaker.MakeInteractionOverlay(moteDef, this.parent.pawn, this.other);
			}
			this.mote.Maintain();
		}

		// Token: 0x06002B86 RID: 11142 RVA: 0x000E374A File Offset: 0x000E194A
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_References.Look<Thing>(ref this.other, "other", false);
			Scribe_Values.Look<bool>(ref this.drawConnection, "drawConnection", false, false);
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x06002B87 RID: 11143 RVA: 0x000E3775 File Offset: 0x000E1975
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (!this.Props.showName || this.other == null)
				{
					return null;
				}
				return this.other.LabelShort;
			}
		}

		// Token: 0x04002308 RID: 8968
		public Thing other;

		// Token: 0x04002309 RID: 8969
		private MoteDualAttached mote;

		// Token: 0x0400230A RID: 8970
		public bool drawConnection;
	}
}
