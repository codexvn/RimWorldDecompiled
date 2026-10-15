using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class CompInteractable_GravshipShieldGenerator : CompInteractable
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__11 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompInteractable_GravshipShieldGenerator _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__11(int _003C_003E1__state)
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
			CompInteractable_GravshipShieldGenerator CS_0024_003C_003E8__locals7 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (!CS_0024_003C_003E8__locals7.parent.SpawnedOrAnyParentSpawned)
				{
					return false;
				}
				string defaultLabel;
				string defaultDesc;
				if (!CS_0024_003C_003E8__locals7.Shield.Active)
				{
					defaultLabel = "GravshipShieldOrderActivation".Translate() + "...";
					defaultDesc = "GravshipShieldOrderActivationDesc".Translate(CS_0024_003C_003E8__locals7.parent.Named("THING"));
				}
				else
				{
					defaultLabel = "GravshipShieldOrderDeactivation".Translate() + "...";
					defaultDesc = "GravshipShieldOrderDeactivationDesc".Translate(CS_0024_003C_003E8__locals7.parent.Named("THING"));
				}
				Command_Action command_Action = new Command_Action
				{
					defaultLabel = defaultLabel,
					defaultDesc = defaultDesc,
					icon = CS_0024_003C_003E8__locals7.UIIcon,
					groupable = false,
					action = delegate
					{
						SoundDefOf.Tick_Tiny.PlayOneShotOnCamera();
						Find.Targeter.BeginTargeting(CS_0024_003C_003E8__locals7);
					}
				};
				AcceptanceReport acceptanceReport = CS_0024_003C_003E8__locals7.CanInteract();
				if (!acceptanceReport.Accepted)
				{
					command_Action.Disable(acceptanceReport.Reason.CapitalizeFirst());
				}
				_003C_003E2__current = command_Action;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__11 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__11(0)
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

	private const int MinHitPointsToActivate = 100;

	private CompProjectileInterceptor shield;

	private CompGravshipFacility facility;

	private CompProjectileInterceptor Shield => shield ?? parent.TryGetComp<CompProjectileInterceptor>();

	private CompGravshipFacility Facility => facility ?? parent.TryGetComp<CompGravshipFacility>();

	protected override string ActivateOptionLabel => Shield.Active ? "Deactivate".Translate() : "Activate".Translate();

	public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
	{
		if (!ModsConfig.OdysseyActive)
		{
			return false;
		}
		if (Facility.LinkedBuildings.NullOrEmpty())
		{
			return "ShieldNotConnectedToGravship".Translate();
		}
		if (Shield.Charging)
		{
			return "ShieldOnCooldown".Translate();
		}
		if (!Shield.Active && Shield.currentHitPoints < 100)
		{
			return "ShieldBelowMinHitpoints".Translate();
		}
		return base.CanInteract(activateBy, checkOptionalItems);
	}

	protected override void OnInteracted(Pawn caster)
	{
		if (!Shield.Active)
		{
			Shield.Activate();
		}
		else
		{
			Shield.Deactivate();
		}
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__11))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__11(-2)
		{
			_003C_003E4__this = this
		};
	}
}
