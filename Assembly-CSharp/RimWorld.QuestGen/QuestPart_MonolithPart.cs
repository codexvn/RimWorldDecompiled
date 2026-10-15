using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen;

public class QuestPart_MonolithPart : QuestPart_DescriptionPart
{
	[CompilerGenerated]
	private sealed class _003Cget_QuestLookTargets_003Ed__4 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_MonolithPart _003C_003E4__this;

		private IEnumerator<GlobalTargetInfo> _003C_003E7__wrap1;

		GlobalTargetInfo IEnumerator<GlobalTargetInfo>.Current
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
		public _003Cget_QuestLookTargets_003Ed__4(int _003C_003E1__state)
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
				QuestPart_MonolithPart questPart_MonolithPart = _003C_003E4__this;
				bool flag;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_MonolithPart).QuestLookTargets.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0074;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0074;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0074:
					if (_003C_003E7__wrap1.MoveNext())
					{
						GlobalTargetInfo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					flag = false;
					if (ModsConfig.IdeologyActive)
					{
						foreach (Quest item in Find.QuestManager.QuestsListForReading)
						{
							if (item.State == QuestState.Ongoing && (item.root == QuestScriptDefOf.EndGame_ArchonexusVictory_FirstCycle || item.root == QuestScriptDefOf.EndGame_ArchonexusVictory_SecondCycle))
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						_003C_003E2__current = questPart_MonolithPart.monolith;
						_003C_003E1__state = 2;
						return true;
					}
					break;
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
		IEnumerator<GlobalTargetInfo> IEnumerable<GlobalTargetInfo>.GetEnumerator()
		{
			_003Cget_QuestLookTargets_003Ed__4 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_QuestLookTargets_003Ed__4(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<GlobalTargetInfo>)this).GetEnumerator();
		}
	}

	private Building_VoidMonolith monolith;

	public override IEnumerable<GlobalTargetInfo> QuestLookTargets
	{
		[IteratorStateMachine(typeof(_003Cget_QuestLookTargets_003Ed__4))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_QuestLookTargets_003Ed__4(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public QuestPart_MonolithPart()
	{
	}

	public QuestPart_MonolithPart(Building_VoidMonolith monolith)
	{
		inSignalEnable = "MonolithLevelChanged";
		this.monolith = monolith;
	}

	public override void Notify_QuestSignalReceived(Signal signal)
	{
		base.Notify_QuestSignalReceived(signal);
		if (signal.tag == "MonolithLevelChanged" || signal.tag == "EntityDiscovered")
		{
			UpdateDescription();
		}
	}

	private void UpdateDescription()
	{
		if (Find.Anomaly.LevelDef.postEndgame)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		int num = Find.Anomaly.Level;
		if (Find.Anomaly.LevelDef == MonolithLevelDefOf.Gleaming)
		{
			num--;
		}
		stringBuilder.AppendLine(Find.Anomaly.LevelDef.extraQuestDescription);
		stringBuilder.AppendLine();
		stringBuilder.AppendLine(string.Concat("CurrentMonolithLevel".Translate() + ": ", num.ToString()));
		EntityCategoryDef entityCategoryDef = Find.Anomaly.NextLevelDef?.entityCatagoryCompletionRequired;
		if (entityCategoryDef != null)
		{
			stringBuilder.AppendLine();
			int num2 = Find.EntityCodex.DiscoveredCount(entityCategoryDef);
			if (num2 >= Find.Anomaly.NextLevelDef.entityCountCompletionRequired)
			{
				stringBuilder.AppendLine("VoidMonolithAllEntitiesDiscovered".Translate()).AppendLine(Find.Anomaly.LevelDef.activateQuestText);
			}
			else
			{
				int entityCountCompletionRequired = Find.Anomaly.NextLevelDef.entityCountCompletionRequired;
				stringBuilder.AppendLine("VoidMonolithRequiredEntityCategory".Translate(entityCountCompletionRequired, entityCategoryDef.label)).AppendLine().AppendLine("DiscoveredCount".Translate(num2 + " / " + entityCountCompletionRequired).CapitalizeFirst());
			}
		}
		resolvedDescriptionPart = stringBuilder.ToString();
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref monolith, "monolith");
	}

	public override void Cleanup()
	{
		base.Cleanup();
		monolith = null;
	}
}
