using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse;

public class CompGlower : ThingComp
{
	private enum GlowerColorChangeType
	{
		None,
		ColorPickerEnabled,
		DarklightToggle,
		AllGlowers
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass34_0
	{
		public CompGlower _003C_003E4__this;

		public List<CompGlower> extraGlowers;

		internal void _003CCompGetGizmosExtra_003Eb__0()
		{
			Widgets.ColorComponents visibleTextfields = (DebugSettings.editableGlowerColors ? visibleDebugColorTextfields : visibleColorTextfields);
			Widgets.ColorComponents editableTextfields = (DebugSettings.editableGlowerColors ? editableDebugColorTextfields : editableColorTextfields);
			Dialog_GlowerColorPicker window = new Dialog_GlowerColorPicker(_003C_003E4__this, extraGlowers, visibleTextfields, editableTextfields);
			Find.WindowStack.Add(window);
		}

		internal void _003CCompGetGizmosExtra_003Eb__1()
		{
			ColorClipboard = _003C_003E4__this.GlowColor;
			Messages.Message("ColorCopiedSuccessfully".Translate(), MessageTypeDefOf.PositiveEvent, historical: false);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass34_1
	{
		public float hue;

		public float sat;

		public _003C_003Ec__DisplayClass34_0 CS_0024_003C_003E8__locals1;

		internal void _003CCompGetGizmosExtra_003Eb__2()
		{
			try
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera();
				ColorInt glowColor = CS_0024_003C_003E8__locals1._003C_003E4__this.GlowColor;
				glowColor.SetHueSaturation(hue, sat);
				CS_0024_003C_003E8__locals1._003C_003E4__this.GlowColor = glowColor;
				foreach (CompGlower item in CS_0024_003C_003E8__locals1.extraGlowers)
				{
					glowColor = item.GlowColor;
					glowColor.SetHueSaturation(hue, sat);
					item.GlowColor = glowColor;
				}
				Messages.Message("ColorPastedSuccessfully".Translate(), MessageTypeDefOf.PositiveEvent, historical: false);
			}
			catch (Exception)
			{
				Messages.Message("ClipboardInvalidColor".Translate(), MessageTypeDefOf.RejectInput, historical: false);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass34_2
	{
		public bool darklight;

		public _003C_003Ec__DisplayClass34_0 CS_0024_003C_003E8__locals2;

		internal void _003CCompGetGizmosExtra_003Eb__3()
		{
			SoundDefOf.Tick_High.PlayOneShotOnCamera();
			if (darklight)
			{
				CS_0024_003C_003E8__locals2._003C_003E4__this.SetGlowColorInternal(null);
			}
			else
			{
				CS_0024_003C_003E8__locals2._003C_003E4__this.GlowColor = new ColorInt(DarklightUtility.DefaultDarklight);
			}
			foreach (CompGlower item in CS_0024_003C_003E8__locals2._003C_003E4__this.ExtraSelectedGlowers(GlowerColorChangeType.DarklightToggle))
			{
				if (darklight)
				{
					item.SetGlowColorInternal(null);
				}
				else
				{
					item.GlowColor = new ColorInt(DarklightUtility.DefaultDarklight);
				}
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__34 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompGlower _003C_003E4__this;

		private _003C_003Ec__DisplayClass34_0 _003C_003E8__1;

		private bool _003CdoCopyPasteGizmos_003E5__2;

		private IEnumerator<Gizmo> _003C_003E7__wrap2;

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
		public _003CCompGetGizmosExtra_003Ed__34(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E8__1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				CompGlower compGlower = _003C_003E4__this;
				GlowerColorChangeType type;
				Color32 value;
				Color32? color;
				Command_ColorIcon command_ColorIcon2;
				_003C_003Ec__DisplayClass34_2 CS_0024_003C_003E8__locals22;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass34_0();
					_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
					_003C_003E7__wrap2 = ((ThingComp)compGlower).CompGetGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00a0;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00a0;
				case 2:
					_003C_003E1__state = -1;
					goto IL_0286;
				case 3:
					_003C_003E1__state = -1;
					goto IL_0286;
				case 4:
				{
					_003C_003E1__state = -1;
					_003C_003Ec__DisplayClass34_1 CS_0024_003C_003E8__locals13 = new _003C_003Ec__DisplayClass34_1
					{
						CS_0024_003C_003E8__locals1 = _003C_003E8__1
					};
					bool flag = true;
					CS_0024_003C_003E8__locals13.hue = 0f;
					CS_0024_003C_003E8__locals13.sat = 0f;
					if (ColorClipboard.HasValue)
					{
						Color.RGBToHSV(ColorClipboard.Value.ProjectToColor32(), out CS_0024_003C_003E8__locals13.hue, out CS_0024_003C_003E8__locals13.sat, out var _);
						flag = false;
					}
					Command_ColorIcon command_ColorIcon = new Command_ColorIcon
					{
						icon = ContentFinder<Texture2D>.Get("UI/Commands/PasteColor"),
						defaultLabel = "CommandPasteColorLabel".Translate(),
						defaultDesc = "CommandPasteColorDesc".Translate()
					};
					if (!flag)
					{
						command_ColorIcon.color = Color.HSVToRGB(CS_0024_003C_003E8__locals13.hue, CS_0024_003C_003E8__locals13.sat, 1f);
					}
					command_ColorIcon.Disabled = flag;
					command_ColorIcon.disabledReason = "ClipboardInvalidColor".Translate();
					command_ColorIcon.action = delegate
					{
						try
						{
							SoundDefOf.Tick_High.PlayOneShotOnCamera();
							ColorInt glowColor = CS_0024_003C_003E8__locals13.CS_0024_003C_003E8__locals1._003C_003E4__this.GlowColor;
							glowColor.SetHueSaturation(CS_0024_003C_003E8__locals13.hue, CS_0024_003C_003E8__locals13.sat);
							CS_0024_003C_003E8__locals13.CS_0024_003C_003E8__locals1._003C_003E4__this.GlowColor = glowColor;
							foreach (CompGlower item in CS_0024_003C_003E8__locals13.CS_0024_003C_003E8__locals1.extraGlowers)
							{
								glowColor = item.GlowColor;
								glowColor.SetHueSaturation(CS_0024_003C_003E8__locals13.hue, CS_0024_003C_003E8__locals13.sat);
								item.GlowColor = glowColor;
							}
							Messages.Message("ColorPastedSuccessfully".Translate(), MessageTypeDefOf.PositiveEvent, historical: false);
						}
						catch (Exception)
						{
							Messages.Message("ClipboardInvalidColor".Translate(), MessageTypeDefOf.RejectInput, historical: false);
						}
					};
					command_ColorIcon.hotKey = KeyBindingDefOf.Misc5;
					_003C_003E2__current = command_ColorIcon;
					_003C_003E1__state = 5;
					return true;
				}
				case 5:
					_003C_003E1__state = -1;
					goto IL_047b;
				case 6:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_00a0:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Gizmo current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					if (compGlower.parent.Faction != Faction.OfPlayer)
					{
						return false;
					}
					_003CdoCopyPasteGizmos_003E5__2 = false;
					type = GlowerColorChangeType.None;
					if (compGlower.Props.colorPickerEnabled)
					{
						type = GlowerColorChangeType.ColorPickerEnabled;
					}
					else if (DebugSettings.editableGlowerColors)
					{
						type = GlowerColorChangeType.AllGlowers;
					}
					_003C_003E8__1.extraGlowers = compGlower.ExtraSelectedGlowers(type);
					value = compGlower.GlowColor.ProjectToColor32();
					value.a = byte.MaxValue;
					color = value;
					foreach (CompGlower item2 in _003C_003E8__1.extraGlowers)
					{
						if (item2.GlowColor != compGlower.GlowColor)
						{
							color = null;
						}
					}
					command_ColorIcon2 = new Command_ColorIcon
					{
						defaultLabel = "GlowerChangeColor".Translate(),
						defaultDesc = "GlowerChangeColorDescription".Translate(),
						icon = ContentFinder<Texture2D>.Get("UI/Commands/ChangeColor"),
						color = color,
						action = delegate
						{
							Widgets.ColorComponents visibleTextfields = (DebugSettings.editableGlowerColors ? visibleDebugColorTextfields : visibleColorTextfields);
							Widgets.ColorComponents editableTextfields = (DebugSettings.editableGlowerColors ? editableDebugColorTextfields : editableColorTextfields);
							Dialog_GlowerColorPicker window = new Dialog_GlowerColorPicker(_003C_003E8__1._003C_003E4__this, _003C_003E8__1.extraGlowers, visibleTextfields, editableTextfields);
							Find.WindowStack.Add(window);
						}
					};
					if (compGlower.Props.colorPickerEnabled)
					{
						command_ColorIcon2.Disabled = !(_003CdoCopyPasteGizmos_003E5__2 = DebugSettings.editableGlowerColors || ResearchProjectDefOf.ColoredLights.IsFinished);
						command_ColorIcon2.disabledReason = "GlowerChangeColorNeedsResearch".Translate(ResearchProjectDefOf.ColoredLights.label);
						_003C_003E2__current = command_ColorIcon2;
						_003C_003E1__state = 2;
						return true;
					}
					if (DebugSettings.editableGlowerColors)
					{
						_003CdoCopyPasteGizmos_003E5__2 = true;
						_003C_003E2__current = command_ColorIcon2;
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0286;
					IL_0286:
					if (_003CdoCopyPasteGizmos_003E5__2)
					{
						Command_ColorIcon command_ColorIcon3 = new Command_ColorIcon
						{
							icon = ContentFinder<Texture2D>.Get("UI/Commands/CopyColor"),
							defaultLabel = "CommandCopyColorLabel".Translate(),
							defaultDesc = "CommandCopyColorDesc".Translate()
						};
						Color32 value2 = compGlower.GlowColor.ProjectToColor32();
						value2.a = byte.MaxValue;
						command_ColorIcon3.color = value2;
						command_ColorIcon3.action = delegate
						{
							ColorClipboard = _003C_003E8__1._003C_003E4__this.GlowColor;
							Messages.Message("ColorCopiedSuccessfully".Translate(), MessageTypeDefOf.PositiveEvent, historical: false);
						};
						command_ColorIcon3.hotKey = KeyBindingDefOf.Misc4;
						_003C_003E2__current = command_ColorIcon3;
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_047b;
					IL_047b:
					if (!ModsConfig.IdeologyActive || !compGlower.Props.darklightToggle)
					{
						break;
					}
					CS_0024_003C_003E8__locals22 = new _003C_003Ec__DisplayClass34_2
					{
						CS_0024_003C_003E8__locals2 = _003C_003E8__1,
						darklight = DarklightUtility.IsDarklight(compGlower.GlowColor.ToColor)
					};
					_003C_003E2__current = new Command_Action
					{
						icon = ContentFinder<Texture2D>.Get(CS_0024_003C_003E8__locals22.darklight ? "UI/Commands/SetNormalLight" : "UI/Commands/SetDarklight"),
						defaultLabel = (CS_0024_003C_003E8__locals22.darklight ? "ToggleDarklightOff" : "ToggleDarklightOn").Translate(),
						defaultDesc = (CS_0024_003C_003E8__locals22.darklight ? "ToggleDarklightOffDesc" : "ToggleDarklightOnDesc").Translate(),
						action = delegate
						{
							SoundDefOf.Tick_High.PlayOneShotOnCamera();
							if (CS_0024_003C_003E8__locals22.darklight)
							{
								CS_0024_003C_003E8__locals22.CS_0024_003C_003E8__locals2._003C_003E4__this.SetGlowColorInternal(null);
							}
							else
							{
								CS_0024_003C_003E8__locals22.CS_0024_003C_003E8__locals2._003C_003E4__this.GlowColor = new ColorInt(DarklightUtility.DefaultDarklight);
							}
							foreach (CompGlower item3 in CS_0024_003C_003E8__locals22.CS_0024_003C_003E8__locals2._003C_003E4__this.ExtraSelectedGlowers(GlowerColorChangeType.DarklightToggle))
							{
								if (CS_0024_003C_003E8__locals22.darklight)
								{
									item3.SetGlowColorInternal(null);
								}
								else
								{
									item3.GlowColor = new ColorInt(DarklightUtility.DefaultDarklight);
								}
							}
						}
					};
					_003C_003E1__state = 6;
					return true;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__34 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__34(0)
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

	public static Widgets.ColorComponents visibleColorTextfields = Widgets.ColorComponents.Hue | Widgets.ColorComponents.Sat;

	public static Widgets.ColorComponents editableColorTextfields = Widgets.ColorComponents.Hue | Widgets.ColorComponents.Sat;

	public static Widgets.ColorComponents visibleDebugColorTextfields = Widgets.ColorComponents.All;

	public static Widgets.ColorComponents editableDebugColorTextfields = Widgets.ColorComponents.Red | Widgets.ColorComponents.Green | Widgets.ColorComponents.Blue | Widgets.ColorComponents.Hue | Widgets.ColorComponents.Sat;

	public static ColorInt? ColorClipboard;

	private bool glowOnInt;

	private ColorInt? glowColorOverride;

	private float? glowRadiusOverride;

	private static List<CompGlower> tmpExtraGlowers = new List<CompGlower>(64);

	public CompProperties_Glower Props => (CompProperties_Glower)props;

	public virtual ColorInt GlowColor
	{
		get
		{
			return glowColorOverride ?? Props.glowColor;
		}
		set
		{
			SetGlowColorInternal(value);
		}
	}

	public virtual float GlowRadius
	{
		get
		{
			return glowRadiusOverride ?? Props.glowRadius;
		}
		set
		{
			glowRadiusOverride = value;
		}
	}

	protected virtual bool ShouldBeLitNow
	{
		get
		{
			if (!parent.Spawned)
			{
				return false;
			}
			if (!FlickUtility.WantsToBeOn(parent))
			{
				return false;
			}
			if (parent is IThingGlower thingGlower && !thingGlower.ShouldBeLitNow())
			{
				return false;
			}
			ThingWithComps thingWithComps = parent;
			if (thingWithComps != null)
			{
				foreach (ThingComp allComp in thingWithComps.AllComps)
				{
					if (allComp is IThingGlower thingGlower2 && !thingGlower2.ShouldBeLitNow())
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	public bool Glows => glowOnInt;

	public bool HasGlowColorOverride => glowColorOverride.HasValue;

	public void UpdateLit(Map map)
	{
		bool shouldBeLitNow = ShouldBeLitNow;
		if (glowOnInt != shouldBeLitNow)
		{
			glowOnInt = shouldBeLitNow;
			if (!glowOnInt)
			{
				map.mapDrawer.MapMeshDirty(parent.Position, MapMeshFlagDefOf.Things);
				map.glowGrid.DeRegisterGlower(this);
			}
			else
			{
				map.mapDrawer.MapMeshDirty(parent.Position, MapMeshFlagDefOf.Things);
				map.glowGrid.RegisterGlower(this);
			}
		}
	}

	public void ForceRegister(Map map)
	{
		map.glowGrid.DeRegisterGlower(this);
		map.mapDrawer.MapMeshDirty(parent.Position, MapMeshFlagDefOf.Things);
		map.glowGrid.RegisterGlower(this);
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		if (!parent.BeingTransportedOnGravship)
		{
			RefreshGlower();
		}
	}

	public override void PostSwapMap()
	{
		RefreshGlower();
	}

	public override void PostMapInit()
	{
		RefreshGlower();
	}

	private void RefreshGlower()
	{
		UpdateLit(parent.Map);
		if (ShouldBeLitNow)
		{
			parent.Map.glowGrid.RegisterGlower(this);
		}
	}

	public override void ReceiveCompSignal(string signal)
	{
		switch (signal)
		{
		case "PowerTurnedOn":
		case "PowerTurnedOff":
		case "FlickedOn":
		case "FlickedOff":
		case "Refueled":
		case "RanOutOfFuel":
		case "ScheduledOn":
		case "ScheduledOff":
		case "MechClusterDefeated":
		case "Hacked":
		case "RitualTargetChanged":
		case "CrateContentsChanged":
		case "Vacuum":
			UpdateLit(parent.Map);
			break;
		}
	}

	public override void PostExposeData()
	{
		Scribe_Values.Look(ref glowOnInt, "glowOn", defaultValue: false);
		Scribe_Values.Look(ref glowColorOverride, "glowColorOverride", null);
	}

	public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
	{
		base.PostDeSpawn(map, mode);
		UpdateLit(map);
	}

	private List<CompGlower> ExtraSelectedGlowers(GlowerColorChangeType type)
	{
		tmpExtraGlowers.Clear();
		foreach (object item in Find.Selector.SelectedObjectsListForReading)
		{
			if (item == this || !(item is ThingWithComps thingWithComps))
			{
				continue;
			}
			foreach (CompGlower comp in thingWithComps.GetComps<CompGlower>())
			{
				if (type == GlowerColorChangeType.AllGlowers || (type == GlowerColorChangeType.ColorPickerEnabled && comp.Props.colorPickerEnabled) || (type == GlowerColorChangeType.DarklightToggle && comp.Props.darklightToggle))
				{
					tmpExtraGlowers.Add(comp);
				}
			}
		}
		return tmpExtraGlowers;
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__34))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__34(-2)
		{
			_003C_003E4__this = this
		};
	}

	protected virtual void SetGlowColorInternal(ColorInt? color)
	{
		if (ShouldBeLitNow)
		{
			parent.MapHeld.glowGrid.DeRegisterGlower(this);
		}
		glowColorOverride = color;
		if (ShouldBeLitNow)
		{
			parent.MapHeld.glowGrid.RegisterGlower(this);
		}
	}
}
