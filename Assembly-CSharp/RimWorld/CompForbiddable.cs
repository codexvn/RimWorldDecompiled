using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompForbiddable : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__14 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompForbiddable _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__14(int _003C_003E1__state)
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
			CompForbiddable CS_0024_003C_003E8__locals12 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals12.parent is Building && !CS_0024_003C_003E8__locals12.Props.allowNonPlayer && CS_0024_003C_003E8__locals12.parent.Faction != Faction.OfPlayer)
				{
					return false;
				}
				if (CS_0024_003C_003E8__locals12.parent.SpawnedParentOrMe is Building building && CS_0024_003C_003E8__locals12.parent != building)
				{
					return false;
				}
				Command_Toggle command_Toggle = new Command_Toggle
				{
					hotKey = KeyBindingDefOf.Command_ItemForbid,
					icon = TexCommand.ForbidOff,
					isActive = () => !CS_0024_003C_003E8__locals12.Forbidden,
					defaultLabel = "CommandAllow".TranslateWithBackup("DesignatorUnforbid"),
					activateIfAmbiguous = false
				};
				if (CS_0024_003C_003E8__locals12.forbiddenInt)
				{
					command_Toggle.defaultDesc = "CommandForbiddenDesc".TranslateWithBackup("DesignatorUnforbidDesc");
				}
				else
				{
					command_Toggle.defaultDesc = "CommandNotForbiddenDesc".TranslateWithBackup("DesignatorForbidDesc");
				}
				if (CS_0024_003C_003E8__locals12.parent.def.IsDoor)
				{
					command_Toggle.tutorTag = "ToggleForbidden-Door";
					command_Toggle.toggleAction = delegate
					{
						CS_0024_003C_003E8__locals12.Forbidden = !CS_0024_003C_003E8__locals12.Forbidden;
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.ForbiddingDoors, KnowledgeAmount.SpecificInteraction);
					};
				}
				else
				{
					command_Toggle.tutorTag = "ToggleForbidden";
					command_Toggle.toggleAction = delegate
					{
						CS_0024_003C_003E8__locals12.Forbidden = !CS_0024_003C_003E8__locals12.Forbidden;
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Forbidding, KnowledgeAmount.SpecificInteraction);
					};
				}
				_003C_003E2__current = command_Toggle;
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
			_003CCompGetGizmosExtra_003Ed__14 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__14(0)
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

	private bool forbiddenInt;

	private OverlayHandle? overlayHandle;

	public CompProperties_Forbiddable Props => (CompProperties_Forbiddable)props;

	public bool Forbidden
	{
		get
		{
			return forbiddenInt;
		}
		set
		{
			if (value == forbiddenInt)
			{
				return;
			}
			forbiddenInt = value;
			if (parent.Spawned)
			{
				if (forbiddenInt)
				{
					parent.Map.listerHaulables.Notify_Forbidden(parent);
					parent.Map.listerMergeables.Notify_Forbidden(parent);
				}
				else
				{
					parent.Map.listerHaulables.Notify_Unforbidden(parent);
					parent.Map.listerMergeables.Notify_Unforbidden(parent);
				}
				if (parent is Building_Door)
				{
					parent.Map.reachability.ClearCache();
				}
			}
			UpdateOverlayHandle();
		}
	}

	private OverlayTypes MyOverlayType
	{
		get
		{
			if (parent is Blueprint || parent is Frame)
			{
				if (parent.def.size.x > 1 || parent.def.size.z > 1)
				{
					return OverlayTypes.ForbiddenBig;
				}
				return OverlayTypes.Forbidden;
			}
			if (parent.def.category == ThingCategory.Building)
			{
				return OverlayTypes.ForbiddenBig;
			}
			return OverlayTypes.Forbidden;
		}
	}

	private void UpdateOverlayHandle()
	{
		if (parent.Spawned)
		{
			parent.Map.overlayDrawer.Disable(parent, ref overlayHandle);
			if (parent.Spawned && forbiddenInt)
			{
				overlayHandle = parent.Map.overlayDrawer.Enable(parent, MyOverlayType);
			}
		}
	}

	public override void PostPostMake()
	{
		base.PostPostMake();
		if (Props.forbidOnMake)
		{
			forbiddenInt = true;
			parent.SetForbidden(value: true);
		}
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		UpdateOverlayHandle();
	}

	public override void PostExposeData()
	{
		Scribe_Values.Look(ref forbiddenInt, "forbidden", defaultValue: false);
	}

	public override void PostSplitOff(Thing piece)
	{
		piece.SetForbidden(forbiddenInt);
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__14))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__14(-2)
		{
			_003C_003E4__this = this
		};
	}
}
