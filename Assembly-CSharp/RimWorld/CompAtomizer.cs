using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002BF9 RID: 11257
	public class CompAtomizer : CompThingContainer, INotifyHauledTo
	{
		// Token: 0x1700279C RID: 10140
		// (get) Token: 0x0600FC3B RID: 64571 RVA: 0x0049280A File Offset: 0x00490A0A
		public new CompProperties_Atomizer Props
		{
			get
			{
				return (CompProperties_Atomizer)this.props;
			}
		}

		// Token: 0x1700279D RID: 10141
		// (get) Token: 0x0600FC3C RID: 64572 RVA: 0x00492817 File Offset: 0x00490A17
		public bool AutoLoad
		{
			get
			{
				return this.autoLoad;
			}
		}

		// Token: 0x1700279E RID: 10142
		// (get) Token: 0x0600FC3D RID: 64573 RVA: 0x0049281F File Offset: 0x00490A1F
		public int SpaceLeft
		{
			get
			{
				return this.Props.stackLimit - base.TotalStackCount;
			}
		}

		// Token: 0x1700279F RID: 10143
		// (get) Token: 0x0600FC3E RID: 64574 RVA: 0x00492833 File Offset: 0x00490A33
		public int TicksPerAtomize
		{
			get
			{
				return this.Props.ticksPerAtomize;
			}
		}

		// Token: 0x170027A0 RID: 10144
		// (get) Token: 0x0600FC3F RID: 64575 RVA: 0x00492840 File Offset: 0x00490A40
		public int TicksLeftUntilAllAtomized
		{
			get
			{
				return base.TotalStackCount * this.TicksPerAtomize - this.ticksAtomized;
			}
		}

		// Token: 0x170027A1 RID: 10145
		// (get) Token: 0x0600FC40 RID: 64576 RVA: 0x00492856 File Offset: 0x00490A56
		public float FillPercent
		{
			get
			{
				return (float)base.TotalStackCount / (float)this.Props.stackLimit;
			}
		}

		// Token: 0x170027A2 RID: 10146
		// (get) Token: 0x0600FC41 RID: 64577 RVA: 0x0049286C File Offset: 0x00490A6C
		private Graphic ContentsGraphic
		{
			get
			{
				if (this.contentsGraphic == null)
				{
					ThingDef thingDef = this.Props.thingDef;
					this.contentsGraphic = thingDef.graphic.GetColoredVersion(thingDef.graphic.Shader, thingDef.graphic.color, thingDef.graphic.colorTwo);
				}
				return this.contentsGraphic;
			}
		}

		// Token: 0x0600FC42 RID: 64578 RVA: 0x004928C5 File Offset: 0x00490AC5
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckBiotech("Atomizer"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.TryGetComp<CompPowerTrader>();
		}

		// Token: 0x0600FC43 RID: 64579 RVA: 0x004928F8 File Offset: 0x00490AF8
		public override bool Accepts(Thing thing)
		{
			return base.Accepts(thing) && thing.def == this.Props.thingDef;
		}

		// Token: 0x0600FC44 RID: 64580 RVA: 0x00492918 File Offset: 0x00490B18
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.parent.Faction == Faction.OfPlayer)
			{
				Command_Toggle command_Toggle = new Command_Toggle();
				command_Toggle.defaultLabel = "CommandAutoLoad".Translate(Find.ActiveLanguageWorker.Pluralize(this.Props.thingDef.label, -1));
				command_Toggle.defaultDesc = "CommandAutoLoadDesc".Translate(Find.ActiveLanguageWorker.Pluralize(this.Props.thingDef.label, -1));
				command_Toggle.icon = (this.autoLoad ? TexCommand.ForbidOff : TexCommand.ForbidOn);
				command_Toggle.hotKey = KeyBindingDefOf.Command_ItemForbid;
				command_Toggle.isActive = (() => this.autoLoad);
				Command_Toggle command_Toggle2 = command_Toggle;
				command_Toggle2.toggleAction = (Action)Delegate.Combine(command_Toggle2.toggleAction, new Action(delegate()
				{
					this.autoLoad = !this.autoLoad;
				}));
				yield return command_Toggle;
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "CommandEjectContents".Translate();
				command_Action.defaultDesc = "CommandEjectContentsDesc".Translate(Find.ActiveLanguageWorker.Pluralize(this.Props.thingDef.label, -1));
				command_Action.icon = this.EjectContentsIcon.Texture;
				command_Action.Disabled = base.Empty;
				Command_Action command_Action2 = command_Action;
				command_Action2.action = (Action)Delegate.Combine(command_Action2.action, new Action(this.EjectContents));
				yield return command_Action;
			}
			if (DebugSettings.ShowDevGizmos)
			{
				Command_Action command_Action3 = new Command_Action();
				command_Action3.defaultLabel = "DEV: Atomize";
				command_Action3.Disabled = base.Empty;
				Command_Action command_Action4 = command_Action3;
				command_Action4.action = (Action)Delegate.Combine(command_Action4.action, new Action(this.DoAtomize));
				yield return command_Action3;
				yield return new Command_Action
				{
					defaultLabel = "DEV: Set next atomization",
					action = delegate()
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						int[] array = new int[]
						{
							60,
							120,
							180,
							240,
							300,
							600,
							900,
							1200,
							1500,
							1800,
							3600
						};
						for (int i = 0; i < array.Length; i++)
						{
							int ticks = array[i];
							list.Add(new FloatMenuOption(ticks.ToStringSecondsFromTicks("F0"), delegate()
							{
								this.ticksAtomized = this.TicksPerAtomize - ticks;
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
						Find.WindowStack.Add(new FloatMenu(list));
					},
					Disabled = base.Empty
				};
			}
			yield break;
		}

		// Token: 0x0600FC45 RID: 64581 RVA: 0x00492928 File Offset: 0x00490B28
		private void DoAtomize()
		{
			Thing containedThing = base.ContainedThing;
			containedThing.stackCount--;
			if (this.Props.resolveEffecter != null)
			{
				this.parent.MapHeld.effecterMaintainer.AddEffecterToMaintain(this.Props.resolveEffecter.Spawn(this.parent, this.parent.MapHeld, 1f), this.parent, this.Props.resolveEffecterTicks);
			}
			if (containedThing.stackCount <= 0)
			{
				this.innerContainer.Remove(containedThing);
				containedThing.Destroy(DestroyMode.Vanish);
			}
			this.ticksAtomized = 0;
		}

		// Token: 0x0600FC46 RID: 64582 RVA: 0x004929C7 File Offset: 0x00490BC7
		private void EjectContents()
		{
			this.innerContainer.TryDropAll(this.parent.InteractionCell, this.parent.Map, ThingPlaceMode.Near, null, null, true);
			this.ticksAtomized = 0;
		}

		// Token: 0x0600FC47 RID: 64583 RVA: 0x004929F6 File Offset: 0x00490BF6
		public void Notify_HauledTo(Pawn hauler, Thing thing, int count)
		{
			this.Props.materialsAddedSound.PlayOneShot(this.parent);
		}

		// Token: 0x0600FC48 RID: 64584 RVA: 0x00492A14 File Offset: 0x00490C14
		public override void PostDraw()
		{
			base.PostDraw();
			if (!this.autoLoad)
			{
				this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.ForbiddenAtomizer);
			}
			if (!base.Empty)
			{
				Vector3 loc = this.parent.DrawPos + this.Props.contentsDrawOffset;
				loc.y -= 0.03658537f;
				Graphic_StackCount graphic_StackCount = this.ContentsGraphic as Graphic_StackCount;
				if (graphic_StackCount != null)
				{
					graphic_StackCount.SubGraphicForStackCount(Mathf.Min(base.TotalStackCount, this.Props.thingDef.stackLimit), this.Props.thingDef).Draw(loc, Rot4.North, this.parent, 0f);
					return;
				}
				this.ContentsGraphic.Draw(loc, Rot4.North, this.parent, 0f);
			}
		}

		// Token: 0x0600FC49 RID: 64585 RVA: 0x00492AF4 File Offset: 0x00490CF4
		public override void CompTick()
		{
			base.CompTick();
			if (!base.Empty && this.powerComp.PowerOn)
			{
				this.ticksAtomized++;
				if (this.ticksAtomized >= this.TicksPerAtomize)
				{
					this.DoAtomize();
				}
				if (this.workingEffecter == null)
				{
					this.workingEffecter = this.Props.workingEffecter.Spawn(this.parent, this.parent.Map, 1f);
				}
				this.workingEffecter.EffectTick(this.parent, this.parent);
				return;
			}
			Effecter effecter = this.workingEffecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.workingEffecter = null;
		}

		// Token: 0x0600FC4A RID: 64586 RVA: 0x00492BAF File Offset: 0x00490DAF
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.autoLoad, "autoLoad", true, false);
			Scribe_Values.Look<int>(ref this.ticksAtomized, "ticksAtomized", 0, false);
		}

		// Token: 0x0600FC4B RID: 64587 RVA: 0x00492BDC File Offset: 0x00490DDC
		public override string CompInspectStringExtra()
		{
			TaggedString t = "";
			if (!base.Empty)
			{
				t += "ContainedThings".Translate(this.Props.thingDef) + ": " + base.TotalStackCount.ToString() + " / " + this.Props.stackLimit.ToString();
				t += "\n" + "FinishesIn".Translate() + ": " + "PeriodDays".Translate(((float)this.TicksLeftUntilAllAtomized / 60000f).ToString("F1"));
				t += "\n" + "DurationUntilNextAtomization".Translate() + ": " + (this.TicksPerAtomize - this.ticksAtomized).ToStringTicksToPeriod(true, false, true, true, false);
				if (!this.powerComp.PowerOn)
				{
					t += " (" + "Paused".Translate() + ")";
				}
			}
			return t.Resolve();
		}

		// Token: 0x0400A542 RID: 42306
		private CachedTexture EjectContentsIcon = new CachedTexture("UI/Icons/EjectContentFromAtomizer");

		// Token: 0x0400A543 RID: 42307
		private bool autoLoad = true;

		// Token: 0x0400A544 RID: 42308
		private int ticksAtomized;

		// Token: 0x0400A545 RID: 42309
		private CompPowerTrader powerComp;

		// Token: 0x0400A546 RID: 42310
		private Effecter workingEffecter;

		// Token: 0x0400A547 RID: 42311
		private Graphic contentsGraphic;
	}
}
