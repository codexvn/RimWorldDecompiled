using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002ECB RID: 11979
	public class CompVoidStructure : CompInteractable
	{
		// Token: 0x17002B99 RID: 11161
		// (get) Token: 0x06010E91 RID: 69265 RVA: 0x004DE2D3 File Offset: 0x004DC4D3
		protected virtual bool Activatable
		{
			get
			{
				return !this.activated;
			}
		}

		// Token: 0x17002B9A RID: 11162
		// (get) Token: 0x06010E92 RID: 69266 RVA: 0x004DE2DE File Offset: 0x004DC4DE
		public override bool Active
		{
			get
			{
				return this.activated;
			}
		}

		// Token: 0x17002B9B RID: 11163
		// (get) Token: 0x06010E93 RID: 69267 RVA: 0x004DE2E6 File Offset: 0x004DC4E6
		protected virtual SoundDef AmbientSound
		{
			get
			{
				if (!this.Active)
				{
					return SoundDefOf.VoidStructure_AmbientPreActivate;
				}
				return SoundDefOf.VoidStructure_AmbientPostActivate;
			}
		}

		// Token: 0x06010E94 RID: 69268 RVA: 0x004DE2FC File Offset: 0x004DC4FC
		public override void CompTick()
		{
			base.CompTick();
			if (this.Active && this.ambienceEffecter == null)
			{
				this.ambienceEffecter = EffecterDefOf.VoidStructureActivatedAmbience.Spawn(this.parent, this.parent.Map, 1f);
			}
			if (this.AmbientSound != null)
			{
				if (this.ambienceSustainer == null || this.ambienceSustainer.Ended)
				{
					this.ambienceSustainer = this.AmbientSound.TrySpawnSustainer(this.parent);
				}
				if (this.ambienceSustainer.def != this.AmbientSound)
				{
					this.ambienceSustainer.End();
				}
			}
			Effecter effecter = this.ambienceEffecter;
			if (effecter == null)
			{
				return;
			}
			effecter.EffectTick(this.parent, this.parent);
		}

		// Token: 0x06010E95 RID: 69269 RVA: 0x004DE3C4 File Offset: 0x004DC5C4
		protected override void OnInteracted(Pawn caster)
		{
			this.parent.overrideGraphicIndex = new int?(1);
			this.parent.DirtyMapMesh(this.parent.Map);
			EffecterDefOf.VoidStructureActivated.Spawn(this.parent, this.parent.Map, 1f);
			CompGlower comp = this.parent.GetComp<CompGlower>();
			if (comp != null)
			{
				comp.GlowRadius = comp.Props.glowRadius + 1f;
				comp.GlowColor = new ColorInt(240, 160, 184);
			}
			this.activated = true;
		}

		// Token: 0x06010E96 RID: 69270 RVA: 0x004DE460 File Offset: 0x004DC660
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			if (!this.Activatable)
			{
				return false;
			}
			return base.CanInteract(activateBy, checkOptionalItems);
		}

		// Token: 0x06010E97 RID: 69271 RVA: 0x004DE479 File Offset: 0x004DC679
		public void Reset()
		{
			this.activated = false;
		}

		// Token: 0x06010E98 RID: 69272 RVA: 0x004DE484 File Offset: 0x004DC684
		public override string CompInspectStringExtra()
		{
			if (this.Active)
			{
				return "Activated".Translate().CapitalizeFirst();
			}
			if (!this.Activatable)
			{
				return "";
			}
			return base.CompInspectStringExtra();
		}

		// Token: 0x06010E99 RID: 69273 RVA: 0x004DE4C5 File Offset: 0x004DC6C5
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			if (!this.Activatable)
			{
				yield break;
			}
			foreach (FloatMenuOption floatMenuOption in base.CompFloatMenuOptions(selPawn))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06010E9A RID: 69274 RVA: 0x004DE4DC File Offset: 0x004DC6DC
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!this.Activatable)
			{
				yield break;
			}
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos)
			{
				Command_Action command_Action = new Command_Action
				{
					defaultLabel = "DEV: Activate",
					action = delegate()
					{
						base.Interact(this.parent.Map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>(), true);
					}
				};
				yield return command_Action;
			}
			yield break;
			yield break;
		}

		// Token: 0x06010E9B RID: 69275 RVA: 0x004DE4EC File Offset: 0x004DC6EC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.activated, "activated", false, false);
		}

		// Token: 0x0400AFE7 RID: 45031
		private const int ActiveGraphicIndex = 1;

		// Token: 0x0400AFE8 RID: 45032
		protected bool activated;

		// Token: 0x0400AFE9 RID: 45033
		private Sustainer ambienceSustainer;

		// Token: 0x0400AFEA RID: 45034
		private Effecter ambienceEffecter;
	}
}
