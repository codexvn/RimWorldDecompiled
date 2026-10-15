using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

[StaticConstructorOnStartup]
public class Building_PodLauncher : Building, INotifyLaunchableLaunch
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__2 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_PodLauncher _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

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
		public _003CGetGizmos_003Ed__2(int _003C_003E1__state)
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
				Building_PodLauncher CS_0024_003C_003E8__locals7 = _003C_003E4__this;
				Designator_Build designator_Build;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building)CS_0024_003C_003E8__locals7).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0078;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0078;
				case 2:
					_003C_003E1__state = -1;
					goto IL_0156;
				case 3:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_0078:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					designator_Build = BuildCopyCommandUtility.FindAllowedDesignator(ThingDefOf.TransportPod);
					if (designator_Build != null)
					{
						AcceptanceReport acceptanceReport = GenConstruct.CanPlaceBlueprintAt(ThingDefOf.TransportPod, FuelingPortUtility.GetFuelingPortCell(CS_0024_003C_003E8__locals7), ThingDefOf.TransportPod.defaultPlacingRot, CS_0024_003C_003E8__locals7.Map);
						Command_Action command_Action = new Command_Action
						{
							defaultLabel = "BuildThing".Translate(ThingDefOf.TransportPod.label),
							icon = designator_Build.icon,
							defaultDesc = designator_Build.Desc,
							action = delegate
							{
								IntVec3 fuelingPortCell = FuelingPortUtility.GetFuelingPortCell(CS_0024_003C_003E8__locals7);
								GenConstruct.PlaceBlueprintForBuild(ThingDefOf.TransportPod, fuelingPortCell, CS_0024_003C_003E8__locals7.Map, ThingDefOf.TransportPod.defaultPlacingRot, Faction.OfPlayer, null);
							}
						};
						if (!acceptanceReport.Accepted)
						{
							command_Action.Disable(acceptanceReport.Reason);
						}
						_003C_003E2__current = command_Action;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0156;
					IL_0156:
					_003C_003E2__current = new Command_Toggle
					{
						icon = AutoBuildIcon,
						defaultLabel = "AutoBuildTransportPod".Translate(),
						defaultDesc = "AutoBuildTransportPodDesc".Translate(),
						isActive = () => CS_0024_003C_003E8__locals7.autoPlacePods,
						toggleAction = CS_0024_003C_003E8__locals7.ToggleAutoBuildTransportPods
					};
					_003C_003E1__state = 3;
					return true;
				}
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__2 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__2(0)
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

	public bool autoPlacePods;

	private static readonly Texture2D AutoBuildIcon = ContentFinder<Texture2D>.Get("UI/Commands/AutoBuildTransportPod");

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__2))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__2(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref autoPlacePods, "autoPlacePods", defaultValue: false);
	}

	private void ToggleAutoBuildTransportPods()
	{
		autoPlacePods = !autoPlacePods;
		if (autoPlacePods)
		{
			CheckPlacePod();
		}
	}

	private void CheckPlacePod()
	{
		if (autoPlacePods)
		{
			IntVec3 fuelingPortCell = FuelingPortUtility.GetFuelingPortCell(this);
			if (GenConstruct.CanPlaceBlueprintAt(ThingDefOf.TransportPod, fuelingPortCell, ThingDefOf.TransportPod.defaultPlacingRot, base.Map).Accepted)
			{
				GenConstruct.PlaceBlueprintForBuild(ThingDefOf.TransportPod, fuelingPortCell, base.Map, ThingDefOf.TransportPod.defaultPlacingRot, Faction.OfPlayer, null);
			}
		}
	}

	public void Notify_LaunchableLaunched(CompLaunchable launchable)
	{
		CheckPlacePod();
	}
}
