using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class Recipe_RemoveImplant : Recipe_Surgery
{
	[CompilerGenerated]
	private sealed class _003CGetPartsToApplyOn_003Ed__0 : IEnumerable<BodyPartRecord>, IEnumerable, IEnumerator<BodyPartRecord>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private BodyPartRecord _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private RecipeDef recipe;

		public RecipeDef _003C_003E3__recipe;

		private List<Hediff> _003CallHediffs_003E5__2;

		private int _003Ci_003E5__3;

		BodyPartRecord IEnumerator<BodyPartRecord>.Current
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
		public _003CGetPartsToApplyOn_003Ed__0(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CallHediffs_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00c0;
			}
			_003C_003E1__state = -1;
			_003CallHediffs_003E5__2 = pawn.health.hediffSet.hediffs;
			_003Ci_003E5__3 = 0;
			goto IL_00d0;
			IL_00c0:
			_003Ci_003E5__3++;
			goto IL_00d0;
			IL_00d0:
			if (_003Ci_003E5__3 < _003CallHediffs_003E5__2.Count)
			{
				if (_003CallHediffs_003E5__2[_003Ci_003E5__3].Part != null && _003CallHediffs_003E5__2[_003Ci_003E5__3].def == recipe.removesHediff && _003CallHediffs_003E5__2[_003Ci_003E5__3].Visible)
				{
					_003C_003E2__current = _003CallHediffs_003E5__2[_003Ci_003E5__3].Part;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00c0;
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
		IEnumerator<BodyPartRecord> IEnumerable<BodyPartRecord>.GetEnumerator()
		{
			_003CGetPartsToApplyOn_003Ed__0 _003CGetPartsToApplyOn_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetPartsToApplyOn_003Ed__ = this;
			}
			else
			{
				_003CGetPartsToApplyOn_003Ed__ = new _003CGetPartsToApplyOn_003Ed__0(0);
			}
			_003CGetPartsToApplyOn_003Ed__.pawn = _003C_003E3__pawn;
			_003CGetPartsToApplyOn_003Ed__.recipe = _003C_003E3__recipe;
			return _003CGetPartsToApplyOn_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<BodyPartRecord>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CGetPartsToApplyOn_003Ed__0))]
	public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetPartsToApplyOn_003Ed__0(-2)
		{
			_003C_003E3__pawn = pawn,
			_003C_003E3__recipe = recipe
		};
	}

	public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
	{
		MedicalRecipesUtility.IsClean(pawn, part);
		bool flag = IsViolationOnPawn(pawn, part, Faction.OfPlayer);
		if (billDoer != null)
		{
			if (CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
			{
				return;
			}
			TaleRecorder.RecordTale(TaleDefOf.DidSurgery, billDoer, pawn);
			if (!pawn.health.hediffSet.GetNotMissingParts().Contains(part))
			{
				return;
			}
			Hediff hediff = pawn.health.hediffSet.hediffs.FirstOrDefault((Hediff x) => x.def == recipe.removesHediff);
			if (hediff != null)
			{
				if (hediff.def.spawnThingOnRemoved != null)
				{
					GenSpawn.Spawn(hediff.def.spawnThingOnRemoved, billDoer.Position, billDoer.Map);
				}
				pawn.health.RemoveHediff(hediff);
			}
		}
		if (flag)
		{
			ReportViolation(pawn, billDoer, pawn.HomeFaction, -70);
		}
	}
}
