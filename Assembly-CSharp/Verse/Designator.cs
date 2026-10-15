using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.Sound;
using Verse.Steam;

namespace Verse;

public abstract class Designator : Command
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass37_0
	{
		public List<Thing> things;

		public Designator _003C_003E4__this;

		internal void _003Cget_RightClickFloatMenuOptions_003Eb__0()
		{
			for (int i = 0; i < things.Count; i++)
			{
				Thing t = things[i];
				if (!t.Fogged() && _003C_003E4__this.CanDesignateThing(t).Accepted)
				{
					_003C_003E4__this.DesignateThing(things[i]);
				}
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_RightClickFloatMenuOptions_003Ed__37 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Designator _003C_003E4__this;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap1;

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
		public _003Cget_RightClickFloatMenuOptions_003Ed__37(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Designator CS_0024_003C_003E8__locals30 = _003C_003E4__this;
				DesignationDef designation;
				int num3;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Gizmo)CS_0024_003C_003E8__locals30).RightClickFloatMenuOptions.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0080;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0080;
				case 2:
					_003C_003E1__state = -1;
					goto IL_01eb;
				case 3:
					_003C_003E1__state = -1;
					goto IL_01eb;
				case 4:
					_003C_003E1__state = -1;
					break;
				case 5:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0080:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (CS_0024_003C_003E8__locals30.hasDesignateAllFloatMenuOption)
					{
						_003C_003Ec__DisplayClass37_0 CS_0024_003C_003E8__locals15 = new _003C_003Ec__DisplayClass37_0
						{
							_003C_003E4__this = CS_0024_003C_003E8__locals30
						};
						int num2 = 0;
						CS_0024_003C_003E8__locals15.things = CS_0024_003C_003E8__locals30.Map.listerThings.AllThings;
						for (int i = 0; i < CS_0024_003C_003E8__locals15.things.Count; i++)
						{
							Thing t = CS_0024_003C_003E8__locals15.things[i];
							if (!t.Fogged() && CS_0024_003C_003E8__locals30.CanDesignateThing(t).Accepted)
							{
								num2++;
							}
						}
						if (num2 > 0)
						{
							_003C_003E2__current = new FloatMenuOption(CS_0024_003C_003E8__locals30.designateAllLabel + " (" + "CountToDesignate".Translate(num2) + ")", delegate
							{
								for (int j = 0; j < CS_0024_003C_003E8__locals15.things.Count; j++)
								{
									Thing t2 = CS_0024_003C_003E8__locals15.things[j];
									if (!t2.Fogged() && CS_0024_003C_003E8__locals15._003C_003E4__this.CanDesignateThing(t2).Accepted)
									{
										CS_0024_003C_003E8__locals15._003C_003E4__this.DesignateThing(CS_0024_003C_003E8__locals15.things[j]);
									}
								}
							});
							_003C_003E1__state = 2;
							return true;
						}
						_003C_003E2__current = new FloatMenuOption(CS_0024_003C_003E8__locals30.designateAllLabel + " (" + "NoneLower".Translate() + ")", null);
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_01eb;
					IL_01eb:
					designation = CS_0024_003C_003E8__locals30.Designation;
					if (CS_0024_003C_003E8__locals30.Designation == null)
					{
						break;
					}
					CS_0024_003C_003E8__locals30.tmpAllDesignations.Clear();
					CS_0024_003C_003E8__locals30.tmpAllDesignations.AddRange(CS_0024_003C_003E8__locals30.Map.designationManager.designationsByDef[designation]);
					if (CS_0024_003C_003E8__locals30.removeAllOtherDesignationDefs != null)
					{
						DesignationDef[] removeAllOtherDesignationDefs = CS_0024_003C_003E8__locals30.removeAllOtherDesignationDefs;
						foreach (DesignationDef def in removeAllOtherDesignationDefs)
						{
							CS_0024_003C_003E8__locals30.tmpAllDesignations.AddRange(CS_0024_003C_003E8__locals30.Map.designationManager.designationsByDef[def]);
						}
					}
					num3 = 0;
					foreach (Designation tmpAllDesignation in CS_0024_003C_003E8__locals30.tmpAllDesignations)
					{
						if (CS_0024_003C_003E8__locals30.RemoveAllDesignationsAffects(tmpAllDesignation.target))
						{
							num3++;
						}
					}
					if (num3 > 0)
					{
						_003C_003E2__current = new FloatMenuOption(string.Concat("RemoveAllDesignations".Translate() + " (", num3.ToString(), ")"), delegate
						{
							for (int num4 = CS_0024_003C_003E8__locals30.tmpAllDesignations.Count - 1; num4 >= 0; num4--)
							{
								CS_0024_003C_003E8__locals30.Map.designationManager.RemoveDesignation(CS_0024_003C_003E8__locals30.tmpAllDesignations[num4]);
							}
						});
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003E2__current = new FloatMenuOption("RemoveAllDesignations".Translate() + " (" + "NoneLower".Translate() + ")", null);
					_003C_003E1__state = 5;
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
			_003Cget_RightClickFloatMenuOptions_003Ed__37 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_RightClickFloatMenuOptions_003Ed__37(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	protected bool useMouseIcon;

	public bool isOrder;

	public SoundDef soundDragSustain;

	public SoundDef soundDragChanged;

	public SoundDef soundSucceeded;

	protected SoundDef soundFailed = SoundDefOf.Designate_Failed;

	protected bool hasDesignateAllFloatMenuOption;

	protected string designateAllLabel;

	protected bool showReverseDesignatorDisabledReason;

	protected DesignationDef[] removeAllOtherDesignationDefs;

	private string cachedTutorTagSelect;

	private string cachedTutorTagDesignate;

	protected string cachedHighlightTag;

	private List<Designation> tmpAllDesignations = new List<Designation>();

	private const float IconButtonSize = 48f;

	private const float IconButtonGap = 16f;

	private const float StyleButtonSize = 60f;

	private const float StyleButtonLabelOffset = 27f;

	public Map Map => Find.CurrentMap;

	public virtual bool DragDrawMeasurements => false;

	public virtual bool DrawHighlight => true;

	protected override bool DoTooltip => false;

	public virtual bool AlwaysDoGuiControls => false;

	protected virtual DesignationDef Designation => null;

	public virtual float PanelReadoutTitleExtraRightMargin => 0f;

	public virtual DrawStyleCategoryDef DrawStyleCategory => null;

	public override string TutorTagSelect
	{
		get
		{
			if (tutorTag == null)
			{
				return null;
			}
			return cachedTutorTagSelect ?? (cachedTutorTagSelect = "SelectDesignator-" + tutorTag);
		}
	}

	public string TutorTagDesignate
	{
		get
		{
			if (tutorTag == null)
			{
				return null;
			}
			return cachedTutorTagDesignate ?? (cachedTutorTagDesignate = "Designate-" + tutorTag);
		}
	}

	public override string HighlightTag
	{
		get
		{
			if (cachedHighlightTag == null && tutorTag != null)
			{
				cachedHighlightTag = "Designator-" + tutorTag;
			}
			return cachedHighlightTag;
		}
	}

	public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
	{
		[IteratorStateMachine(typeof(_003Cget_RightClickFloatMenuOptions_003Ed__37))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_RightClickFloatMenuOptions_003Ed__37(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public Designator()
	{
		activateSound = SoundDefOf.Tick_Tiny;
		designateAllLabel = "DesignateAll".Translate();
	}

	protected bool CheckCanInteract()
	{
		if (TutorSystem.TutorialMode && !TutorSystem.AllowAction(TutorTagSelect))
		{
			return false;
		}
		return true;
	}

	public override void ProcessInput(Event ev)
	{
		if (CheckCanInteract())
		{
			base.ProcessInput(ev);
			Find.DesignatorManager.Select(this);
		}
	}

	public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
	{
		GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth, parms);
		if (DebugViewSettings.showArchitectMenuOrder)
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Tiny;
			Widgets.Label(new Rect(topLeft.x, topLeft.y + 5f, GetWidth(maxWidth), 15f), Order.ToString());
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
		}
		return result;
	}

	public Command_Action CreateReverseDesignationGizmo(Thing t)
	{
		AcceptanceReport acceptanceReport = CanDesignateThing(t);
		float angle;
		Vector2 offset;
		if (acceptanceReport.Accepted || (showReverseDesignatorDisabledReason && !acceptanceReport.Reason.NullOrEmpty()))
		{
			return new Command_Action
			{
				defaultLabel = LabelCapReverseDesignating(t),
				icon = IconReverseDesignating(t, out angle, out offset),
				iconAngle = angle,
				iconOffset = offset,
				defaultDesc = (acceptanceReport.Reason.NullOrEmpty() ? DescReverseDesignating(t) : acceptanceReport.Reason),
				Order = ((this is Designator_Uninstall) ? (-11f) : (-20f)),
				Disabled = !acceptanceReport.Accepted,
				action = delegate
				{
					if (TutorSystem.AllowAction(TutorTagDesignate) && (Designation == null || Designation.targetType != 0 || Map.designationManager.DesignationOn(t, Designation) == null))
					{
						DesignateThing(t);
						Finalize(somethingSucceeded: true);
					}
				},
				hotKey = hotKey,
				groupKeyIgnoreContent = groupKeyIgnoreContent,
				groupKey = groupKey
			};
		}
		return null;
	}

	public virtual AcceptanceReport CanDesignateThing(Thing t)
	{
		return AcceptanceReport.WasRejected;
	}

	public virtual void DesignateThing(Thing t)
	{
		throw new NotImplementedException();
	}

	public abstract AcceptanceReport CanDesignateCell(IntVec3 loc);

	public virtual void DesignateMultiCell(IEnumerable<IntVec3> cells)
	{
		if (TutorSystem.TutorialMode && !TutorSystem.AllowAction(new EventPack(TutorTagDesignate, cells)))
		{
			return;
		}
		bool somethingSucceeded = false;
		bool flag = false;
		foreach (IntVec3 cell in cells)
		{
			if (CanDesignateCell(cell).Accepted)
			{
				DesignateSingleCell(cell);
				somethingSucceeded = true;
				if (!flag)
				{
					flag = ShowWarningForCell(cell);
				}
			}
		}
		Finalize(somethingSucceeded);
		if (TutorSystem.TutorialMode)
		{
			TutorSystem.Notify_Event(new EventPack(TutorTagDesignate, cells));
		}
	}

	public virtual void DesignateSingleCell(IntVec3 c)
	{
		throw new NotImplementedException();
	}

	public virtual bool ShowWarningForCell(IntVec3 c)
	{
		return false;
	}

	public void Finalize(bool somethingSucceeded)
	{
		if (somethingSucceeded)
		{
			FinalizeDesignationSucceeded();
		}
		else
		{
			FinalizeDesignationFailed();
		}
	}

	protected virtual void FinalizeDesignationSucceeded()
	{
		if (soundSucceeded != null)
		{
			soundSucceeded.PlayOneShotOnCamera();
		}
	}

	protected virtual void FinalizeDesignationFailed()
	{
		if (soundFailed != null)
		{
			soundFailed.PlayOneShotOnCamera();
		}
		if (Find.DesignatorManager.Dragger.FailureReason != null)
		{
			Messages.Message(Find.DesignatorManager.Dragger.FailureReason, MessageTypeDefOf.RejectInput, historical: false);
		}
	}

	public virtual string LabelCapReverseDesignating(Thing t)
	{
		return LabelCap;
	}

	public virtual string DescReverseDesignating(Thing t)
	{
		return Desc;
	}

	public virtual Texture2D IconReverseDesignating(Thing t, out float angle, out Vector2 offset)
	{
		angle = iconAngle;
		offset = iconOffset;
		return (Texture2D)icon;
	}

	protected virtual bool RemoveAllDesignationsAffects(LocalTargetInfo target)
	{
		return true;
	}

	public virtual void DrawMouseAttachments()
	{
		if (useMouseIcon)
		{
			GenUI.DrawMouseAttachment(hideMouseIcon ? null : icon, mouseText, iconAngle, iconOffset, null, null, drawTextBackground: false, default(Color), null);
		}
	}

	public virtual void DrawPanelReadout(ref float curY, float width)
	{
	}

	public virtual void DoExtraGuiControls(float leftX, float bottomY)
	{
		Rect winRect = new Rect(leftX, bottomY - 90f, 200f, 90f);
		DrawStyleDef style = Find.DesignatorManager.SelectedStyle;
		List<DrawStyleDef> list = DrawStyleCategory?.styles;
		if (style == null || list == null || list.Count <= 1)
		{
			return;
		}
		Find.WindowStack.ImmediateWindow(415111, winRect, WindowLayer.GameUI, delegate
		{
			using (new TextBlock(GameFont.Medium, TextAnchor.MiddleCenter))
			{
				Rect rect = winRect;
				rect.x = 0f;
				rect.y = 0f;
				Rect rect2 = rect.MiddlePartPixels(48f, 48f);
				if (Widgets.ButtonImage(rect2, style.uiIcon))
				{
					List<FloatMenuOption> list2 = new List<FloatMenuOption>();
					foreach (DrawStyleDef style2 in DrawStyleCategory.styles)
					{
						DrawStyleDef lDef = style2;
						list2.Add(new FloatMenuOption(style2.LabelCap, delegate
						{
							Find.DesignatorManager.SelectedStyle = lDef;
						}));
					}
					Find.WindowStack.Add(new FloatMenu(list2));
				}
				Rect rect3 = rect.MiddlePartPixels(60f, 60f);
				rect3.x = rect2.xMin - 16f - rect3.width;
				Rect rect4 = rect2;
				rect4.x = rect3.xMin + 27f;
				rect4.xMax = rect3.xMax + 27f;
				Rect butRect = rect3;
				butRect.x = rect2.xMax + 16f;
				Rect rect5 = rect2;
				rect5.x = butRect.xMin - 27f;
				rect5.xMax = butRect.xMax - 27f;
				if (Widgets.ButtonImage(rect3, TexUI.ConcaveArrowTexLeft))
				{
					SoundDefOf.DragSlider.PlayOneShotOnCamera();
					Find.DesignatorManager.PreviousDrawStyle();
					Event.current.Use();
				}
				if (!SteamDeck.IsSteamDeck)
				{
					Widgets.Label(rect4, KeyBindingDefOf.Designator_PreviousDrawStyle.MainKeyLabel);
				}
				if (Widgets.ButtonImage(butRect, TexUI.ConcaveArrowTexRight))
				{
					SoundDefOf.DragSlider.PlayOneShotOnCamera();
					Find.DesignatorManager.AdvanceDrawStyle();
					Event.current.Use();
				}
				if (!SteamDeck.IsSteamDeck)
				{
					Widgets.Label(rect5, KeyBindingDefOf.Designator_NextDrawStyle.MainKeyLabel);
				}
			}
		});
	}

	public virtual void SelectedUpdate()
	{
	}

	public virtual void SelectedProcessInput(Event ev)
	{
		if (KeyBindingDefOf.Designator_NextDrawStyle.KeyDownEvent)
		{
			SoundDefOf.DragSlider.PlayOneShotOnCamera();
			Find.DesignatorManager.AdvanceDrawStyle();
		}
		if (KeyBindingDefOf.Designator_PreviousDrawStyle.KeyDownEvent)
		{
			SoundDefOf.DragSlider.PlayOneShotOnCamera();
			Find.DesignatorManager.PreviousDrawStyle();
		}
	}

	public virtual bool CanRemainSelected()
	{
		return true;
	}

	public virtual void Selected()
	{
	}

	public virtual void Deselected()
	{
	}

	public virtual void RenderHighlight(List<IntVec3> dragCells)
	{
		DesignatorUtility.RenderHighlightOverSelectableThings(this, dragCells);
	}
}
