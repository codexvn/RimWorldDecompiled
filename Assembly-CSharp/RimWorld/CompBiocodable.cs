using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompBiocodable : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__21 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompBiocodable _003C_003E4__this;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CSpecialDisplayStats_003Ed__21(int _003C_003E1__state)
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
			CompBiocodable compBiocodable = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (compBiocodable.biocoded)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "Stat_Thing_Biocoded_Name".Translate(), compBiocodable.codedPawnLabel, "Stat_Thing_Biocoded_Desc".Translate(), 1104);
					_003C_003E1__state = 1;
					return true;
				}
				break;
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__21 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__21(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	protected bool biocoded;

	protected string codedPawnLabel;

	protected Pawn codedPawn;

	public bool Biocoded => biocoded;

	public Pawn CodedPawn => codedPawn;

	public string CodedPawnLabel => codedPawnLabel;

	public CompProperties_Biocodable Props => (CompProperties_Biocodable)props;

	public virtual bool Biocodable => true;

	public static bool IsBiocoded(Thing thing)
	{
		return thing.TryGetComp<CompBiocodable>()?.Biocoded ?? false;
	}

	public static bool IsBiocodedFor(Thing thing, Pawn pawn)
	{
		CompBiocodable compBiocodable = thing.TryGetComp<CompBiocodable>();
		if (compBiocodable != null)
		{
			return compBiocodable.CodedPawn == pawn;
		}
		return false;
	}

	public virtual void CodeFor(Pawn p)
	{
		if (Biocodable)
		{
			biocoded = true;
			codedPawn = p;
			codedPawnLabel = p.Name.ToStringFull;
			OnCodedFor(p);
		}
	}

	protected virtual void OnCodedFor(Pawn p)
	{
	}

	public virtual void UnCode()
	{
		biocoded = false;
		codedPawn = null;
		codedPawnLabel = null;
	}

	public override void Notify_Equipped(Pawn pawn)
	{
		if (Biocodable && Props.biocodeOnEquip)
		{
			CodeFor(pawn);
		}
	}

	public override string TransformLabel(string label)
	{
		if (!biocoded)
		{
			return label;
		}
		return "Biocoded".Translate(label, parent.def).Resolve();
	}

	public override string CompInspectStringExtra()
	{
		if (!biocoded)
		{
			return string.Empty;
		}
		return "CodedFor".Translate(codedPawnLabel.ApplyTag(TagType.Name)).Resolve();
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__21))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__21(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref biocoded, "biocoded", defaultValue: false);
		Scribe_Values.Look(ref codedPawnLabel, "biocodedPawnLabel");
		if (Scribe.mode == LoadSaveMode.Saving && codedPawn != null && codedPawn.Discarded)
		{
			codedPawn = null;
		}
		Scribe_References.Look(ref codedPawn, "codedPawn", saveDestroyedThings: true);
	}
}
