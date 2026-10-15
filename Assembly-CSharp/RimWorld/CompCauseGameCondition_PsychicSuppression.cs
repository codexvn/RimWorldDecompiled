using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class CompCauseGameCondition_PsychicSuppression : CompCauseGameCondition
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__3 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompCauseGameCondition_PsychicSuppression _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__3(int _003C_003E1__state)
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
			CompCauseGameCondition_PsychicSuppression CS_0024_003C_003E8__locals5 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!Prefs.DevMode || !DebugSettings.godMode)
				{
					return false;
				}
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = CS_0024_003C_003E8__locals5.gender.GetLabel(),
					action = delegate
					{
						if (CS_0024_003C_003E8__locals5.gender == Gender.Female)
						{
							CS_0024_003C_003E8__locals5.gender = Gender.Male;
						}
						else
						{
							CS_0024_003C_003E8__locals5.gender = Gender.Female;
						}
						CS_0024_003C_003E8__locals5.ReSetupAllConditions();
					},
					hotKey = KeyBindingDefOf.Misc1
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
			_003CCompGetGizmosExtra_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__3(0)
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

	public Gender gender;

	public override void Initialize(CompProperties props)
	{
		base.Initialize(props);
		gender = (Rand.Bool ? Gender.Male : Gender.Female);
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref gender, "gender", Gender.None);
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__3))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__3(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void CompTick()
	{
		base.CompTick();
		if (!Active || !base.MyTile.Valid)
		{
			return;
		}
		foreach (Caravan caravan in Find.World.worldObjects.Caravans)
		{
			if (!(Find.WorldGrid.ApproxDistanceInTiles(caravan.Tile, base.MyTile) < (float)base.Props.worldRange))
			{
				continue;
			}
			foreach (Pawn pawn in caravan.pawns)
			{
				GameCondition_PsychicSuppression.CheckPawn(pawn, gender);
			}
		}
	}

	protected override void SetupCondition(GameCondition condition, Map map)
	{
		base.SetupCondition(condition, map);
		((GameCondition_PsychicSuppression)condition).gender = gender;
	}

	public override string CompInspectStringExtra()
	{
		string text = base.CompInspectStringExtra();
		if (!text.NullOrEmpty())
		{
			text += "\n";
		}
		return text + ("AffectedGender".Translate() + ": " + gender.GetLabel().CapitalizeFirst());
	}

	public override void RandomizeSettings(Site site)
	{
		gender = (Rand.Bool ? Gender.Male : Gender.Female);
	}
}
