using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class MechanitorControlGroupGizmo : Gizmo
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass16_0
	{
		public MechanitorControlGroup controlGroup;
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass16_1
	{
		public MechWorkModeDef wm;

		public _003C_003Ec__DisplayClass16_0 CS_0024_003C_003E8__locals1;

		internal void _003CGetWorkModeOptions_003Eb__1()
		{
			CS_0024_003C_003E8__locals1.controlGroup.SetWorkMode(wm);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetWorkModeOptions_003Ed__16 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private MechanitorControlGroup controlGroup;

		public MechanitorControlGroup _003C_003E3__controlGroup;

		private _003C_003Ec__DisplayClass16_0 _003C_003E8__1;

		private IEnumerator<MechWorkModeDef> _003C_003E7__wrap1;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CGetWorkModeOptions_003Ed__16(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass16_0();
					_003C_003E8__1.controlGroup = controlGroup;
					_003C_003E7__wrap1 = DefDatabase<MechWorkModeDef>.AllDefsListForReading.OrderBy((MechWorkModeDef d) => d.uiOrder).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					_003C_003Ec__DisplayClass16_1 CS_0024_003C_003E8__locals6 = new _003C_003Ec__DisplayClass16_1
					{
						CS_0024_003C_003E8__locals1 = _003C_003E8__1,
						wm = _003C_003E7__wrap1.Current
					};
					_003C_003E2__current = new FloatMenuOption(CS_0024_003C_003E8__locals6.wm.LabelCap, delegate
					{
						CS_0024_003C_003E8__locals6.CS_0024_003C_003E8__locals1.controlGroup.SetWorkMode(CS_0024_003C_003E8__locals6.wm);
					}, CS_0024_003C_003E8__locals6.wm.uiIcon, Color.white)
					{
						tooltip = new TipSignal(CS_0024_003C_003E8__locals6.wm.description, CS_0024_003C_003E8__locals6.wm.index ^ 0xDFE8661)
					};
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetWorkModeOptions_003Ed__16 _003CGetWorkModeOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetWorkModeOptions_003Ed__ = this;
			}
			else
			{
				_003CGetWorkModeOptions_003Ed__ = new _003CGetWorkModeOptions_003Ed__16(0);
			}
			_003CGetWorkModeOptions_003Ed__.controlGroup = _003C_003E3__controlGroup;
			return _003CGetWorkModeOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	public const int InRectPadding = 6;

	private const float Width = 130f;

	private const int IconButtonSize = 26;

	private const float BaseSelectedTexJump = 20f;

	private const float BaseSelectedTextScale = 0.8f;

	private static readonly CachedTexture PowerIcon = new CachedTexture("UI/Icons/MechRechargeSettings");

	private static readonly Color UncontrolledMechBackgroundColor = new Color32(byte.MaxValue, 25, 25, 55);

	private MechanitorControlGroup controlGroup;

	private List<MechanitorControlGroup> mergedControlGroups;

	public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions => GetWorkModeOptions(controlGroup);

	public override bool Visible
	{
		get
		{
			if (controlGroup.MechsForReading.Count <= 0)
			{
				return Find.Selector.SelectedPawns.Count == 1;
			}
			return true;
		}
	}

	public override float Order
	{
		get
		{
			if (controlGroup.MechsForReading.Count > 0)
			{
				return -89f;
			}
			return -88f;
		}
	}

	public MechanitorControlGroupGizmo(MechanitorControlGroup controlGroup)
	{
		this.controlGroup = controlGroup;
		Order = -89f;
	}

	[IteratorStateMachine(typeof(_003CGetWorkModeOptions_003Ed__16))]
	public static IEnumerable<FloatMenuOption> GetWorkModeOptions(MechanitorControlGroup controlGroup)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetWorkModeOptions_003Ed__16(-2)
		{
			_003C_003E3__controlGroup = controlGroup
		};
	}

	public override void GizmoUpdateOnMouseover()
	{
		base.GizmoUpdateOnMouseover();
		controlGroup.WorkMode.Worker.DrawControlGroupMouseOverExtra(controlGroup);
	}

	public override bool GroupsWith(Gizmo other)
	{
		if (!(other is MechanitorControlGroupGizmo mechanitorControlGroupGizmo))
		{
			return false;
		}
		if (mechanitorControlGroupGizmo.controlGroup == controlGroup)
		{
			return true;
		}
		if (controlGroup.Tracker == mechanitorControlGroupGizmo.controlGroup.Tracker && controlGroup.MechsForReading.Count == 0 && mechanitorControlGroupGizmo.controlGroup.MechsForReading.Count == 0)
		{
			return true;
		}
		if (mergedControlGroups.NotNullAndContains(mechanitorControlGroupGizmo.controlGroup))
		{
			mergedControlGroups.Remove(mechanitorControlGroupGizmo.controlGroup);
		}
		return false;
	}

	public override void MergeWith(Gizmo gizmo)
	{
		if (!(gizmo is MechanitorControlGroupGizmo mechanitorControlGroupGizmo))
		{
			Log.ErrorOnce("Tried to merge MechanitorControlGroupGizmo with unexpected type", 345234235);
		}
		else if (mechanitorControlGroupGizmo.controlGroup != controlGroup)
		{
			if (mergedControlGroups == null)
			{
				mergedControlGroups = new List<MechanitorControlGroup>();
			}
			if (!mergedControlGroups.Contains(mechanitorControlGroupGizmo.controlGroup))
			{
				mergedControlGroups.Add(mechanitorControlGroupGizmo.controlGroup);
			}
		}
	}

	public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
	{
		if (!ModLister.CheckBiotech("Mechanitor control group gizmo"))
		{
			return new GizmoResult(GizmoState.Clear);
		}
		AcceptanceReport canControlMechs = controlGroup.Tracker.CanControlMechs;
		disabled = !canControlMechs;
		disabledReason = canControlMechs.Reason;
		Rect rect = new Rect(topLeft.x, topLeft.y, GetWidth(maxWidth), 75f);
		Rect rect2 = rect.ContractedBy(6f);
		bool flag = Mouse.IsOver(rect2);
		List<Pawn> mechsForReading = controlGroup.MechsForReading;
		Color white = Color.white;
		Material material = ((disabled || parms.lowLight || mechsForReading.Count <= 0) ? TexUI.GrayscaleGUI : null);
		GUI.color = (parms.lowLight ? Command.LowLightBgColor : white);
		GenUI.DrawTextureWithMaterial(rect, parms.shrunk ? Command.BGTexShrunk : Command.BGTex, material);
		GUI.color = Color.white;
		Text.Font = GameFont.Small;
		Text.Anchor = TextAnchor.UpperLeft;
		Rect rect3 = rect2;
		TaggedString str = ((!mergedControlGroups.NullOrEmpty()) ? "Groups".Translate() : "Group".Translate());
		str += " " + controlGroup.Index;
		if (!mergedControlGroups.NullOrEmpty())
		{
			mergedControlGroups.SortBy((MechanitorControlGroup c) => c.Index);
			for (int i = 0; i < mergedControlGroups.Count; i++)
			{
				str += ", " + mergedControlGroups[i].Index;
			}
		}
		str = str.Truncate(rect2.width);
		Vector2 vector = Text.CalcSize(str);
		rect3.width = vector.x;
		rect3.height = vector.y;
		Widgets.Label(rect3, str);
		if (mechsForReading.Count <= 0)
		{
			GUI.color = ColoredText.SubtleGrayColor;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect2, "(" + "NoMechs".Translate() + ")");
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			return new GizmoResult(GizmoState.Clear);
		}
		if (Mouse.IsOver(rect3))
		{
			Widgets.DrawHighlight(rect3);
			if (Widgets.ButtonInvisible(rect3))
			{
				Find.Selector.ClearSelection();
				for (int j = 0; j < mechsForReading.Count; j++)
				{
					Find.Selector.Select(mechsForReading[j]);
				}
			}
		}
		bool flag2 = false;
		Rect rect4 = new Rect(rect.x + rect.width - 26f - 6f, rect.y + 6f, 26f, 26f);
		Widgets.DrawTextureFitted(rect4, PowerIcon.Texture, 1f);
		if (!disabled && Mouse.IsOver(rect4))
		{
			flag2 = true;
			Widgets.DrawHighlight(rect4);
			if (Widgets.ButtonInvisible(rect4))
			{
				Find.WindowStack.Add(new Dialog_RechargeSettings(controlGroup));
			}
		}
		bool flag3 = false;
		Rect rect5 = new Rect(rect.x + rect.width - 52f - 6f, rect.y + 6f, 26f, 26f);
		Widgets.DrawTextureFitted(rect5, controlGroup.WorkMode.uiIcon, 1f);
		if (!disabled && Mouse.IsOver(rect5))
		{
			flag3 = true;
			Widgets.DrawHighlight(rect5);
		}
		Rect rect6 = new Rect(rect2.x, rect2.y + 26f + 4f, rect2.width, rect2.height - 26f - 4f);
		float num = rect6.height;
		int num2 = 0;
		int num3 = 0;
		for (float num4 = num; num4 >= 0f; num4 -= 1f)
		{
			num2 = Mathf.FloorToInt(rect6.width / num4);
			num3 = Mathf.FloorToInt(rect6.height / num4);
			if (num2 * num3 >= mechsForReading.Count)
			{
				num = num4;
				break;
			}
		}
		float num5 = (rect6.width - (float)num2 * num) / 2f;
		float num6 = (rect6.height - (float)num3 * num) / 2f;
		int num7 = 0;
		for (int k = 0; k < num2; k++)
		{
			for (int l = 0; l < num2; l++)
			{
				if (num7 >= mechsForReading.Count)
				{
					break;
				}
				Rect rect7 = new Rect(rect6.x + (float)l * num + num5, rect6.y + (float)k * num + num6, num, num);
				Pawn pawn = mechsForReading[num7];
				Vector2 size = rect7.size;
				Rot4 east = Rot4.East;
				float controlGroupPortraitZoom = mechsForReading[num7].kindDef.controlGroupPortraitZoom;
				RenderTexture image = PortraitsCache.Get(pawn, size, east, default(Vector3), controlGroupPortraitZoom, supersample: true, compensateForUIScale: true, renderHeadgear: true, renderClothes: true, null, null, stylingStation: false, null);
				if (!controlGroup.Tracker.ControlledPawns.Contains(mechsForReading[num7]))
				{
					Widgets.DrawRectFast(rect7, UncontrolledMechBackgroundColor);
				}
				GUI.DrawTexture(rect7, image);
				if (Mouse.IsOver(rect7))
				{
					Widgets.DrawHighlight(rect7);
					MouseoverSounds.DoRegion(rect7, SoundDefOf.Mouseover_Command);
					if (Event.current.type == EventType.mouseDown)
					{
						if (Event.current.shift)
						{
							Find.Selector.Select(mechsForReading[num7]);
						}
						else
						{
							CameraJumper.TryJumpAndSelect(mechsForReading[num7]);
						}
					}
					TargetHighlighter.Highlight(mechsForReading[num7], arrow: true, colonistBar: false);
				}
				if (Find.Selector.IsSelected(mechsForReading[num7]))
				{
					SelectionDrawerUtility.DrawSelectionOverlayOnGUI(mechsForReading[num7], rect7, 0.8f / (float)num2, 20f);
				}
				num7++;
			}
			if (num7 >= mechsForReading.Count)
			{
				break;
			}
		}
		if (Find.WindowStack.FloatMenu == null && !flag2)
		{
			TooltipHandler.TipRegion(rect, delegate
			{
				string text = string.Concat("ControlGroup".Translate() + " #", controlGroup.Index.ToString()).Colorize(ColoredText.TipSectionTitleColor) + "\n\n";
				text = text + ("CurrentMechWorkMode".Translate() + ": " + controlGroup.WorkMode.LabelCap).Colorize(ColoredText.TipSectionTitleColor) + "\n" + controlGroup.WorkMode.description + "\n\n";
				IEnumerable<string> entries = from x in controlGroup.MechsForReading
					where x.needs?.energy != null
					select x into m
					select (m.LabelCap + " (" + m.needs.energy.CurLevelPercentage.ToStringPercent() + " " + "EnergyLower".Translate() + ")").Resolve();
				text = text + "AssignedMechs".Translate().Colorize(ColoredText.TipSectionTitleColor) + "\n" + entries.ToLineList(" - ");
				if (disabled && !disabledReason.NullOrEmpty())
				{
					text += ("\n\n" + "DisabledCommand".Translate() + ": " + disabledReason).Colorize(ColorLibrary.RedReadable);
				}
				return text;
			}, 2545872);
		}
		if (flag3 && Event.current.type == EventType.mouseDown)
		{
			return new GizmoResult(GizmoState.OpenedFloatMenu, Event.current);
		}
		return new GizmoResult(flag ? GizmoState.Mouseover : GizmoState.Clear);
	}

	public override float GetWidth(float maxWidth)
	{
		return 130f;
	}
}
