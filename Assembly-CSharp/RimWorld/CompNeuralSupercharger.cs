using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001948 RID: 6472
	[StaticConstructorOnStartup]
	public class CompNeuralSupercharger : CompRechargeable
	{
		// Token: 0x17001654 RID: 5716
		// (get) Token: 0x06009333 RID: 37683 RVA: 0x002B4EC7 File Offset: 0x002B30C7
		private static Texture2D ColonistOnlyCommandTex
		{
			get
			{
				if (CompNeuralSupercharger.colonistOnlyCommandTex == null)
				{
					CompNeuralSupercharger.colonistOnlyCommandTex = ContentFinder<Texture2D>.Get("UI/Gizmos/NeuralSupercharger_AllowGuests", true);
				}
				return CompNeuralSupercharger.colonistOnlyCommandTex;
			}
		}

		// Token: 0x17001655 RID: 5717
		// (get) Token: 0x06009334 RID: 37684 RVA: 0x002B4EEB File Offset: 0x002B30EB
		private CompProperties_NeuralSupercharger Props
		{
			get
			{
				return (CompProperties_NeuralSupercharger)this.props;
			}
		}

		// Token: 0x06009335 RID: 37685 RVA: 0x002B4EF8 File Offset: 0x002B30F8
		public bool CanAutoUse(Pawn pawn)
		{
			if (!this.allowGuests && pawn.IsQuestLodger())
			{
				return false;
			}
			switch (this.autoUseMode)
			{
			case CompNeuralSupercharger.AutoUseMode.NoAutoUse:
				return false;
			case CompNeuralSupercharger.AutoUseMode.AutoUseWithDesire:
				return pawn.Ideo != null && pawn.Ideo.HasPrecept(PreceptDefOf.NeuralSupercharge_Preferred);
			case CompNeuralSupercharger.AutoUseMode.AutoUseForEveryone:
				return true;
			default:
				Log.Error(string.Format("Unknown auto use mode: {0}", this.autoUseMode));
				return false;
			}
		}

		// Token: 0x06009336 RID: 37686 RVA: 0x002B4F6C File Offset: 0x002B316C
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.autoUseMode = CompNeuralSupercharger.AutoUseMode.AutoUseWithDesire;
		}

		// Token: 0x06009337 RID: 37687 RVA: 0x002B4F7C File Offset: 0x002B317C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<CompNeuralSupercharger.AutoUseMode>(ref this.autoUseMode, "autoUseMode", CompNeuralSupercharger.AutoUseMode.AutoUseWithDesire, false);
			Scribe_Values.Look<bool>(ref this.allowGuests, "allowGuests", false, false);
		}

		// Token: 0x06009338 RID: 37688 RVA: 0x002B4FA8 File Offset: 0x002B31A8
		public override void CompTick()
		{
			base.CompTick();
			if (this.Props.effectCharged != null)
			{
				if (this.parent.Spawned)
				{
					if (base.Charged)
					{
						if (this.effecterCharged == null)
						{
							this.effecterCharged = this.Props.effectCharged.Spawn();
							this.effecterCharged.Trigger(this.parent, new TargetInfo(this.parent.InteractionCell, this.parent.Map, false), -1);
						}
						this.effecterCharged.EffectTick(this.parent, new TargetInfo(this.parent.InteractionCell, this.parent.Map, false));
						return;
					}
					if (this.effecterCharged != null)
					{
						this.DespawnEffecter();
						return;
					}
				}
				else
				{
					this.DespawnEffecter();
				}
			}
		}

		// Token: 0x06009339 RID: 37689 RVA: 0x002B5080 File Offset: 0x002B3280
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckIdeology("Neural supercharger"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x0600933A RID: 37690 RVA: 0x002B50A2 File Offset: 0x002B32A2
		private void DespawnEffecter()
		{
			Effecter effecter = this.effecterCharged;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.effecterCharged = null;
		}

		// Token: 0x0600933B RID: 37691 RVA: 0x002B50BC File Offset: 0x002B32BC
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			if (!ModLister.CheckIdeology("Neural supercharger"))
			{
				yield break;
			}
			if (!base.Charged)
			{
				yield return new FloatMenuOption(this.Props.jobString + " (" + "NeuralSuperchargeNotReady".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			if (selPawn.CurJob != null && selPawn.CurJob.def == JobDefOf.GetNeuralSupercharge && selPawn.CurJob.targetA.Thing == this.parent)
			{
				yield return new FloatMenuOption(this.Props.jobString + " (" + "NeuralSuperchargeAlreadyGetting".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(this.Props.jobString, delegate()
			{
				Job job = JobMaker.MakeJob(JobDefOf.GetNeuralSupercharge, this.parent);
				selPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), selPawn, this.parent, "ReservedBy", null);
			yield break;
		}

		// Token: 0x0600933C RID: 37692 RVA: 0x002B50D3 File Offset: 0x002B32D3
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield return new Command_SetNeuralSuperchargerAutoUse(this);
			yield return new Command_Toggle
			{
				defaultLabel = "CommandNeuralSuperchargerAllowGuests".Translate(),
				defaultDesc = "CommandNeuralSuperchargerAllowGuestsDescription".Translate(),
				icon = CompNeuralSupercharger.ColonistOnlyCommandTex,
				isActive = (() => this.allowGuests),
				toggleAction = delegate()
				{
					this.allowGuests = !this.allowGuests;
				},
				activateSound = SoundDefOf.Tick_Tiny
			};
			yield break;
			yield break;
		}

		// Token: 0x04005EA7 RID: 24231
		private static Texture2D colonistOnlyCommandTex;

		// Token: 0x04005EA8 RID: 24232
		public CompNeuralSupercharger.AutoUseMode autoUseMode = CompNeuralSupercharger.AutoUseMode.AutoUseWithDesire;

		// Token: 0x04005EA9 RID: 24233
		public bool allowGuests;

		// Token: 0x04005EAA RID: 24234
		private Effecter effecterCharged;

		// Token: 0x02001949 RID: 6473
		public enum AutoUseMode
		{
			// Token: 0x04005EAC RID: 24236
			NoAutoUse,
			// Token: 0x04005EAD RID: 24237
			AutoUseWithDesire,
			// Token: 0x04005EAE RID: 24238
			AutoUseForEveryone
		}
	}
}
