using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class CompAtomizer : CompThingContainer, INotifyHauledTo
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass22_0
	{
		public int ticks;

		public CompAtomizer _003C_003E4__this;

		internal void _003CCompGetGizmosExtra_003Eb__3()
		{
			_003C_003E4__this.ticksAtomized = _003C_003E4__this.TicksPerAtomize - ticks;
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__22 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompAtomizer _003C_003E4__this;

		Gizmo IEnumerator<Gizmo>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CCompGetGizmosExtra_003Ed__22(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CompAtomizer CS_0024_003C_003E8__locals20 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals20.parent.Faction == Faction.OfPlayer)
				{
					Command_Toggle command_Toggle = new Command_Toggle
					{
						defaultLabel = "CommandAutoLoad".Translate(Find.ActiveLanguageWorker.Pluralize(CS_0024_003C_003E8__locals20.Props.thingDef.label)),
						defaultDesc = "CommandAutoLoadDesc".Translate(Find.ActiveLanguageWorker.Pluralize(CS_0024_003C_003E8__locals20.Props.thingDef.label)),
						icon = (CS_0024_003C_003E8__locals20.autoLoad ? TexCommand.ForbidOff : TexCommand.ForbidOn),
						hotKey = KeyBindingDefOf.Command_ItemForbid,
						isActive = () => CS_0024_003C_003E8__locals20.autoLoad
					};
					command_Toggle.toggleAction = (Action)Delegate.Combine(command_Toggle.toggleAction, (Action)delegate
					{
						CS_0024_003C_003E8__locals20.autoLoad = !CS_0024_003C_003E8__locals20.autoLoad;
					});
					_003C_003E2__current = command_Toggle;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_01cc;
			case 1:
			{
				_003C_003E1__state = -1;
				Command_Action command_Action = new Command_Action
				{
					defaultLabel = "CommandEjectContents".Translate(),
					defaultDesc = "CommandEjectContentsDesc".Translate(Find.ActiveLanguageWorker.Pluralize(CS_0024_003C_003E8__locals20.Props.thingDef.label)),
					icon = CS_0024_003C_003E8__locals20.EjectContentsIcon.Texture,
					Disabled = CS_0024_003C_003E8__locals20.Empty
				};
				command_Action.action = (Action)Delegate.Combine(command_Action.action, new Action(CS_0024_003C_003E8__locals20.EjectContents));
				_003C_003E2__current = command_Action;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
				_003C_003E1__state = -1;
				goto IL_01cc;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Set next atomization",
					action = delegate
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						int[] array = new int[11]
						{
							60, 120, 180, 240, 300, 600, 900, 1200, 1500, 1800,
							3600
						};
						for (int i = 0; i < array.Length; i++)
						{
							_003C_003Ec__DisplayClass22_0 CS_0024_003C_003E8__locals6 = new _003C_003Ec__DisplayClass22_0
							{
								_003C_003E4__this = CS_0024_003C_003E8__locals20,
								ticks = array[i]
							};
							list.Add(new FloatMenuOption(CS_0024_003C_003E8__locals6.ticks.ToStringSecondsFromTicks("F0"), delegate
							{
								CS_0024_003C_003E8__locals6._003C_003E4__this.ticksAtomized = CS_0024_003C_003E8__locals6._003C_003E4__this.TicksPerAtomize - CS_0024_003C_003E8__locals6.ticks;
							}));
						}
						Find.WindowStack.Add(new FloatMenu(list));
					},
					Disabled = CS_0024_003C_003E8__locals20.Empty
				};
				_003C_003E1__state = 4;
				return true;
			case 4:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_01cc:
				if (DebugSettings.ShowDevGizmos)
				{
					Command_Action command_Action2 = new Command_Action
					{
						defaultLabel = "DEV: Atomize",
						Disabled = CS_0024_003C_003E8__locals20.Empty
					};
					command_Action2.action = (Action)Delegate.Combine(command_Action2.action, new Action(CS_0024_003C_003E8__locals20.DoAtomize));
					_003C_003E2__current = command_Action2;
					_003C_003E1__state = 3;
					return true;
				}
				break;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__22 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__22(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	private CachedTexture EjectContentsIcon = new CachedTexture("UI/Icons/EjectContentFromAtomizer");

	private bool autoLoad = true;

	private int ticksAtomized;

	private CompPowerTrader powerComp;

	private Effecter workingEffecter;

	private Graphic contentsGraphic;

	public new CompProperties_Atomizer Props => (CompProperties_Atomizer)props;

	public bool AutoLoad => autoLoad;

	public int SpaceLeft => Props.stackLimit - base.TotalStackCount;

	public int TicksPerAtomize => Props.ticksPerAtomize;

	public int TicksLeftUntilAllAtomized => base.TotalStackCount * TicksPerAtomize - ticksAtomized;

	public float FillPercent => (float)base.TotalStackCount / (float)Props.stackLimit;

	private Graphic ContentsGraphic
	{
		get
		{
			if (contentsGraphic == null)
			{
				ThingDef thingDef = Props.thingDef;
				contentsGraphic = thingDef.graphic.GetColoredVersion(thingDef.graphic.Shader, thingDef.graphic.color, thingDef.graphic.colorTwo);
			}
			return contentsGraphic;
		}
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		if (!ModLister.CheckBiotech("Atomizer"))
		{
			parent.Destroy();
			return;
		}
		base.PostSpawnSetup(respawningAfterLoad);
		powerComp = parent.TryGetComp<CompPowerTrader>();
	}

	public override bool Accepts(Thing thing)
	{
		if (base.Accepts(thing))
		{
			return thing.def == Props.thingDef;
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__22))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__22(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void DoAtomize()
	{
		Thing containedThing = base.ContainedThing;
		containedThing.stackCount--;
		if (Props.resolveEffecter != null)
		{
			parent.MapHeld.effecterMaintainer.AddEffecterToMaintain(Props.resolveEffecter.Spawn(parent, parent.MapHeld), parent, Props.resolveEffecterTicks);
		}
		if (containedThing.stackCount <= 0)
		{
			innerContainer.Remove(containedThing);
			containedThing.Destroy();
		}
		ticksAtomized = 0;
	}

	private void EjectContents()
	{
		innerContainer.TryDropAll(parent.InteractionCell, parent.Map, ThingPlaceMode.Near);
		ticksAtomized = 0;
	}

	public void Notify_HauledTo(Pawn hauler, Thing thing, int count)
	{
		Props.materialsAddedSound.PlayOneShot(parent);
	}

	public override void PostDraw()
	{
		base.PostDraw();
		if (!autoLoad)
		{
			parent.Map.overlayDrawer.DrawOverlay(parent, OverlayTypes.ForbiddenAtomizer);
		}
		if (!base.Empty)
		{
			Vector3 loc = parent.DrawPos + Props.contentsDrawOffset;
			loc.y -= 0.03658537f;
			if (ContentsGraphic is Graphic_StackCount graphic_StackCount)
			{
				graphic_StackCount.SubGraphicForStackCount(Mathf.Min(base.TotalStackCount, Props.thingDef.stackLimit), Props.thingDef).Draw(loc, Rot4.North, parent);
			}
			else
			{
				ContentsGraphic.Draw(loc, Rot4.North, parent);
			}
		}
	}

	public override void CompTick()
	{
		base.CompTick();
		if (!base.Empty && powerComp.PowerOn)
		{
			ticksAtomized++;
			if (ticksAtomized >= TicksPerAtomize)
			{
				DoAtomize();
			}
			if (workingEffecter == null)
			{
				workingEffecter = Props.workingEffecter.Spawn(parent, parent.Map);
			}
			workingEffecter.EffectTick(parent, parent);
		}
		else
		{
			workingEffecter?.Cleanup();
			workingEffecter = null;
		}
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref autoLoad, "autoLoad", defaultValue: true);
		Scribe_Values.Look(ref ticksAtomized, "ticksAtomized", 0);
	}

	public override string CompInspectStringExtra()
	{
		TaggedString taggedString = "";
		if (!base.Empty)
		{
			taggedString += string.Concat("ContainedThings".Translate(Props.thingDef) + ": ", base.TotalStackCount.ToString(), " / ", Props.stackLimit.ToString());
			taggedString += "\n" + "FinishesIn".Translate() + ": " + "PeriodDays".Translate(((float)TicksLeftUntilAllAtomized / 60000f).ToString("F1"));
			taggedString += "\n" + "DurationUntilNextAtomization".Translate() + ": " + (TicksPerAtomize - ticksAtomized).ToStringTicksToPeriod();
			if (!powerComp.PowerOn)
			{
				taggedString += " (" + "Paused".Translate() + ")";
			}
		}
		return taggedString.Resolve();
	}
}
