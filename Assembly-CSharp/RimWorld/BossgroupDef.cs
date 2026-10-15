using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class BossgroupDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__16 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public BossgroupDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		string IEnumerator<string>.Current
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
		public _003CConfigErrors_003Ed__16(int _003C_003E1__state)
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
				BossgroupDef bossgroupDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)bossgroupDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007a;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007a;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00b9;
				case 3:
					_003C_003E1__state = -1;
					goto IL_00e3;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_007a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (bossgroupDef.boss == null)
					{
						_003C_003E2__current = "boss required for all bossgroups";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00b9;
					IL_00e3:
					if (bossgroupDef.repeatWaveStartIndex >= bossgroupDef.waves.Count)
					{
						_003C_003E2__current = "repeatWaveStartIndex must be lower than wave count.";
						_003C_003E1__state = 4;
						return true;
					}
					break;
					IL_00b9:
					if (bossgroupDef.waves.NullOrEmpty())
					{
						_003C_003E2__current = "no waves defined.";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_00e3;
				}
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__16 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__16(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public BossDef boss;

	public List<BossGroupWave> waves = new List<BossGroupWave>();

	public int repeatWaveStartIndex;

	public Type workerClass = typeof(BossgroupWorker);

	[MustTranslate]
	public string leaderDescription;

	public QuestScriptDef quest;

	public ThingDef rewardDef;

	private BossgroupWorker workerInt;

	private List<string> tmpEntries = new List<string>();

	public BossgroupWorker Worker
	{
		get
		{
			if (workerInt == null && workerClass != null)
			{
				workerInt = (BossgroupWorker)Activator.CreateInstance(workerClass);
				workerInt.def = this;
			}
			return workerInt;
		}
	}

	public string LeaderDescription => leaderDescription.Formatted(NamedArgumentUtility.Named(boss.kindDef, "LEADERKIND"));

	public int GetWaveIndex(int timesSummoned)
	{
		int num = timesSummoned;
		if (num >= waves.Count)
		{
			Rand.PushState(Gen.HashCombine(Find.World.info.Seed, timesSummoned));
			num = Rand.Range(repeatWaveStartIndex, waves.Count);
			Rand.PopState();
		}
		return num;
	}

	public BossGroupWave GetWave(int index)
	{
		return waves[GetWaveIndex(index)];
	}

	public string GetWaveDescription(int waveIndex)
	{
		BossGroupWave wave = GetWave(GetWaveIndex(waveIndex));
		tmpEntries.Clear();
		string text = GenLabel.BestKindLabel(boss.kindDef, Gender.None).CapitalizeFirst();
		if (!wave.bossApparel.NullOrEmpty())
		{
			text = "BossWithApparel".Translate(text.Named("BOSS"), wave.bossApparel.Select((ThingDef a) => a.label).ToCommaList(useAnd: true).Named("APPAREL"));
		}
		tmpEntries.Add(text + " x" + wave.bossCount);
		foreach (PawnKindDefCount escort in wave.escorts)
		{
			tmpEntries.Add(GenLabel.BestKindLabel(escort.kindDef, Gender.None).CapitalizeFirst() + " x" + escort.count);
		}
		return tmpEntries.ToLineList("  - ");
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__16))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__16(-2)
		{
			_003C_003E4__this = this
		};
	}
}
