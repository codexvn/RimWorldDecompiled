using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002BAB RID: 11179
	public abstract class CompAnalyzable : CompInteractable
	{
		// Token: 0x17002726 RID: 10022
		// (get) Token: 0x0600F9EB RID: 63979 RVA: 0x0048AE3B File Offset: 0x0048903B
		public new CompProperties_Analyzable Props
		{
			get
			{
				return (CompProperties_Analyzable)this.props;
			}
		}

		// Token: 0x17002727 RID: 10023
		// (get) Token: 0x0600F9EC RID: 63980 RVA: 0x0048AE48 File Offset: 0x00489048
		public Pawn Pawn
		{
			get
			{
				return this.parent as Pawn;
			}
		}

		// Token: 0x17002728 RID: 10024
		// (get) Token: 0x0600F9ED RID: 63981
		public abstract int AnalysisID { get; }

		// Token: 0x17002729 RID: 10025
		// (get) Token: 0x0600F9EE RID: 63982 RVA: 0x0048AE55 File Offset: 0x00489055
		public override string ExposeKey
		{
			get
			{
				return "Analyzable";
			}
		}

		// Token: 0x0600F9EF RID: 63983 RVA: 0x0048AE5C File Offset: 0x0048905C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && !Find.AnalysisManager.HasAnalysisWithID(this.AnalysisID))
			{
				Find.AnalysisManager.AddAnalysisTask(this.AnalysisID, this.Props.analysisRequiredRange.RandomInRange);
			}
		}

		// Token: 0x0600F9F0 RID: 63984 RVA: 0x0048AE9C File Offset: 0x0048909C
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			Building_ResearchBench building_ResearchBench = null;
			if (base.ValidateTarget(target, false) && (this.Props.canStudyInPlace || StudyUtility.TryFindResearchBench(target.Pawn, out building_ResearchBench)))
			{
				ThingWithComps parent = this.parent;
				CompForbiddable compForbiddable = (parent != null) ? parent.TryGetComp<CompForbiddable>() : null;
				if (compForbiddable != null)
				{
					compForbiddable.Forbidden = false;
				}
				target.Pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.AnalyzeItem, this.parent, building_ResearchBench, (building_ResearchBench != null) ? building_ResearchBench.Position : IntVec3.Invalid), new JobTag?(JobTag.Misc), false);
			}
		}

		// Token: 0x0600F9F1 RID: 63985 RVA: 0x0048AF3C File Offset: 0x0048913C
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			AcceptanceReport result = base.CanInteract(activateBy, checkOptionalItems);
			if (!result.Accepted)
			{
				return result;
			}
			AnalysisDetails analysisDetails;
			Find.AnalysisManager.TryGetAnalysisProgress(this.AnalysisID, out analysisDetails);
			if (!this.Props.allowRepeatAnalysis && analysisDetails != null && analysisDetails.Satisfied)
			{
				return "AlreadyAnalyzed".Translate();
			}
			if (activateBy != null && StatDefOf.ResearchSpeed.Worker.IsDisabledFor(activateBy))
			{
				return "Incapable".Translate();
			}
			Building_ResearchBench building_ResearchBench;
			if (activateBy != null && !this.Props.canStudyInPlace && !StudyUtility.TryFindResearchBench(activateBy, out building_ResearchBench))
			{
				return "NoResearchBench".Translate();
			}
			if (activateBy == null && !this.Props.canStudyInPlace && !this.parent.MapHeld.listerBuildings.ColonistsHaveResearchBench())
			{
				return "NoResearchBench".Translate();
			}
			return true;
		}

		// Token: 0x0600F9F2 RID: 63986 RVA: 0x0048B024 File Offset: 0x00489224
		public virtual void OnAnalyzed(Pawn pawn)
		{
			AnalysisDetails details;
			if (!Find.AnalysisManager.TryIncrementAnalysisProgress(this.AnalysisID, out details) && Find.AnalysisManager.IsAnalysisComplete(this.AnalysisID))
			{
				this.SendLetter(this.Props.repeatCompletedLetterLabel, this.Props.repeatCompletedLetter, this.Props.repeatCompletedLetterDef, pawn);
			}
			else
			{
				this.SendAppropriateProgressLetter(pawn, details);
			}
			if (this.Props.destroyedOnAnalyzed)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600F9F3 RID: 63987 RVA: 0x0048B0A4 File Offset: 0x004892A4
		private void SendAppropriateProgressLetter(Pawn pawn, AnalysisDetails details)
		{
			if (details.Satisfied)
			{
				this.SendLetter(this.Props.completedLetterLabel, this.Props.completedLetter, this.Props.completedLetterDef, pawn);
				return;
			}
			string str = "";
			if (this.Props.showProgress)
			{
				str = string.Format(" {0}/{1}", details.timesDone, details.required);
			}
			string letter = (details.timesDone <= this.Props.progressedLetters.Count) ? this.Props.progressedLetters[details.timesDone - 1] : this.Props.progressedLetters.Last<string>();
			this.SendLetter(this.Props.progressedLetterLabel + str, letter, this.Props.progressedLetterDef, pawn);
		}

		// Token: 0x0600F9F4 RID: 63988 RVA: 0x0048B180 File Offset: 0x00489380
		private void SendLetter(string label, string letter, LetterDef def, Pawn pawn)
		{
			if (string.IsNullOrEmpty(label) || string.IsNullOrEmpty(letter))
			{
				return;
			}
			string formattedLetterString = this.GetFormattedLetterString(label, pawn);
			string formattedLetterString2 = this.GetFormattedLetterString(letter, pawn);
			Find.LetterStack.ReceiveLetter(formattedLetterString, formattedLetterString2, def, null, 0, true);
		}

		// Token: 0x0600F9F5 RID: 63989 RVA: 0x0048B1CC File Offset: 0x004893CC
		private string GetFormattedLetterString(string text, Pawn pawn)
		{
			if (this.ExtraNamedArg != null)
			{
				return text.Formatted(pawn.Named("PAWN"), this.ExtraNamedArg.Value).Resolve();
			}
			return text.Formatted(pawn.Named("PAWN")).Resolve();
		}

		// Token: 0x0600F9F6 RID: 63990 RVA: 0x0048B22A File Offset: 0x0048942A
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Finish analysis",
					action = delegate()
					{
						this.OnAnalyzed(Find.CurrentMap.mapPawns.FreeColonistsSpawned.First<Pawn>());
					}
				};
			}
			yield break;
			yield break;
		}
	}
}
