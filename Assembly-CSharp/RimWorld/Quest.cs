using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B79 RID: 7033
	public class Quest : IExposable, ILoadReferenceable, ISignalReceiver
	{
		// Token: 0x17001814 RID: 6164
		// (get) Token: 0x06009CCC RID: 40140 RVA: 0x002D79E7 File Offset: 0x002D5BE7
		public List<QuestPart> PartsListForReading
		{
			get
			{
				return this.parts;
			}
		}

		// Token: 0x17001815 RID: 6165
		// (get) Token: 0x06009CCD RID: 40141 RVA: 0x002D79EF File Offset: 0x002D5BEF
		public int TicksSinceAppeared
		{
			get
			{
				return Find.TickManager.TicksGame - this.appearanceTick;
			}
		}

		// Token: 0x17001816 RID: 6166
		// (get) Token: 0x06009CCE RID: 40142 RVA: 0x002D7A02 File Offset: 0x002D5C02
		public int TicksSinceAccepted
		{
			get
			{
				if (this.acceptanceTick >= 0)
				{
					return Find.TickManager.TicksGame - this.acceptanceTick;
				}
				return -1;
			}
		}

		// Token: 0x17001817 RID: 6167
		// (get) Token: 0x06009CCF RID: 40143 RVA: 0x002D7A20 File Offset: 0x002D5C20
		public int TicksSinceCleanup
		{
			get
			{
				if (!this.cleanedUp)
				{
					return -1;
				}
				return Find.TickManager.TicksGame - this.cleanupTick;
			}
		}

		// Token: 0x17001818 RID: 6168
		// (get) Token: 0x06009CD0 RID: 40144 RVA: 0x002D7A3D File Offset: 0x002D5C3D
		public int TicksUntilExpiry
		{
			get
			{
				if (this.acceptanceExpireTick == -1)
				{
					return -1;
				}
				return Mathf.Max(this.acceptanceExpireTick - Find.TickManager.TicksGame, 0);
			}
		}

		// Token: 0x17001819 RID: 6169
		// (get) Token: 0x06009CD1 RID: 40145 RVA: 0x002D7A61 File Offset: 0x002D5C61
		public string AccepterPawnLabelCap
		{
			get
			{
				if (this.accepterPawn == null)
				{
					return this.accepterPawnLabel;
				}
				return this.accepterPawn.LabelCap;
			}
		}

		// Token: 0x1700181A RID: 6170
		// (get) Token: 0x06009CD2 RID: 40146 RVA: 0x002D7A7D File Offset: 0x002D5C7D
		public string AddedSignal
		{
			get
			{
				return string.Format("Quest{0}.Added", this.id);
			}
		}

		// Token: 0x1700181B RID: 6171
		// (get) Token: 0x06009CD3 RID: 40147 RVA: 0x002D7A94 File Offset: 0x002D5C94
		public string InitiateSignal
		{
			get
			{
				return string.Format("Quest{0}.Initiate", this.id);
			}
		}

		// Token: 0x1700181C RID: 6172
		// (get) Token: 0x06009CD4 RID: 40148 RVA: 0x002D7AAB File Offset: 0x002D5CAB
		public bool EverAccepted
		{
			get
			{
				return this.initiallyAccepted || this.acceptanceTick >= 0;
			}
		}

		// Token: 0x1700181D RID: 6173
		// (get) Token: 0x06009CD5 RID: 40149 RVA: 0x002D7AC3 File Offset: 0x002D5CC3
		public Pawn AccepterPawn
		{
			get
			{
				return this.accepterPawn;
			}
		}

		// Token: 0x1700181E RID: 6174
		// (get) Token: 0x06009CD6 RID: 40150 RVA: 0x002D7ACC File Offset: 0x002D5CCC
		public bool RequiresAccepter
		{
			get
			{
				for (int i = 0; i < this.parts.Count; i++)
				{
					if (this.parts[i].RequiresAccepter)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700181F RID: 6175
		// (get) Token: 0x06009CD7 RID: 40151 RVA: 0x002D7B08 File Offset: 0x002D5D08
		public QuestState State
		{
			get
			{
				if (this.TicksUntilExpiry == 0 && this.acceptanceTick < 0)
				{
					return QuestState.EndedOfferExpired;
				}
				if (this.ended)
				{
					if (this.endOutcome == QuestEndOutcome.Success)
					{
						return QuestState.EndedSuccess;
					}
					if (this.endOutcome == QuestEndOutcome.Fail)
					{
						return QuestState.EndedFailed;
					}
					if (this.endOutcome == QuestEndOutcome.InvalidPreAcceptance)
					{
						return QuestState.EndedInvalid;
					}
					return QuestState.EndedUnknownOutcome;
				}
				else
				{
					if (this.acceptanceTick < 0)
					{
						return QuestState.NotYetAccepted;
					}
					return QuestState.Ongoing;
				}
			}
		}

		// Token: 0x17001820 RID: 6176
		// (get) Token: 0x06009CD8 RID: 40152 RVA: 0x002D7B5F File Offset: 0x002D5D5F
		public IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				return this.parts.SelectMany((QuestPart x) => x.QuestLookTargets).Distinct<GlobalTargetInfo>();
			}
		}

		// Token: 0x17001821 RID: 6177
		// (get) Token: 0x06009CD9 RID: 40153 RVA: 0x002D7B90 File Offset: 0x002D5D90
		public IEnumerable<GlobalTargetInfo> QuestSelectTargets
		{
			get
			{
				return this.parts.SelectMany((QuestPart x) => x.QuestSelectTargets).Distinct<GlobalTargetInfo>();
			}
		}

		// Token: 0x17001822 RID: 6178
		// (get) Token: 0x06009CDA RID: 40154 RVA: 0x002D7BC1 File Offset: 0x002D5DC1
		public IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				return this.parts.SelectMany((QuestPart x) => x.InvolvedFactions).Distinct<Faction>();
			}
		}

		// Token: 0x17001823 RID: 6179
		// (get) Token: 0x06009CDB RID: 40155 RVA: 0x002D7BF2 File Offset: 0x002D5DF2
		public IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				if (this.parent != null)
				{
					yield return new Dialog_InfoCard.Hyperlink(this.parent, -1);
				}
				foreach (Quest quest in from q in this.GetSubquests(null)
				orderby q.Historical
				select q)
				{
					yield return new Dialog_InfoCard.Hyperlink(quest, -1);
				}
				IEnumerator<Quest> enumerator = null;
				foreach (Dialog_InfoCard.Hyperlink hyperlink in this.parts.SelectMany((QuestPart x) => x.Hyperlinks).Distinct<Dialog_InfoCard.Hyperlink>())
				{
					yield return hyperlink;
				}
				IEnumerator<Dialog_InfoCard.Hyperlink> enumerator2 = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17001824 RID: 6180
		// (get) Token: 0x06009CDC RID: 40156 RVA: 0x002D7C02 File Offset: 0x002D5E02
		public bool Historical
		{
			get
			{
				return this.State != QuestState.NotYetAccepted && this.State != QuestState.Ongoing;
			}
		}

		// Token: 0x17001825 RID: 6181
		// (get) Token: 0x06009CDD RID: 40157 RVA: 0x002D7C1C File Offset: 0x002D5E1C
		public bool IncreasesPopulation
		{
			get
			{
				for (int i = 0; i < this.parts.Count; i++)
				{
					if (this.parts[i].IncreasesPopulation)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06009CDE RID: 40158 RVA: 0x002D7C55 File Offset: 0x002D5E55
		public static Quest MakeRaw()
		{
			return new Quest
			{
				id = Find.UniqueIDsManager.GetNextQuestID(),
				appearanceTick = Find.TickManager.TicksGame,
				name = "Unnamed quest"
			};
		}

		// Token: 0x06009CDF RID: 40159 RVA: 0x002D7C88 File Offset: 0x002D5E88
		public void QuestTick()
		{
			if (this.Historical)
			{
				if (!this.cleanedUp)
				{
					this.CleanupQuestParts();
				}
				if (this.TicksSinceCleanup >= 1800000)
				{
					this.parts.Clear();
				}
				return;
			}
			if (this.TicksUntilExpiry == 0 && this.State == QuestState.NotYetAccepted && !this.cleanedUp)
			{
				this.CleanupQuestParts();
			}
			if (!this.Historical)
			{
				for (int i = 0; i < this.parts.Count; i++)
				{
					QuestPartActivable questPartActivable = this.parts[i] as QuestPartActivable;
					if (questPartActivable != null && questPartActivable.State == QuestPartState.Enabled)
					{
						try
						{
							questPartActivable.QuestPartTick();
						}
						catch (Exception arg)
						{
							Log.Error(string.Format("Exception ticking QuestPart: {0}", arg));
						}
						if (this.Historical)
						{
							break;
						}
					}
				}
			}
		}

		// Token: 0x06009CE0 RID: 40160 RVA: 0x002D7D54 File Offset: 0x002D5F54
		public void AddPart(QuestPart part)
		{
			if (this.parts.Contains(part))
			{
				Log.Error("Tried to add the same QuestPart twice: " + part.ToStringSafe<QuestPart>() + ", quest=" + this.ToStringSafe<Quest>());
				return;
			}
			part.quest = this;
			this.parts.Add(part);
		}

		// Token: 0x06009CE1 RID: 40161 RVA: 0x002D7DA4 File Offset: 0x002D5FA4
		public T AddPart<T>() where T : QuestPart
		{
			T t = Activator.CreateInstance<T>();
			this.AddPart(t);
			return t;
		}

		// Token: 0x06009CE2 RID: 40162 RVA: 0x002D7DC4 File Offset: 0x002D5FC4
		public T GetFirstOrAddPart<T>() where T : QuestPart
		{
			T result;
			if ((result = this.GetFirstPartOfType<T>()) == null)
			{
				result = this.AddPart<T>();
			}
			return result;
		}

		// Token: 0x06009CE3 RID: 40163 RVA: 0x002D7DDC File Offset: 0x002D5FDC
		public T GetFirstPartOfType<T>() where T : QuestPart
		{
			foreach (QuestPart questPart in this.parts)
			{
				T t = questPart as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06009CE4 RID: 40164 RVA: 0x002D7E4C File Offset: 0x002D604C
		public bool TryGetFirstPartOfType<T>(out T part) where T : QuestPart
		{
			part = this.GetFirstPartOfType<T>();
			return part != null;
		}

		// Token: 0x06009CE5 RID: 40165 RVA: 0x002D7E68 File Offset: 0x002D6068
		public void RemovePart(QuestPart part)
		{
			if (!this.parts.Contains(part))
			{
				Log.Error("Tried to remove QuestPart which doesn't exist: " + part.ToStringSafe<QuestPart>() + ", quest=" + this.ToStringSafe<Quest>());
				return;
			}
			part.quest = null;
			this.parts.Remove(part);
		}

		// Token: 0x06009CE6 RID: 40166 RVA: 0x002D7EB8 File Offset: 0x002D60B8
		public void Accept(Pawn by)
		{
			if (this.State != QuestState.NotYetAccepted)
			{
				return;
			}
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].PreQuestAccept();
			}
			this.acceptanceTick = Find.TickManager.TicksGame;
			this.accepterPawn = by;
			this.dismissed = false;
			this.Initiate();
		}

		// Token: 0x06009CE7 RID: 40167 RVA: 0x002D7F1C File Offset: 0x002D611C
		public void End(QuestEndOutcome outcome, bool sendLetter = true, bool playSound = true)
		{
			if (this.Historical)
			{
				Log.Error(string.Format("Tried to resolve a historical quest. id={0}", this.id));
				return;
			}
			this.ended = true;
			this.endOutcome = outcome;
			this.CleanupQuestParts();
			if (!this.EverAccepted && this.State == QuestState.EndedOfferExpired)
			{
				return;
			}
			if (sendLetter && !this.hidden)
			{
				string key = null;
				string key2 = null;
				LetterDef textLetterDef = null;
				switch (this.State)
				{
				case QuestState.EndedUnknownOutcome:
					key2 = "LetterQuestConcludedLabel";
					key = "LetterQuestCompletedConcluded";
					textLetterDef = LetterDefOf.NeutralEvent;
					if (playSound)
					{
						SoundDefOf.Quest_Concluded.PlayOneShotOnCamera(null);
					}
					break;
				case QuestState.EndedSuccess:
					key2 = "LetterQuestCompletedLabel";
					key = "LetterQuestCompletedSuccess";
					textLetterDef = LetterDefOf.PositiveEvent;
					if (playSound)
					{
						SoundDefOf.Quest_Succeded.PlayOneShotOnCamera(null);
					}
					break;
				case QuestState.EndedFailed:
					key2 = "LetterQuestFailedLabel";
					key = "LetterQuestCompletedFail";
					textLetterDef = LetterDefOf.NegativeEvent;
					if (playSound)
					{
						SoundDefOf.Quest_Failed.PlayOneShotOnCamera(null);
					}
					break;
				}
				Find.LetterStack.ReceiveLetter(key2.Translate(), key.Translate(this.name.CapitalizeFirst()), textLetterDef, null, null, this, null, null, 0, playSound);
			}
		}

		// Token: 0x06009CE8 RID: 40168 RVA: 0x002D803C File Offset: 0x002D623C
		public bool QuestReserves(Pawn p)
		{
			if (this.Historical)
			{
				return false;
			}
			int count = this.parts.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.parts[i].QuestPartReserves(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009CE9 RID: 40169 RVA: 0x002D8084 File Offset: 0x002D6284
		public bool QuestReserves(Faction f)
		{
			if (this.Historical)
			{
				return false;
			}
			for (int i = 0; i < this.parts.Count; i++)
			{
				if (this.parts[i].QuestPartReserves(f))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009CEA RID: 40170 RVA: 0x002D80C8 File Offset: 0x002D62C8
		public bool QuestReserves(TransportShip ship)
		{
			if (this.Historical)
			{
				return false;
			}
			for (int i = 0; i < this.parts.Count; i++)
			{
				if (this.parts[i].QuestPartReserves(ship))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009CEB RID: 40171 RVA: 0x002D810C File Offset: 0x002D630C
		public void SetInitiallyAccepted()
		{
			this.acceptanceTick = Find.TickManager.TicksGame;
			this.acceptanceExpireTick = -1;
			this.initiallyAccepted = true;
		}

		// Token: 0x06009CEC RID: 40172 RVA: 0x002D812C File Offset: 0x002D632C
		public void SetNotYetAccepted()
		{
			this.acceptanceTick = -1;
		}

		// Token: 0x06009CED RID: 40173 RVA: 0x002D8138 File Offset: 0x002D6338
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.appearanceTick, "appearanceTick", -1, false);
			Scribe_Values.Look<int>(ref this.acceptanceTick, "acceptanceTick", -1, false);
			Scribe_Values.Look<int>(ref this.acceptanceExpireTick, "acceptanceExpireTick", -1, false);
			Scribe_References.Look<Pawn>(ref this.accepterPawn, "acceptedBy", false);
			Scribe_Values.Look<string>(ref this.accepterPawnLabel, "acceptedByLabel", null, false);
			Scribe_Values.Look<bool>(ref this.ended, "ended", false, false);
			Scribe_Values.Look<QuestEndOutcome>(ref this.endOutcome, "endOutcome", QuestEndOutcome.Unknown, false);
			Scribe_Values.Look<bool>(ref this.cleanedUp, "cleanedUp", false, false);
			Scribe_Values.Look<int>(ref this.cleanupTick, "cleanupTick", -1, false);
			Scribe_Values.Look<bool>(ref this.initiallyAccepted, "initiallyAccepted", false, false);
			Scribe_Values.Look<bool>(ref this.dismissed, "dismissed", false, false);
			Scribe_Values.Look<bool>(ref this.hiddenInUI, "hiddenInUI", false, false);
			Scribe_Values.Look<int>(ref this.challengeRating, "challengeRating", 0, false);
			Scribe_Values.Look<TaggedString>(ref this.description, "description", default(TaggedString), false);
			Scribe_Values.Look<string>(ref this.lastSlateStateDebug, "lastSlateStateDebug", null, false);
			Scribe_Defs.Look<QuestScriptDef>(ref this.root, "root");
			Scribe_Values.Look<bool>(ref this.hidden, "hidden", false, false);
			Scribe_Collections.Look<string>(ref this.signalsReceivedDebug, "signalsReceivedDebug", LookMode.Undefined, Array.Empty<object>());
			Scribe_Collections.Look<QuestPart>(ref this.parts, "parts", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.tags, "tags", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.charity, "charity", false, false);
			Scribe_References.Look<Quest>(ref this.parent, "parent", false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.parts.RemoveAll((QuestPart x) => x == null) != 0)
				{
					Log.Error("Some quest parts were null after loading.");
				}
				for (int i = 0; i < this.parts.Count; i++)
				{
					this.parts[i].quest = this;
				}
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06009CEE RID: 40174 RVA: 0x002D8374 File Offset: 0x002D6574
		public void Notify_PawnDiscarded(Pawn pawn)
		{
			if (this.accepterPawn == pawn)
			{
				this.accepterPawn = null;
				this.accepterPawnLabel = pawn.LabelCap;
			}
			foreach (QuestPart questPart in this.PartsListForReading)
			{
				questPart.Notify_PawnDiscarded(pawn);
			}
		}

		// Token: 0x06009CEF RID: 40175 RVA: 0x002D83E4 File Offset: 0x002D65E4
		public void Notify_SignalReceived(Signal signal)
		{
			if (!signal.global && !signal.tag.StartsWith(string.Format("Quest{0}.", this.id)))
			{
				return;
			}
			for (int i = 0; i < this.parts.Count; i++)
			{
				try
				{
					bool flag;
					switch (this.parts[i].signalListenMode)
					{
					case QuestPart.SignalListenMode.OngoingOnly:
						flag = (this.State == QuestState.Ongoing);
						break;
					case QuestPart.SignalListenMode.NotYetAcceptedOnly:
						flag = (this.State == QuestState.NotYetAccepted);
						break;
					case QuestPart.SignalListenMode.OngoingOrNotYetAccepted:
						flag = (this.State == QuestState.Ongoing || this.State == QuestState.NotYetAccepted);
						break;
					case QuestPart.SignalListenMode.HistoricalOnly:
						flag = this.Historical;
						break;
					case QuestPart.SignalListenMode.Always:
						flag = true;
						break;
					default:
						flag = false;
						break;
					}
					if (flag)
					{
						this.parts[i].Notify_QuestSignalReceived(signal);
					}
				}
				catch (Exception arg)
				{
					Log.Error(string.Format("Error while processing a quest signal: {0}", arg));
				}
			}
		}

		// Token: 0x06009CF0 RID: 40176 RVA: 0x002D84E4 File Offset: 0x002D66E4
		public void PostAdded()
		{
			Find.SignalManager.SendSignal(new Signal(this.AddedSignal, false));
		}

		// Token: 0x06009CF1 RID: 40177 RVA: 0x002D84FC File Offset: 0x002D66FC
		public void Initiate()
		{
			Find.SignalManager.SendSignal(new Signal(this.InitiateSignal, false));
		}

		// Token: 0x06009CF2 RID: 40178 RVA: 0x002D8514 File Offset: 0x002D6714
		public void CleanupQuestParts()
		{
			if (this.cleanedUp)
			{
				return;
			}
			this.cleanupTick = Find.TickManager.TicksGame;
			for (int i = 0; i < this.parts.Count; i++)
			{
				try
				{
					this.parts[i].Notify_PreCleanup();
				}
				catch (Exception arg)
				{
					Log.Error(string.Format("Error in QuestPart Notify_PreCleanup: {0}", arg));
				}
			}
			for (int j = 0; j < this.parts.Count; j++)
			{
				try
				{
					this.parts[j].Cleanup();
				}
				catch (Exception arg2)
				{
					Log.Error(string.Format("Error in QuestPart cleanup: {0}", arg2));
				}
			}
			this.cleanedUp = true;
			Find.FactionManager.Notify_QuestCleanedUp(this);
			IdeoUtility.Notify_QuestCleanedUp(this, this.State);
			if (this.root.hideOnCleanup)
			{
				this.hiddenInUI = true;
			}
		}

		// Token: 0x06009CF3 RID: 40179 RVA: 0x002D8604 File Offset: 0x002D6804
		public void Notify_ThingsProduced(Pawn worker, List<Thing> things)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_ThingsProduced(worker, things);
			}
		}

		// Token: 0x06009CF4 RID: 40180 RVA: 0x002D863C File Offset: 0x002D683C
		public void Notify_PlantHarvested(Pawn worker, Thing harvested)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_PlantHarvested(worker, harvested);
			}
		}

		// Token: 0x06009CF5 RID: 40181 RVA: 0x002D8674 File Offset: 0x002D6874
		public void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_PawnKilled(pawn, dinfo);
			}
		}

		// Token: 0x06009CF6 RID: 40182 RVA: 0x002D86AC File Offset: 0x002D68AC
		public void Notify_PawnBorn(Thing baby, Thing birther, Pawn mother, Pawn father)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_PawnBorn(baby, birther, mother, father);
			}
		}

		// Token: 0x06009CF7 RID: 40183 RVA: 0x002D86E8 File Offset: 0x002D68E8
		public void Notify_FactionRemoved(Faction faction)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_FactionRemoved(faction);
			}
		}

		// Token: 0x06009CF8 RID: 40184 RVA: 0x002D871D File Offset: 0x002D691D
		public string GetUniqueLoadID()
		{
			return string.Format("Quest_{0}", this.id);
		}

		// Token: 0x04006CC2 RID: 27842
		public int id;

		// Token: 0x04006CC3 RID: 27843
		private List<QuestPart> parts = new List<QuestPart>();

		// Token: 0x04006CC4 RID: 27844
		public string name;

		// Token: 0x04006CC5 RID: 27845
		public TaggedString description;

		// Token: 0x04006CC6 RID: 27846
		public float points;

		// Token: 0x04006CC7 RID: 27847
		public int challengeRating = -1;

		// Token: 0x04006CC8 RID: 27848
		public List<string> tags = new List<string>();

		// Token: 0x04006CC9 RID: 27849
		public string lastSlateStateDebug;

		// Token: 0x04006CCA RID: 27850
		public QuestScriptDef root;

		// Token: 0x04006CCB RID: 27851
		public bool hidden;

		// Token: 0x04006CCC RID: 27852
		public Quest parent;

		// Token: 0x04006CCD RID: 27853
		public int appearanceTick = -1;

		// Token: 0x04006CCE RID: 27854
		public int acceptanceTick = -1;

		// Token: 0x04006CCF RID: 27855
		public int acceptanceExpireTick = -1;

		// Token: 0x04006CD0 RID: 27856
		public bool initiallyAccepted;

		// Token: 0x04006CD1 RID: 27857
		public bool dismissed;

		// Token: 0x04006CD2 RID: 27858
		public bool hiddenInUI;

		// Token: 0x04006CD3 RID: 27859
		private Pawn accepterPawn;

		// Token: 0x04006CD4 RID: 27860
		private string accepterPawnLabel;

		// Token: 0x04006CD5 RID: 27861
		public bool charity;

		// Token: 0x04006CD6 RID: 27862
		public bool canGenerateInSpace;

		// Token: 0x04006CD7 RID: 27863
		public List<string> signalsReceivedDebug;

		// Token: 0x04006CD8 RID: 27864
		private bool ended;

		// Token: 0x04006CD9 RID: 27865
		private QuestEndOutcome endOutcome;

		// Token: 0x04006CDA RID: 27866
		private bool cleanedUp;

		// Token: 0x04006CDB RID: 27867
		public int cleanupTick = -1;

		// Token: 0x04006CDC RID: 27868
		public const int MaxSignalsReceivedDebugCount = 25;

		// Token: 0x04006CDD RID: 27869
		private const int RemoveAllQuestPartsAfterTicksSinceCleanup = 1800000;
	}
}
