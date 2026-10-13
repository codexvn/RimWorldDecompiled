using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002294 RID: 8852
	public class CompAbilityEffect_Teleport : CompAbilityEffect_WithDest
	{
		// Token: 0x17001D78 RID: 7544
		// (get) Token: 0x0600C0B0 RID: 49328 RVA: 0x0037CC4B File Offset: 0x0037AE4B
		public new CompProperties_AbilityTeleport Props
		{
			get
			{
				return (CompProperties_AbilityTeleport)this.props;
			}
		}

		// Token: 0x0600C0B1 RID: 49329 RVA: 0x0037CC58 File Offset: 0x0037AE58
		public override IEnumerable<PreCastAction> GetPreCastActions()
		{
			yield return new PreCastAction
			{
				action = delegate(LocalTargetInfo t, LocalTargetInfo d)
				{
					if (!this.parent.def.HasAreaOfEffect)
					{
						Pawn pawn = t.Pawn;
						if (pawn != null)
						{
							FleckCreationData dataAttachedOverlay = FleckMaker.GetDataAttachedOverlay(pawn, FleckDefOf.PsycastSkipFlashEntry, new Vector3(-0.5f, 0f, -0.5f), 1f, -1f);
							dataAttachedOverlay.link.detachAfterTicks = 5;
							pawn.Map.flecks.CreateFleck(dataAttachedOverlay);
						}
						else
						{
							FleckMaker.Static(t.CenterVector3, this.parent.pawn.Map, FleckDefOf.PsycastSkipFlashEntry, 1f);
						}
						FleckMaker.Static(d.Cell, this.parent.pawn.Map, FleckDefOf.PsycastSkipInnerExit, 1f);
					}
					if (this.Props.destination != AbilityEffectDestination.RandomInRange)
					{
						FleckMaker.Static(d.Cell, this.parent.pawn.Map, FleckDefOf.PsycastSkipOuterRingExit, 1f);
					}
					if (!this.parent.def.HasAreaOfEffect)
					{
						SoundDefOf.Psycast_Skip_Entry.PlayOneShot(new TargetInfo(t.Cell, this.parent.pawn.Map, false));
						SoundDefOf.Psycast_Skip_Exit.PlayOneShot(new TargetInfo(d.Cell, this.parent.pawn.Map, false));
					}
				},
				ticksAwayFromCast = 5
			};
			yield break;
		}

		// Token: 0x0600C0B2 RID: 49330 RVA: 0x0037CC68 File Offset: 0x0037AE68
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (target.HasThing)
			{
				base.Apply(target, dest);
				LocalTargetInfo destination = base.GetDestination(dest.IsValid ? dest : target);
				if (destination.IsValid)
				{
					Pawn pawn = this.parent.pawn;
					if (!this.parent.def.HasAreaOfEffect)
					{
						this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_Entry.Spawn(target.Thing, pawn.Map, 1f), target.Thing.Position, 60, null);
					}
					else
					{
						this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_EntryNoDelay.Spawn(target.Thing, pawn.Map, 1f), target.Thing.Position, 60, null);
					}
					if (this.Props.destination == AbilityEffectDestination.Selected)
					{
						this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_Exit.Spawn(destination.Cell, pawn.Map, 1f), destination.Cell, 60, null);
					}
					else
					{
						this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(destination.Cell, pawn.Map, 1f), destination.Cell, 60, null);
					}
					CompCanBeDormant compCanBeDormant = target.Thing.TryGetComp<CompCanBeDormant>();
					if (compCanBeDormant != null)
					{
						compCanBeDormant.WakeUp();
					}
					target.Thing.Position = destination.Cell;
					Pawn pawn2 = target.Thing as Pawn;
					if (pawn2 != null)
					{
						if ((pawn2.Faction == Faction.OfPlayer || pawn2.IsPlayerControlled) && pawn2.Position.Fogged(pawn2.Map))
						{
							FloodFillerFog.FloodUnfog(pawn2.Position, pawn2.Map);
						}
						pawn2.stances.stunner.StunFor(this.Props.stunTicks.RandomInRange, this.parent.pawn, false, false, false);
						pawn2.Notify_Teleported(true, true);
						CompAbilityEffect_Teleport.SendSkipUsedSignal(pawn2.Position, pawn2);
					}
					if (this.Props.destClamorType != null)
					{
						GenClamor.DoClamor(pawn, target.Cell, (float)this.Props.destClamorRadius, this.Props.destClamorType);
					}
				}
			}
		}

		// Token: 0x0600C0B3 RID: 49331 RVA: 0x0037CE96 File Offset: 0x0037B096
		public override bool CanHitTarget(LocalTargetInfo target)
		{
			return base.CanPlaceSelectedTargetAt(target) && base.CanHitTarget(target);
		}

		// Token: 0x0600C0B4 RID: 49332 RVA: 0x0037CEAC File Offset: 0x0037B0AC
		public override bool Valid(LocalTargetInfo target, bool showMessages = true)
		{
			AcceptanceReport report = this.CanSkipTarget(target);
			if (!report)
			{
				if (showMessages && !report.Reason.NullOrEmpty())
				{
					Pawn pawn = target.Thing as Pawn;
					if (pawn != null)
					{
						Messages.Message("CannotSkipTarget".Translate(pawn.Named("PAWN")) + ": " + report.Reason, pawn, MessageTypeDefOf.RejectInput, false);
					}
				}
				return false;
			}
			return base.Valid(target, showMessages);
		}

		// Token: 0x0600C0B5 RID: 49333 RVA: 0x0037CF38 File Offset: 0x0037B138
		private AcceptanceReport CanSkipTarget(LocalTargetInfo target)
		{
			Pawn pawn = target.Thing as Pawn;
			if (pawn != null)
			{
				if (pawn.BodySize > this.Props.maxBodySize)
				{
					return "CannotSkipTargetTooLarge".Translate();
				}
				if (pawn.kindDef.skipResistant)
				{
					return "CannotSkipTargetPsychicResistant".Translate();
				}
			}
			return true;
		}

		// Token: 0x0600C0B6 RID: 49334 RVA: 0x0037CF9C File Offset: 0x0037B19C
		public override string ExtraLabelMouseAttachment(LocalTargetInfo target)
		{
			return this.CanSkipTarget(target).Reason;
		}

		// Token: 0x0600C0B7 RID: 49335 RVA: 0x0037CFB8 File Offset: 0x0037B1B8
		public static void SendSkipUsedSignal(LocalTargetInfo target, Thing initiator)
		{
			Find.SignalManager.SendSignal(new Signal(CompAbilityEffect_Teleport.SkipUsedSignalTag, target.Named("POSITION"), initiator.Named("SUBJECT")));
		}

		// Token: 0x040083E9 RID: 33769
		public static string SkipUsedSignalTag = "CompAbilityEffect.SkipUsed";
	}
}
