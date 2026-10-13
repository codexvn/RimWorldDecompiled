using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200226D RID: 8813
	public class CompAbilityEffect_Resurrect : CompAbilityEffect
	{
		// Token: 0x17001D58 RID: 7512
		// (get) Token: 0x0600C011 RID: 49169 RVA: 0x0037A6D1 File Offset: 0x003788D1
		public new CompProperties_Resurrect Props
		{
			get
			{
				return (CompProperties_Resurrect)this.props;
			}
		}

		// Token: 0x0600C012 RID: 49170 RVA: 0x0037A6E0 File Offset: 0x003788E0
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn innerPawn = ((Corpse)target.Thing).InnerPawn;
			if (ResurrectionUtility.TryResurrectWithSideEffects(innerPawn))
			{
				Messages.Message("MessagePawnResurrected".Translate(innerPawn), innerPawn, MessageTypeDefOf.PositiveEvent, true);
				MoteMaker.MakeAttachedOverlay(innerPawn, ThingDefOf.Mote_ResurrectFlash, Vector3.zero, 1f, -1f);
			}
		}

		// Token: 0x0600C013 RID: 49171 RVA: 0x0037A750 File Offset: 0x00378950
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			if (target.HasThing)
			{
				Corpse corpse = target.Thing as Corpse;
				if (corpse != null && corpse.GetRotStage() == RotStage.Dessicated)
				{
					if (throwMessages)
					{
						Messages.Message("MessageCannotResurrectDessicatedCorpse".Translate(), corpse, MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
			}
			return base.Valid(target, throwMessages);
		}
	}
}
