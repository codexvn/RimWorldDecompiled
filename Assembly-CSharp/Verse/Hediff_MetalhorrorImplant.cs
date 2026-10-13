using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000755 RID: 1877
	public class Hediff_MetalhorrorImplant : HediffWithComps
	{
		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06002DE2 RID: 11746 RVA: 0x000EC076 File Offset: 0x000EA276
		// (set) Token: 0x06002DE3 RID: 11747 RVA: 0x000EC07E File Offset: 0x000EA27E
		public ImplantSource ImplantSource
		{
			get
			{
				return this.implantSource;
			}
			set
			{
				this.implantSource = value;
			}
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06002DE4 RID: 11748 RVA: 0x000EC087 File Offset: 0x000EA287
		public int Biosignature
		{
			get
			{
				return this.implantSource.Biosignature;
			}
		}

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06002DE5 RID: 11749 RVA: 0x000EC094 File Offset: 0x000EA294
		public string BiosignatureName
		{
			get
			{
				return this.implantSource.BiosignatureName;
			}
		}

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06002DE6 RID: 11750 RVA: 0x000EC0A1 File Offset: 0x000EA2A1
		public bool Emerging
		{
			get
			{
				return this.emerging;
			}
		}

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x06002DE7 RID: 11751 RVA: 0x000EC0A9 File Offset: 0x000EA2A9
		public bool KnowsDetected
		{
			get
			{
				return this.knowsDetected;
			}
		}

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x06002DE8 RID: 11752 RVA: 0x000EC0B1 File Offset: 0x000EA2B1
		// (set) Token: 0x06002DE9 RID: 11753 RVA: 0x000EC0B9 File Offset: 0x000EA2B9
		public bool LiedAboutInspection
		{
			get
			{
				return this.liedAboutInspection;
			}
			set
			{
				this.liedAboutInspection = (value || this.liedAboutInspection);
			}
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x000EC0D0 File Offset: 0x000EA2D0
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Metalhorror implant"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
			this.filthTimer = Mathf.CeilToInt(Hediff_MetalhorrorImplant.InitialFilthDelayDays.RandomInRange * 60000f);
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x000EC120 File Offset: 0x000EA320
		public override void PostTickInterval(int delta)
		{
			base.PostTickInterval(delta);
			if (!this.pawn.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			this.DoPassiveTransmission();
			this.FilthTick();
			if (this.Emerging && GenTicks.TicksGame >= this.emergeAt)
			{
				MetalhorrorUtility.SpawnMetalhorror(this.pawn, this);
			}
			if (this.knowsDetected)
			{
				this.Emerge("", false);
				return;
			}
			if (MetalhorrorUtility.ShouldRandomEmerge(this.pawn, delta))
			{
				this.Emerge("MetalhorrorReasonRandom".Translate(this.pawn.Named("INFECTED")), false);
			}
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x000EC1B9 File Offset: 0x000EA3B9
		public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			this.Emerge("MetalhorrorReasonHostDied".Translate(this.pawn.Named("INFECTED")), false);
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x000EC1E4 File Offset: 0x000EA3E4
		public void Emerge(string reason, bool isSympathetic = false)
		{
			if (this.emerging || !this.pawn.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			Find.Anomaly.emergedBiosignatures.Add(this.Biosignature);
			base.SetVisible();
			this.emerging = true;
			this.knowsDetected = true;
			this.sympatheticEmergence = (isSympathetic || this.sympatheticEmergence);
			if (string.IsNullOrEmpty(this.emergeReason))
			{
				this.emergeReason = reason;
			}
			if (this.sympatheticEmergence)
			{
				this.emergeAt = GenTicks.TicksGame + Hediff_MetalhorrorImplant.TicksToEmergeSympathetic.RandomInRange;
			}
			else
			{
				this.emergeAt = GenTicks.TicksGame + Hediff_MetalhorrorImplant.TicksToEmerge.RandomInRange;
			}
			if (!this.pawn.DeadOrDowned)
			{
				HealthUtility.DamageUntilDowned(this.pawn, true, DamageDefOf.Cut, ThingDefOf.Metalhorror, BodyPartGroupDefOf.LeftBlade);
			}
			else if (!this.pawn.Dead)
			{
				DamageWorker.DamageResult damageResult = this.pawn.TakeDamage(new DamageInfo(DamageDefOf.Cut, 20f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
				if (damageResult.hediffs != null)
				{
					foreach (Hediff hediff in damageResult.hediffs)
					{
						hediff.sourceDef = ThingDefOf.Metalhorror;
						hediff.sourceBodyPartGroup = BodyPartGroupDefOf.LeftBlade;
					}
				}
			}
			if (!this.sympatheticEmergence)
			{
				Hediff_MetalhorrorImplant.TmpEmerging.Clear();
				foreach (Pawn target in this.pawn.MapHeld.mapPawns.AllHumanlike)
				{
					this.TryTriggerSympatheticEmergence(target, Hediff_MetalhorrorImplant.TmpEmerging);
				}
				foreach (Thing thing in this.pawn.MapHeld.listerThings.ThingsInGroup(ThingRequestGroup.Corpse))
				{
					Corpse corpse = thing as Corpse;
					if (corpse != null && corpse.InnerPawn != null)
					{
						this.TryTriggerSympatheticEmergence(corpse.InnerPawn, Hediff_MetalhorrorImplant.TmpEmerging);
					}
				}
				Hediff_MetalhorrorImplant.TmpEmerging.SortByDescending(([TupleElementNames(new string[]
				{
					"hediff",
					"message"
				})] ValueTuple<Hediff_MetalhorrorImplant, string> tuple) => tuple.Item1.ageTicks);
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = new StringBuilder();
				foreach (ValueTuple<Hediff_MetalhorrorImplant, string> valueTuple in Hediff_MetalhorrorImplant.TmpEmerging)
				{
					Hediff_MetalhorrorImplant item = valueTuple.Item1;
					string item2 = valueTuple.Item2;
					stringBuilder.AppendLine(item2);
					if (item.LiedAboutInspection)
					{
						if (stringBuilder2.Length == 0)
						{
							stringBuilder2.AppendLine(string.Format("{0}:\n", "MetalhorrorSurgicalInspectionDetails".Translate()));
						}
						stringBuilder2.AppendLine(string.Format(" - {0}", item.pawn.NameShortColored));
					}
				}
				TaggedString taggedString;
				if (Hediff_MetalhorrorImplant.TmpEmerging.Count == 1)
				{
					taggedString = "MetalhorrorEmergingDesc".Translate();
				}
				else
				{
					taggedString = "MetalhorrorEmergingPluralDesc".Translate();
				}
				TaggedString t = taggedString;
				string str = "\n\n";
				StringBuilder stringBuilder3 = stringBuilder;
				taggedString = t + (str + ((stringBuilder3 != null) ? stringBuilder3.ToString() : null));
				if (stringBuilder2.Length > 0)
				{
					TaggedString t2 = taggedString;
					string str2 = "\n";
					StringBuilder stringBuilder4 = stringBuilder2;
					taggedString = t2 + (str2 + ((stringBuilder4 != null) ? stringBuilder4.ToString() : null));
				}
				taggedString += "\n" + this.emergeReason;
				taggedString += "\n\n" + "MetalhorrorEmergingDescAppended".Translate();
				Find.LetterStack.ReceiveLetter("MetalhorrorEmergingLabel".Translate(), taggedString.Resolve(), LetterDefOf.ThreatBig, (from hd in Hediff_MetalhorrorImplant.TmpEmerging
				select hd.Item1.pawn).ToList<Pawn>(), null, null, null, null, 0, true);
				Find.AnalysisManager.RemoveAnalysisDetails(this.Biosignature);
				Hediff_MetalhorrorImplant.TmpEmerging.Clear();
			}
			this.DoEmergingEffects();
			if (this.pawn.Dead)
			{
				MetalhorrorUtility.SpawnMetalhorror(this.pawn, this);
			}
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x000EC654 File Offset: 0x000EA854
		private void TryTriggerSympatheticEmergence(Pawn target, [TupleElementNames(new string[]
		{
			"hediff",
			"message"
		})] List<ValueTuple<Hediff_MetalhorrorImplant, string>> list)
		{
			Hediff_MetalhorrorImplant firstHediff = target.health.hediffSet.GetFirstHediff<Hediff_MetalhorrorImplant>();
			if (firstHediff == null || firstHediff.Biosignature != this.Biosignature)
			{
				return;
			}
			if (target != this.pawn)
			{
				MetalhorrorUtility.TryEmerge(target, null, true);
			}
			string item = string.Format("  - {0} ({1})", target.NameShortColored, firstHediff.ImplantSource.GetSourceDesc());
			list.Add(new ValueTuple<Hediff_MetalhorrorImplant, string>(firstHediff, item));
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x000EC6C4 File Offset: 0x000EA8C4
		private void DoEmergingEffects()
		{
			EffecterDefOf.MetalhorrorEmerging.Spawn(this.pawn.PositionHeld, this.pawn.MapHeld, 1f).Cleanup();
			IntVec3 positionHeld = this.pawn.PositionHeld;
			Map mapHeld = this.pawn.MapHeld;
			CellRect cellRect = new CellRect(positionHeld.x, positionHeld.z, 3, 3);
			for (int i = 0; i < 5; i++)
			{
				IntVec3 randomCell = cellRect.RandomCell;
				if (randomCell.InBounds(mapHeld) && GenSight.LineOfSight(randomCell, positionHeld, mapHeld))
				{
					FilthMaker.TryMakeFilth(randomCell, mapHeld, (i % 2 == 0) ? ThingDefOf.Filth_Blood : ThingDefOf.Filth_GrayFlesh, 1, FilthSourceFlags.None, true);
				}
			}
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x000EC76F File Offset: 0x000EA96F
		public void Detect(string reason, bool noticed)
		{
			base.SetVisible();
			this.knowsDetected = noticed;
			if (this.knowsDetected)
			{
				this.Emerge(reason, false);
			}
		}

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06002DF1 RID: 11761 RVA: 0x000EC78E File Offset: 0x000EA98E
		public override string TipStringExtra
		{
			get
			{
				return this.implantSource.GetSourceDesc().CapitalizeFirst();
			}
		}

		// Token: 0x06002DF2 RID: 11762 RVA: 0x000EC7A0 File Offset: 0x000EA9A0
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (!DebugSettings.godMode || this.emerging)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Emerge",
				action = delegate()
				{
					this.Emerge("MetalhorrorReasonRandom".Translate(this.pawn.Named("INFECTED")), false);
				}
			};
			if (!this.filthPos.IsValid)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Mark for flesh drop",
					action = delegate()
					{
						this.filthTimer = 0;
					}
				};
			}
			if (!this.debugDiscoverNextInteraction)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Discover next interaction",
					action = delegate()
					{
						this.debugDiscoverNextInteraction = true;
					}
				};
			}
			int hours = this.ageTicks / 2500;
			if (hours < 72)
			{
				string str = "Larva";
				if (hours > 24)
				{
					str = "Juvenile";
				}
				yield return new Command_Action
				{
					defaultLabel = "DEV: Inc Lifestage (" + str + ")",
					action = delegate()
					{
						if (hours < 24)
						{
							this.ageTicks = 62500;
							return;
						}
						this.ageTicks = 182500;
					}
				};
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Change biosignature",
				action = delegate()
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (Pawn pawn in this.pawn.MapHeld.mapPawns.AllHumanlike)
					{
						Hediff_MetalhorrorImplant implant;
						if (MetalhorrorUtility.IsInfected(pawn, out implant) && !list.ContainsAny((FloatMenuOption x) => x.Label == implant.BiosignatureName))
						{
							list.Add(new FloatMenuOption(implant.BiosignatureName, delegate()
							{
								this.implantSource.DebugSetBiosignature(implant.Biosignature);
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
			};
			yield break;
			yield break;
		}

		// Token: 0x06002DF3 RID: 11763 RVA: 0x000EC7B0 File Offset: 0x000EA9B0
		private void DoPassiveTransmission()
		{
			if (this.pawn.InBed())
			{
				foreach (Pawn pawn in this.pawn.CurrentBed().CurOccupants)
				{
					if (pawn != this.pawn)
					{
						MetalhorrorUtility.Infect(pawn, this.pawn, "SleepImplant", null);
					}
				}
			}
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x000EC828 File Offset: 0x000EAA28
		private void FilthTick()
		{
			this.UpdateFilthTimer();
			if (this.filthTimer <= 0)
			{
				if (!FilthMaker.CanMakeFilth(this.pawn.PositionHeld, this.pawn.MapHeld, ThingDefOf.Filth_GrayFleshNoticeable, FilthSourceFlags.None))
				{
					this.filthTimer = 600;
				}
				else
				{
					this.filthTimer = Mathf.CeilToInt(Hediff_MetalhorrorImplant.FilthDelayDays.RandomInRange * 60000f);
					this.spawnFilthAt = GenTicks.TicksGame + Mathf.CeilToInt(Hediff_MetalhorrorImplant.SpawnFilthDelayHours.RandomInRange * 2500f);
					this.filthMap = this.pawn.MapHeld;
					this.filthPos = this.pawn.PositionHeld;
				}
			}
			if (GenTicks.TicksGame >= this.spawnFilthAt && this.filthPos != IntVec3.Invalid && this.filthMap != null)
			{
				Filth filth;
				if (FilthMaker.TryMakeFilth(this.filthPos, this.filthMap, ThingDefOf.Filth_GrayFleshNoticeable, out filth, 1, FilthSourceFlags.None, true))
				{
					((FilthGrayFleshNoticeable)filth).biosignature = this.Biosignature;
				}
				this.filthPos = IntVec3.Invalid;
				this.filthMap = null;
			}
		}

		// Token: 0x06002DF5 RID: 11765 RVA: 0x000EC944 File Offset: 0x000EAB44
		public override string GetInspectString()
		{
			if (Prefs.DevMode && DebugSettings.godMode)
			{
				return string.Format("DEV: Bio: {0}, mark filth: {1:0}s ", this.BiosignatureName, this.filthTimer.TicksToSeconds()) + ((this.spawnFilthAt < GenTicks.TicksGame) ? "" : string.Format(" (spawns: {0:0}s)", (this.spawnFilthAt - GenTicks.TicksGame).TicksToSeconds()));
			}
			return base.GetInspectString();
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x000EC9BF File Offset: 0x000EABBF
		private void UpdateFilthTimer()
		{
			if (this.pawn.InBed() || this.pawn.GetPosture().Laying() || this.pawn.Downed)
			{
				return;
			}
			this.filthTimer--;
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x000EC9FC File Offset: 0x000EABFC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.emerging, "emerging", false, false);
			Scribe_Values.Look<int>(ref this.emergeAt, "emergeAt", 0, false);
			Scribe_Values.Look<int>(ref this.filthTimer, "filthTimer", 0, false);
			Scribe_Values.Look<bool>(ref this.knowsDetected, "knowsDetected", false, false);
			Scribe_Values.Look<bool>(ref this.sympatheticEmergence, "sympatheticEmergence", false, false);
			Scribe_Values.Look<bool>(ref this.sentMaturingMessage, "sentMaturingMessage", false, false);
			Scribe_Values.Look<int>(ref this.spawnFilthAt, "spawnFilthAt", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.filthPos, "filthPos", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.liedAboutInspection, "liedAboutInspection", false, false);
			Scribe_References.Look<Map>(ref this.filthMap, "filthMap", false);
			Scribe_Deep.Look<ImplantSource>(ref this.implantSource, "implantSource", Array.Empty<object>());
		}

		// Token: 0x0400242A RID: 9258
		private ImplantSource implantSource = new ImplantSource();

		// Token: 0x0400242B RID: 9259
		private bool emerging;

		// Token: 0x0400242C RID: 9260
		private bool knowsDetected;

		// Token: 0x0400242D RID: 9261
		private bool sympatheticEmergence;

		// Token: 0x0400242E RID: 9262
		private bool sentMaturingMessage;

		// Token: 0x0400242F RID: 9263
		private string emergeReason;

		// Token: 0x04002430 RID: 9264
		private int emergeAt;

		// Token: 0x04002431 RID: 9265
		private bool liedAboutInspection;

		// Token: 0x04002432 RID: 9266
		private int filthTimer;

		// Token: 0x04002433 RID: 9267
		private int spawnFilthAt;

		// Token: 0x04002434 RID: 9268
		private IntVec3 filthPos = IntVec3.Invalid;

		// Token: 0x04002435 RID: 9269
		private Map filthMap;

		// Token: 0x04002436 RID: 9270
		public bool debugDiscoverNextInteraction;

		// Token: 0x04002437 RID: 9271
		private static readonly IntRange TicksToEmerge = new IntRange(60, 180);

		// Token: 0x04002438 RID: 9272
		private static readonly IntRange TicksToEmergeSympathetic = new IntRange(60, 600);

		// Token: 0x04002439 RID: 9273
		private static readonly FloatRange InitialFilthDelayDays = new FloatRange(1.5f, 2.5f);

		// Token: 0x0400243A RID: 9274
		private static readonly FloatRange FilthDelayDays = new FloatRange(3f, 10f);

		// Token: 0x0400243B RID: 9275
		private static readonly FloatRange SpawnFilthDelayHours = new FloatRange(1f, 12f);

		// Token: 0x0400243C RID: 9276
		[TupleElementNames(new string[]
		{
			"hediff",
			"message"
		})]
		private static readonly List<ValueTuple<Hediff_MetalhorrorImplant, string>> TmpEmerging = new List<ValueTuple<Hediff_MetalhorrorImplant, string>>();
	}
}
