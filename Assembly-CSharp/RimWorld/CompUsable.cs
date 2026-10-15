using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld;

public class CompUsable : ThingComp, ITargetingSource
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass31_0
	{
		public CompUsable _003C_003E4__this;

		public Pawn myPawn;

		internal void _003CCompFloatMenuOptions_003Eb__0()
		{
			foreach (CompUseEffect comp in _003C_003E4__this.parent.GetComps<CompUseEffect>())
			{
				if (comp.SelectedUseOption(myPawn))
				{
					return;
				}
			}
			_003C_003E4__this.TryStartUseJob(myPawn, _003C_003E4__this.GetExtraTarget(myPawn), _003C_003E4__this.Props.ignoreOtherReservations);
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompFloatMenuOptions_003Ed__31 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompUsable _003C_003E4__this;

		private Pawn myPawn;

		public Pawn _003C_003E3__myPawn;

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
		public _003CCompFloatMenuOptions_003Ed__31(int _003C_003E1__state)
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
			CompUsable compUsable = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass31_0 CS_0024_003C_003E8__locals10 = new _003C_003Ec__DisplayClass31_0
				{
					_003C_003E4__this = _003C_003E4__this,
					myPawn = myPawn
				};
				if (compUsable.Props.useJob == null)
				{
					return false;
				}
				AcceptanceReport acceptanceReport = compUsable.CanBeUsedBy(CS_0024_003C_003E8__locals10.myPawn, compUsable.Props.ignoreOtherReservations);
				FloatMenuOption floatMenuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(compUsable.FloatMenuOptionLabel(CS_0024_003C_003E8__locals10.myPawn), delegate
				{
					foreach (CompUseEffect comp in CS_0024_003C_003E8__locals10._003C_003E4__this.parent.GetComps<CompUseEffect>())
					{
						if (comp.SelectedUseOption(CS_0024_003C_003E8__locals10.myPawn))
						{
							return;
						}
					}
					CS_0024_003C_003E8__locals10._003C_003E4__this.TryStartUseJob(CS_0024_003C_003E8__locals10.myPawn, CS_0024_003C_003E8__locals10._003C_003E4__this.GetExtraTarget(CS_0024_003C_003E8__locals10.myPawn), CS_0024_003C_003E8__locals10._003C_003E4__this.Props.ignoreOtherReservations);
				}, priority: compUsable.Props.floatMenuOptionPriority, iconTex: compUsable.Icon, iconColor: compUsable.IconColor), CS_0024_003C_003E8__locals10.myPawn, compUsable.parent);
				if (!acceptanceReport.Accepted)
				{
					floatMenuOption.Disabled = true;
					floatMenuOption.Label = floatMenuOption.Label + " (" + acceptanceReport.Reason + ")";
				}
				_003C_003E2__current = floatMenuOption;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				return false;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CCompFloatMenuOptions_003Ed__31 _003CCompFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCompFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CCompFloatMenuOptions_003Ed__ = new _003CCompFloatMenuOptions_003Ed__31(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CCompFloatMenuOptions_003Ed__.myPawn = _003C_003E3__myPawn;
			return _003CCompFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__34 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompUsable _003C_003E4__this;

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
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CompUsable CS_0024_003C_003E8__locals5 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!CS_0024_003C_003E8__locals5.Props.showUseGizmo)
				{
					return false;
				}
				_003C_003E2__current = new Command_Action
				{
					icon = CS_0024_003C_003E8__locals5.parent.def.uiIcon,
					defaultLabel = $"{"UseGizmo".Translate()} {CS_0024_003C_003E8__locals5.parent.def.label}...",
					defaultDesc = "UseGizmoTooltip".Translate(CS_0024_003C_003E8__locals5.parent.def.label),
					action = delegate
					{
						SoundDefOf.Tick_Tiny.PlayOneShotOnCamera();
						Find.Targeter.BeginTargeting(CS_0024_003C_003E8__locals5);
					}
				};
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
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

	private Texture2D icon;

	private Color? iconColor;

	public CompProperties_Usable Props => (CompProperties_Usable)props;

	private Texture2D Icon
	{
		get
		{
			if (icon == null && Props.floatMenuFactionIcon != null)
			{
				icon = Find.FactionManager.FirstFactionOfDef(Props.floatMenuFactionIcon)?.def?.FactionIcon;
			}
			return icon;
		}
	}

	private Color IconColor
	{
		get
		{
			if (!iconColor.HasValue && Props.floatMenuFactionIcon != null)
			{
				iconColor = Find.FactionManager.FirstFactionOfDef(Props.floatMenuFactionIcon)?.Color;
			}
			return iconColor ?? Color.white;
		}
	}

	public bool CasterIsPawn => true;

	public bool IsMeleeAttack => false;

	public bool Targetable => true;

	public bool MultiSelect => false;

	public bool HidePawnTooltips => false;

	public Thing Caster => parent;

	public Pawn CasterPawn => null;

	public Verb GetVerb => null;

	public TargetingParameters targetParams => TargetingParameters.ForPawns();

	public virtual ITargetingSource DestinationSelector => null;

	public Texture2D UIIcon => null;

	protected virtual string FloatMenuOptionLabel(Pawn pawn)
	{
		return Props.useLabel.Formatted(parent);
	}

	[IteratorStateMachine(typeof(_003CCompFloatMenuOptions_003Ed__31))]
	public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn myPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompFloatMenuOptions_003Ed__31(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__myPawn = myPawn
		};
	}

	public virtual LocalTargetInfo GetExtraTarget(Pawn pawn)
	{
		return LocalTargetInfo.Invalid;
	}

	public virtual void TryStartUseJob(Pawn pawn, LocalTargetInfo extraTarget, bool forced = false)
	{
		if (Props.useJob == null || !CanBeUsedBy(pawn, forced))
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (CompUseEffect comp in parent.GetComps<CompUseEffect>())
		{
			TaggedString taggedString = comp.ConfirmMessage(pawn);
			if (!taggedString.NullOrEmpty())
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendTagged(taggedString);
			}
		}
		string text = stringBuilder.ToString();
		if (text.NullOrEmpty())
		{
			StartJob();
		}
		else
		{
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(text, StartJob));
		}
		void StartJob()
		{
			if (extraTarget == pawn)
			{
				extraTarget = LocalTargetInfo.Invalid;
			}
			Job job = (extraTarget.IsValid ? JobMaker.MakeJob(Props.useJob, parent, extraTarget) : JobMaker.MakeJob(Props.useJob, parent));
			job.count = 1;
			pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}
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

	public virtual void UsedBy(Pawn p)
	{
		if (!CanBeUsedBy(p, forced: false, ignoreReserveAndReachable: true))
		{
			return;
		}
		foreach (CompUseEffect item in from x in parent.GetComps<CompUseEffect>()
			orderby x.OrderPriority descending
			select x)
		{
			try
			{
				item.DoEffect(p);
			}
			catch (Exception ex)
			{
				Log.Error("Error in CompUseEffect: " + ex);
			}
		}
		if (Props.useMessage != null)
		{
			Messages.Message(Props.useMessage.Formatted(p.Named("PAWN"), parent), parent, MessageTypeDefOf.NeutralEvent);
		}
	}

	public virtual AcceptanceReport CanBeUsedBy(Pawn p, bool forced = false, bool ignoreReserveAndReachable = false)
	{
		if (!p.RaceProps.IsFlesh)
		{
			return false;
		}
		if (p.IsMutant && !Props.allowedMutants.Contains(p.mutant.Def))
		{
			return false;
		}
		PlanetTile tile = p.MapHeld.Tile;
		if (tile.Valid && !Props.layerWhitelist.NullOrEmpty() && !Props.layerWhitelist.Contains(tile.LayerDef))
		{
			return "CannotPerformPlanetLayer".Translate(tile.LayerDef.gerundLabel.Named("GERUND"), tile.LayerDef.label.Named("LAYER")).Resolve();
		}
		if (tile.Valid && !Props.layerBlacklist.NullOrEmpty() && Props.layerBlacklist.Contains(tile.LayerDef))
		{
			return "CannotPerformPlanetLayer".Translate(tile.LayerDef.gerundLabel.Named("GERUND"), tile.LayerDef.label.Named("LAYER")).Resolve();
		}
		if (parent.TryGetComp<CompPowerTrader>(out var comp) && !comp.PowerOn)
		{
			return "NoPower".Translate();
		}
		if (!ignoreReserveAndReachable && !p.CanReach(parent, PathEndMode.Touch, Danger.Deadly))
		{
			return "NoPath".Translate();
		}
		if (!ignoreReserveAndReachable && !p.CanReserve(parent, 1, -1, null, forced))
		{
			Pawn pawn = p.Map.reservationManager.FirstRespectedReserver(parent, p) ?? p.Map.physicalInteractionReservationManager.FirstReserverOf(parent);
			if (pawn != null)
			{
				return "ReservedBy".Translate(pawn.LabelShort, pawn);
			}
			return "Reserved".Translate();
		}
		if (Props.userMustHaveHediff != null && !p.health.hediffSet.HasHediff(Props.userMustHaveHediff))
		{
			return "MustHaveHediff".Translate(Props.userMustHaveHediff);
		}
		List<ThingComp> allComps = parent.AllComps;
		for (int i = 0; i < allComps.Count; i++)
		{
			if (allComps[i] is CompUseEffect compUseEffect)
			{
				AcceptanceReport result = compUseEffect.CanBeUsedBy(p);
				if (!result.Accepted)
				{
					return result;
				}
			}
		}
		return true;
	}

	public bool CanHitTarget(LocalTargetInfo target)
	{
		return ValidateTarget(target, showMessages: false);
	}

	public bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
	{
		if (!target.IsValid || !target.TryGetPawn(out var pawn) || !pawn.IsPlayerControlled)
		{
			return false;
		}
		AcceptanceReport acceptanceReport = CanBeUsedBy(pawn, Props.ignoreOtherReservations);
		if (!acceptanceReport.Accepted)
		{
			if (showMessages && !acceptanceReport.Reason.NullOrEmpty())
			{
				SendCannotUseMessage(pawn, acceptanceReport.Reason);
			}
			return false;
		}
		return true;
	}

	public void SendCannotUseMessage(Pawn pawn, string reason)
	{
		Messages.Message($"{"CannotGenericWorkCustom".Translate(FloatMenuOptionLabel(pawn))}: {reason.CapitalizeFirst()}", pawn, MessageTypeDefOf.RejectInput, historical: false);
	}

	public void DrawHighlight(LocalTargetInfo target)
	{
		if (target.IsValid)
		{
			GenDraw.DrawTargetHighlight(target);
		}
	}

	public void OrderForceTarget(LocalTargetInfo target)
	{
		foreach (CompUseEffect comp in parent.GetComps<CompUseEffect>())
		{
			if (comp.SelectedUseOption(target.Pawn))
			{
				return;
			}
		}
		TryStartUseJob(target.Pawn, GetExtraTarget(target.Pawn), Props.ignoreOtherReservations);
	}

	public void OnGUI(LocalTargetInfo target)
	{
		Widgets.MouseAttachedLabel("UseGizmoMouse".Translate(), 0f, 0f, null);
		if (ValidateTarget(target, showMessages: false))
		{
			GenUI.DrawMouseAttachment(TexCommand.Attack);
		}
		else
		{
			GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
		}
	}
}
