using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompReadable : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CGetDoers_003Ed__9<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable where T : ReadingOutcomeDoer
	{
		private int _003C_003E1__state;

		private T _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompReadable _003C_003E4__this;

		private int _003Ci_003E5__2;

		T IEnumerator<T>.Current
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
		public _003CGetDoers_003Ed__9(int _003C_003E1__state)
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
			CompReadable compReadable = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_006c;
			}
			_003C_003E1__state = -1;
			if (compReadable.doers == null)
			{
				return false;
			}
			_003Ci_003E5__2 = 0;
			goto IL_007c;
			IL_007c:
			if (_003Ci_003E5__2 < compReadable.doers.Count)
			{
				if (compReadable.doers[_003Ci_003E5__2] is T val)
				{
					_003C_003E2__current = val;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_006c;
			}
			return false;
			IL_006c:
			_003Ci_003E5__2++;
			goto IL_007c;
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
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			_003CGetDoers_003Ed__9<T> result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetDoers_003Ed__9<T>(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}
	}

	protected List<ReadingOutcomeDoer> doers;

	public IEnumerable<ReadingOutcomeDoer> Doers => doers;

	public CompProperties_Readable Props => (CompProperties_Readable)props;

	public override void Initialize(CompProperties props)
	{
		base.Initialize(props);
		InitializeDoers();
	}

	public override void PostPostMake()
	{
		base.PostPostMake();
		InitializeDoers();
		if (doers != null)
		{
			for (int i = 0; i < doers.Count; i++)
			{
				doers[i].PostMake();
			}
		}
	}

	public bool TryGetDoer<T>(out T doer) where T : ReadingOutcomeDoer
	{
		if (doers == null)
		{
			doer = null;
			return false;
		}
		for (int i = 0; i < doers.Count; i++)
		{
			if (doers[i] is T val)
			{
				doer = val;
				return true;
			}
		}
		doer = null;
		return false;
	}

	public T GetDoer<T>() where T : ReadingOutcomeDoer
	{
		if (doers == null)
		{
			return null;
		}
		for (int i = 0; i < doers.Count; i++)
		{
			if (doers[i] is T result)
			{
				return result;
			}
		}
		return null;
	}

	[IteratorStateMachine(typeof(_003CGetDoers_003Ed__9<>))]
	public IEnumerable<T> GetDoers<T>() where T : ReadingOutcomeDoer
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetDoers_003Ed__9<T>(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void InitializeDoers()
	{
		if (Props.doers.Empty())
		{
			return;
		}
		doers = new List<ReadingOutcomeDoer>();
		for (int i = 0; i < Props.doers.Count; i++)
		{
			ReadingOutcomeDoer readingOutcomeDoer = null;
			ReadingOutcomeProperties readingOutcomeProperties = Props.doers[i];
			try
			{
				readingOutcomeDoer = (ReadingOutcomeDoer)Activator.CreateInstance(readingOutcomeProperties.DoerClass);
				doers.Add(readingOutcomeDoer);
				readingOutcomeDoer.Initialize(parent, readingOutcomeProperties);
			}
			catch (Exception value)
			{
				Log.Error($"Could not instantiate or initialize reading outcome doer ({readingOutcomeProperties.DoerClass}): {value}");
				doers.Remove(readingOutcomeDoer);
			}
		}
	}

	public override void PostExposeData()
	{
		if (Scribe.mode == LoadSaveMode.LoadingVars)
		{
			InitializeDoers();
		}
		if (doers != null)
		{
			for (int i = 0; i < doers.Count; i++)
			{
				doers[i].PostExposeData();
			}
		}
	}
}
