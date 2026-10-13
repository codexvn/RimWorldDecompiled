using System;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F54 RID: 3924
	public static class JobUtility
	{
		// Token: 0x0600611F RID: 24863 RVA: 0x001EBF44 File Offset: 0x001EA144
		public static void TryStartErrorRecoverJob(Pawn pawn, string message, Exception exception = null, JobDriver concreteDriver = null)
		{
			string text = message;
			JobUtility.AppendVarsInfoToDebugMessage(pawn, ref text, concreteDriver);
			if (exception != null)
			{
				text = text + "\n" + ((exception != null) ? exception.ToString() : null);
			}
			Log.Error(text);
			if (pawn.jobs != null)
			{
				if (pawn.jobs.curJob != null)
				{
					pawn.jobs.EndCurrentJob(JobCondition.Errored, false, true);
				}
				if (JobUtility.startingErrorRecoverJob)
				{
					Log.Error("An error occurred while starting an error recover job. We have to stop now to avoid infinite loops. This means that the pawn is now jobless which can cause further bugs. pawn=" + pawn.ToStringSafe<Pawn>());
					return;
				}
				JobUtility.startingErrorRecoverJob = true;
				try
				{
					pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Wait, 150, false), JobCondition.None, null, false, true, null, null, false, false, null, false, true, false);
				}
				finally
				{
					JobUtility.startingErrorRecoverJob = false;
				}
			}
		}

		// Token: 0x06006120 RID: 24864 RVA: 0x001EC018 File Offset: 0x001EA218
		public static string GetResolvedJobReport(string baseText, LocalTargetInfo a)
		{
			return JobUtility.GetResolvedJobReport(baseText, a, LocalTargetInfo.Invalid, LocalTargetInfo.Invalid);
		}

		// Token: 0x06006121 RID: 24865 RVA: 0x001EC02B File Offset: 0x001EA22B
		public static string GetResolvedJobReport(string baseText, LocalTargetInfo a, LocalTargetInfo b)
		{
			return JobUtility.GetResolvedJobReport(baseText, a, b, LocalTargetInfo.Invalid);
		}

		// Token: 0x06006122 RID: 24866 RVA: 0x001EC03C File Offset: 0x001EA23C
		public static string GetResolvedJobReport(string baseText, LocalTargetInfo a, LocalTargetInfo b, LocalTargetInfo c)
		{
			string aText;
			object aObj;
			JobUtility.<GetResolvedJobReport>g__GetText|4_0(a, out aText, out aObj);
			string bText;
			object bObj;
			JobUtility.<GetResolvedJobReport>g__GetText|4_0(b, out bText, out bObj);
			string cText;
			object cObj;
			JobUtility.<GetResolvedJobReport>g__GetText|4_0(c, out cText, out cObj);
			return JobUtility.GetResolvedJobReportRaw(baseText, aText, aObj, bText, bObj, cText, cObj);
		}

		// Token: 0x06006123 RID: 24867 RVA: 0x001EC078 File Offset: 0x001EA278
		public static string GetResolvedJobReportRaw(string baseText, string aText, object aObj, string bText, object bObj, string cText, object cObj)
		{
			baseText = baseText.Formatted(aObj.Named("TargetA"), bObj.Named("TargetB"), cObj.Named("TargetC"));
			baseText = baseText.Replace("TargetA", aText);
			baseText = baseText.Replace("TargetB", bText);
			baseText = baseText.Replace("TargetC", cText);
			return baseText;
		}

		// Token: 0x06006124 RID: 24868 RVA: 0x001EC0E4 File Offset: 0x001EA2E4
		private static void AppendVarsInfoToDebugMessage(Pawn pawn, ref string msg, JobDriver concreteDriver)
		{
			if (concreteDriver != null)
			{
				msg = string.Concat(new string[]
				{
					msg,
					" driver=",
					concreteDriver.GetType().Name,
					" (toilIndex=",
					concreteDriver.CurToilIndex.ToString(),
					")"
				});
				if (concreteDriver.job != null)
				{
					msg = msg + " driver.job=(" + concreteDriver.job.ToStringSafe<Job>() + ")";
					return;
				}
			}
			else if (pawn.jobs != null)
			{
				if (pawn.jobs.curDriver != null)
				{
					msg = string.Concat(new string[]
					{
						msg,
						" curDriver=",
						pawn.jobs.curDriver.GetType().Name,
						" (toilIndex=",
						pawn.jobs.curDriver.CurToilIndex.ToString(),
						")"
					});
				}
				if (pawn.jobs.curJob != null)
				{
					msg = msg + " curJob=(" + pawn.jobs.curJob.ToStringSafe<Job>() + ")";
				}
			}
		}

		// Token: 0x06006125 RID: 24869 RVA: 0x001EC20C File Offset: 0x001EA40C
		[CompilerGenerated]
		internal static void <GetResolvedJobReport>g__GetText|4_0(LocalTargetInfo x, out string backCompatibleText, out object obj)
		{
			if (!x.IsValid)
			{
				backCompatibleText = "UnknownLower".Translate();
				obj = backCompatibleText;
				return;
			}
			if (x.HasThing && !x.ThingDiscarded)
			{
				backCompatibleText = x.Thing.LabelShort;
				obj = x.Thing;
				return;
			}
			backCompatibleText = "AreaLower".Translate();
			obj = backCompatibleText;
		}

		// Token: 0x040045F7 RID: 17911
		private static bool startingErrorRecoverJob;
	}
}
