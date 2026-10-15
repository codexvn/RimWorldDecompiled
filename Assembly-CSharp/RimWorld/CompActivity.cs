using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld;

public sealed class CompActivity : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__28 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompActivity _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__28(int _003C_003E1__state)
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
			CompActivity CS_0024_003C_003E8__locals11 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals11.Deactivated)
				{
					return false;
				}
				if (CS_0024_003C_003E8__locals11.gizmo == null)
				{
					CS_0024_003C_003E8__locals11.gizmo = new ActivityGizmo(CS_0024_003C_003E8__locals11.parent);
				}
				if (Find.Selector.SelectedObjects.Count == 1 && CS_0024_003C_003E8__locals11.IsDormant)
				{
					_003C_003E2__current = CS_0024_003C_003E8__locals11.gizmo;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_008a;
			case 1:
				_003C_003E1__state = -1;
				goto IL_008a;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Activity +5%",
					action = delegate
					{
						CS_0024_003C_003E8__locals11.AdjustActivity(0.05f);
					}
				};
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: " + (CS_0024_003C_003E8__locals11.IsDormant ? "Go active" : "Go passive"),
					action = delegate
					{
						CS_0024_003C_003E8__locals11.activityLevelPercent = (CS_0024_003C_003E8__locals11.IsDormant ? 1f : 0f);
					}
				};
				_003C_003E1__state = 4;
				return true;
			case 4:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_008a:
				if (!DebugSettings.godMode)
				{
					return false;
				}
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Activity -5%",
					action = delegate
					{
						CS_0024_003C_003E8__locals11.AdjustActivity(-0.05f);
					}
				};
				_003C_003E1__state = 2;
				return true;
			}
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
			_003CCompGetGizmosExtra_003Ed__28 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__28(0)
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

	private float activityLevelPercent;

	private ActivityState state;

	private bool triggeredNaturally;

	private bool deactivated;

	public bool suppressionEnabled;

	public float suppressIfAbove = 0.4f;

	private float lastGraphicDirtyPercent;

	private Gizmo gizmo;

	public CompProperties_Activity Props => (CompProperties_Activity)props;

	public float ActivityLevel => activityLevelPercent;

	public bool IsActive => state == ActivityState.Active;

	public bool IsDormant => state == ActivityState.Passive;

	public bool Deactivated => deactivated;

	public ActivityState State => state;

	public float ActivityResearchFactor => Props.activityResearchFactorCurve.Evaluate(ActivityLevel);

	public bool CanBeSuppressed
	{
		get
		{
			foreach (IActivity comp in parent.GetComps<IActivity>())
			{
				if (!comp.CanBeSuppressed())
				{
					return false;
				}
			}
			return true;
		}
	}

	public override void Initialize(CompProperties props)
	{
		base.Initialize(props);
		SetActivity(Props.startingRange.RandomInRange, dontDirtyGraphic: true);
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		base.PostSpawnSetup(respawningAfterLoad);
		LessonAutoActivator.TeachOpportunity(ConceptDefOf.SuppressingEntities, OpportunityType.Important);
	}

	public override void CompTick()
	{
		AdjustActivity(Props.Worker.GetChangeRatePerDay(parent) / 60000f);
	}

	public override string CompInspectStringExtra()
	{
		if (Deactivated)
		{
			return "Deactivated".Translate() + ".";
		}
		if (IsActive)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder();
		Props.Worker.GetInspectString(parent, stringBuilder);
		return stringBuilder.ToString();
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__28))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__28(-2)
		{
			_003C_003E4__this = this
		};
	}

	public void AdjustActivity(float delta)
	{
		SetActivity(activityLevelPercent + delta);
	}

	public void SetActivity(float activity, bool dontDirtyGraphic = false)
	{
		if (!Deactivated)
		{
			activityLevelPercent = Mathf.Clamp01(activity);
			if (activityLevelPercent >= 1f && state != ActivityState.Active && CanActivate())
			{
				EnterActiveState();
			}
			else if (activityLevelPercent <= 0f && state != 0)
			{
				EnterPassiveState();
			}
			if (!dontDirtyGraphic && Props.dirtyGraphicsOnActivityChange && parent is Pawn pawn && Mathf.Abs(activityLevelPercent - lastGraphicDirtyPercent) > 0.01f)
			{
				lastGraphicDirtyPercent = activityLevelPercent;
				pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
		}
	}

	private bool CanActivate()
	{
		foreach (IActivity comp in parent.GetComps<IActivity>())
		{
			if (!comp.CanActivate())
			{
				return false;
			}
		}
		return true;
	}

	public bool ShouldGoPassive()
	{
		if (Deactivated)
		{
			return false;
		}
		foreach (IActivity comp in parent.GetComps<IActivity>())
		{
			if (comp.ShouldGoPassive())
			{
				return true;
			}
		}
		return false;
	}

	public void EnterActiveState()
	{
		if (Deactivated)
		{
			return;
		}
		triggeredNaturally = activityLevelPercent >= 1f;
		activityLevelPercent = 1f;
		state = ActivityState.Active;
		foreach (IActivity comp in parent.GetComps<IActivity>())
		{
			comp.OnActivityActivated();
		}
		if ((triggeredNaturally || Props.showLetterOnManualActivation) && Props.showLetterOnActivated)
		{
			Find.LetterStack.ReceiveLetter(Props.letterTitle, Props.letterDesc, Props.letterDef, parent);
		}
	}

	public void EnterPassiveState()
	{
		if (Deactivated)
		{
			return;
		}
		activityLevelPercent = 0f;
		state = ActivityState.Passive;
		foreach (IActivity comp in parent.GetComps<IActivity>())
		{
			comp.OnPassive();
		}
	}

	public void Deactivate()
	{
		if (IsActive)
		{
			EnterPassiveState();
		}
		activityLevelPercent = 0f;
		deactivated = true;
		suppressionEnabled = false;
		suppressIfAbove = 0.5f;
		gizmo = null;
		parent.GetComp<CompStudiable>()?.Notify_ActivityDeactivated();
	}

	public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
	{
		if (Deactivated || !(totalDamageDealt > 0f) || !IsDormant || !(Props.changePerDamage > 0f))
		{
			return;
		}
		float x = 1f;
		if (parent is Pawn pawn)
		{
			x = pawn.health.summaryHealth.SummaryHealthPercent;
		}
		else if (parent.def.useHitPoints)
		{
			x = (float)parent.HitPoints / (float)parent.MaxHitPoints;
		}
		float num = Props.changePerDamage * totalDamageDealt * Props.damagedActivityMultiplierCurve.Evaluate(x);
		SetActivity(activityLevelPercent + num);
		if (!IsActive)
		{
			TaggedString taggedString = "MessageActivityRisingDamage".Translate(parent.LabelNoParenthesis);
			if (MessagesRepeatAvoider.MessageShowAllowed(taggedString, 15f))
			{
				Messages.Message(taggedString, parent, MessageTypeDefOf.CautionInput);
			}
		}
	}

	public void Notify_HeldOnPlatform()
	{
		suppressionEnabled = true;
	}

	public override void PostExposeData()
	{
		Scribe_Values.Look(ref activityLevelPercent, "activityPercent", 0f);
		Scribe_Values.Look(ref state, "state", ActivityState.Passive);
		Scribe_Values.Look(ref suppressIfAbove, "suppressIfAbove", 0f);
		Scribe_Values.Look(ref suppressionEnabled, "suppressionEnabled", defaultValue: false);
		Scribe_Values.Look(ref triggeredNaturally, "triggeredNaturally", defaultValue: false);
		Scribe_Values.Look(ref lastGraphicDirtyPercent, "lastGraphicDirtyPercent", 0f);
		Scribe_Values.Look(ref deactivated, "deactivated", defaultValue: false);
	}
}
