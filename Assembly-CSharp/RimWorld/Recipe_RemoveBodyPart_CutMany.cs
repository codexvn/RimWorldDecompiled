using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class Recipe_RemoveBodyPart_CutMany : Recipe_RemoveBodyPart_Cut
{
	[CompilerGenerated]
	private sealed class _003CGetPartsToApplyOn_003Ed__0 : IEnumerable<BodyPartRecord>, IEnumerable, IEnumerator<BodyPartRecord>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private BodyPartRecord _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Recipe_RemoveBodyPart_CutMany _003C_003E4__this;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private RecipeDef recipe;

		public RecipeDef _003C_003E3__recipe;

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
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Recipe_RemoveBodyPart_CutMany recipe_RemoveBodyPart_CutMany = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				IEnumerable<BodyPartRecord> source = recipe_RemoveBodyPart_CutMany.FixedPartsToApplyOn(pawn, recipe);
				if (source.Any() && recipe.minPartCount <= source.Count())
				{
					_003C_003E2__current = source.First();
					_003C_003E1__state = 1;
					return true;
				}
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
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
				_003CGetPartsToApplyOn_003Ed__ = new _003CGetPartsToApplyOn_003Ed__0(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
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
			_003C_003E4__this = this,
			_003C_003E3__pawn = pawn,
			_003C_003E3__recipe = recipe
		};
	}

	public IEnumerable<BodyPartRecord> FixedPartsToApplyOn(Pawn pawn, RecipeDef recipe)
	{
		return MedicalRecipesUtility.GetFixedPartsToApplyOn(recipe, pawn, (BodyPartRecord record) => !pawn.health.hediffSet.PartIsMissing(record));
	}

	public override void DamagePart(Pawn pawn, BodyPartRecord part)
	{
		foreach (BodyPartRecord item in FixedPartsToApplyOn(pawn, recipe))
		{
			pawn.TakeDamage(new DamageInfo(DamageDefOf.SurgicalCut, 99999f, 999f, -1f, null, item));
		}
	}

	public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
	{
		return recipe.label;
	}
}
