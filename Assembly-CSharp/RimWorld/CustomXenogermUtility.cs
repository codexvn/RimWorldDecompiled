using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public static class CustomXenogermUtility
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass2_0
	{
		public GeneSet templateGeneset;

		internal bool _003CGetMatchingGenepacks_003Eb__0(Genepack gp)
		{
			return gp.GeneSet.Matches(templateGeneset);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetMatchingGenepacks_003Ed__2 : IEnumerable<Genepack>, IEnumerable, IEnumerator<Genepack>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Genepack _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IEnumerable<Genepack> genepacks;

		public IEnumerable<Genepack> _003C_003E3__genepacks;

		private IEnumerable<GeneSet> templateGenesets;

		public IEnumerable<GeneSet> _003C_003E3__templateGenesets;

		private IEnumerator<GeneSet> _003C_003E7__wrap1;

		private Genepack _003CgenePack_003E5__3;

		Genepack IEnumerator<Genepack>.Current
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
		public _003CGetMatchingGenepacks_003Ed__2(int _003C_003E1__state)
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
			_003CgenePack_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					tmpGenepacks.Remove(_003CgenePack_003E5__3);
					goto IL_00bf;
				}
				_003C_003E1__state = -1;
				tmpGenepacks.Clear();
				tmpGenepacks.AddRange(genepacks);
				_003C_003E7__wrap1 = templateGenesets.GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_00c6;
				IL_00bf:
				_003CgenePack_003E5__3 = null;
				goto IL_00c6;
				IL_00c6:
				if (_003C_003E7__wrap1.MoveNext())
				{
					_003C_003Ec__DisplayClass2_0 CS_0024_003C_003E8__locals1 = new _003C_003Ec__DisplayClass2_0
					{
						templateGeneset = _003C_003E7__wrap1.Current
					};
					_003CgenePack_003E5__3 = tmpGenepacks.FirstOrDefault((Genepack gp) => gp.GeneSet.Matches(CS_0024_003C_003E8__locals1.templateGeneset));
					if (_003CgenePack_003E5__3 != null)
					{
						_003C_003E2__current = _003CgenePack_003E5__3;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_00bf;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
				tmpGenepacks.Clear();
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
		IEnumerator<Genepack> IEnumerable<Genepack>.GetEnumerator()
		{
			_003CGetMatchingGenepacks_003Ed__2 _003CGetMatchingGenepacks_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetMatchingGenepacks_003Ed__ = this;
			}
			else
			{
				_003CGetMatchingGenepacks_003Ed__ = new _003CGetMatchingGenepacks_003Ed__2(0);
			}
			_003CGetMatchingGenepacks_003Ed__.templateGenesets = _003C_003E3__templateGenesets;
			_003CGetMatchingGenepacks_003Ed__.genepacks = _003C_003E3__genepacks;
			return _003CGetMatchingGenepacks_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Genepack>)this).GetEnumerator();
		}
	}

	private static List<Genepack> tmpGenepacks = new List<Genepack>();

	public static AcceptanceReport SaveXenogermTemplate(string xenoTypeName, XenotypeIconDef iconDef, List<Genepack> selectedGenepacks)
	{
		if (selectedGenepacks.NullOrEmpty())
		{
			return "CannotSaveXenogermTemplate".Translate("GenepackPlural".Translate());
		}
		if (xenoTypeName.NullOrEmpty())
		{
			return "CannotSaveXenogermTemplate".Translate("XenotypeName".Translate());
		}
		CustomXenogerm customXenogerm = new CustomXenogerm(xenoTypeName, iconDef, selectedGenepacks);
		Find.CustomXenogermDatabase.Add(customXenogerm);
		Messages.Message("XenogermTemplateSaved".Translate(xenoTypeName.Named("NAME")), MessageTypeDefOf.PositiveEvent, historical: false);
		return true;
	}

	[IteratorStateMachine(typeof(_003CGetMatchingGenepacks_003Ed__2))]
	public static IEnumerable<Genepack> GetMatchingGenepacks(IEnumerable<GeneSet> templateGenesets, IEnumerable<Genepack> genepacks)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetMatchingGenepacks_003Ed__2(-2)
		{
			_003C_003E3__templateGenesets = templateGenesets,
			_003C_003E3__genepacks = genepacks
		};
	}
}
