using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F6F RID: 3951
	public class MentalState_TargetedInsultingSpree : MentalState_InsultingSpree
	{
		// Token: 0x17000F73 RID: 3955
		// (get) Token: 0x060061AD RID: 25005 RVA: 0x001EE9AC File Offset: 0x001ECBAC
		public override string InspectLine
		{
			get
			{
				return string.Format(this.def.baseInspectLine, this.target.LabelShort);
			}
		}

		// Token: 0x17000F74 RID: 3956
		// (get) Token: 0x060061AE RID: 25006 RVA: 0x001EE9C9 File Offset: 0x001ECBC9
		protected override bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return this.insultedTargetAtLeastOnce;
			}
		}

		// Token: 0x060061AF RID: 25007 RVA: 0x001EE9D4 File Offset: 0x001ECBD4
		public override void MentalStateTick(int delta)
		{
			if (this.target != null && (!this.target.Spawned || !this.pawn.CanReach(this.target, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) || !this.target.Awake()))
			{
				Pawn target = this.target;
				if (!this.TryFindNewTarget())
				{
					base.RecoverFromState();
					return;
				}
				Messages.Message("MessageTargetedInsultingSpreeChangedTarget".Translate(this.pawn.LabelShort, target.Label, this.target.Label, this.pawn.Named("PAWN"), target.Named("OLDTARGET"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true), this.pawn, MessageTypeDefOf.NegativeEvent, true);
				base.MentalStateTick(delta);
				return;
			}
			else
			{
				if (this.target == null || !InsultingSpreeMentalStateUtility.CanChaseAndInsult(this.pawn, this.target, false, false))
				{
					base.RecoverFromState();
					return;
				}
				base.MentalStateTick(delta);
				return;
			}
		}

		// Token: 0x060061B0 RID: 25008 RVA: 0x001EEAFE File Offset: 0x001ECCFE
		public override void PreStart()
		{
			base.PreStart();
			this.TryFindNewTarget();
		}

		// Token: 0x060061B1 RID: 25009 RVA: 0x001EEB0D File Offset: 0x001ECD0D
		private bool TryFindNewTarget()
		{
			InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(this.pawn, MentalState_TargetedInsultingSpree.candidates, false);
			bool result = MentalState_TargetedInsultingSpree.candidates.TryRandomElement(out this.target);
			MentalState_TargetedInsultingSpree.candidates.Clear();
			return result;
		}

		// Token: 0x060061B2 RID: 25010 RVA: 0x001EEB3C File Offset: 0x001ECD3C
		public override void PostEnd()
		{
			base.PostEnd();
			if (this.target != null && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Messages.Message("MessageNoLongerOnTargetedInsultingSpree".Translate(this.pawn.LabelShort, this.target.Label, this.pawn.Named("PAWN"), this.target.Named("TARGET")), this.pawn, MessageTypeDefOf.SituationResolved, true);
			}
		}

		// Token: 0x060061B3 RID: 25011 RVA: 0x001EEBCC File Offset: 0x001ECDCC
		public override TaggedString GetBeginLetterText()
		{
			if (this.target == null)
			{
				Log.Error("No target. This should have been checked in this mental state's worker.");
				return "";
			}
			return this.def.beginLetter.Formatted(this.pawn.NameShortColored, this.target.NameShortColored, this.pawn.Named("PAWN"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true).Resolve().CapitalizeFirst();
		}

		// Token: 0x04004639 RID: 17977
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
