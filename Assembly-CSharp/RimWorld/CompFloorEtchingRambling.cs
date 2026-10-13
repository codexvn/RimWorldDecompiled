using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02002CA0 RID: 11424
	public class CompFloorEtchingRambling : CompInteractable
	{
		// Token: 0x06010088 RID: 65672 RVA: 0x004A4804 File Offset: 0x004A2A04
		protected override void OnInteracted(Pawn caster)
		{
			this.deciphered = true;
			this.GenerateMessage();
			TaggedString label = "LetterLabelFloorEtchingRamblings".Translate();
			TaggedString taggedString = "LetterFloorEtchingsRamblings".Translate(caster.Named("PAWN"));
			taggedString += string.Format("\n\n{0}", this.message);
			Find.LetterStack.ReceiveLetter(label, taggedString, LetterDefOf.NeutralEvent, this.parent, null, null, null, null, 0, true);
		}

		// Token: 0x06010089 RID: 65673 RVA: 0x004A4880 File Offset: 0x004A2A80
		protected void GenerateMessage()
		{
			this.deciphered = true;
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(RulePackDefOf.LabyrinthRamblings);
			foreach (Thing thing in this.parent.Map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse))
			{
				Corpse corpse = thing as Corpse;
				if (corpse != null)
				{
					Pawn innerPawn = corpse.InnerPawn;
					if (innerPawn.RaceProps.Humanlike && !innerPawn.IsColonist && corpse.timeOfDeath < this.parent.TickSpawned)
					{
						CompFloorEtchingRambling.PossibleCorpses.Add(innerPawn);
					}
				}
			}
			TaleData_Pawn taleData_Pawn;
			if (CompFloorEtchingRambling.PossibleCorpses.Any<Pawn>())
			{
				taleData_Pawn = TaleData_Pawn.GenerateFrom(CompFloorEtchingRambling.PossibleCorpses.RandomElement<Pawn>());
			}
			else
			{
				taleData_Pawn = TaleData_Pawn.GenerateRandom(true);
			}
			foreach (Rule item in taleData_Pawn.GetRules("PAWN", request.Constants))
			{
				request.Rules.Add(item);
			}
			this.message = GrammarResolver.Resolve("r_root", request, null, false, null, null, null, true);
			CompFloorEtchingRambling.PossibleCorpses.Clear();
		}

		// Token: 0x0601008A RID: 65674 RVA: 0x004A49E4 File Offset: 0x004A2BE4
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.deciphered)
			{
				stringBuilder.AppendLineIfNotEmpty();
				stringBuilder.Append(this.message);
			}
			else
			{
				stringBuilder.Append(base.CompInspectStringExtra());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0601008B RID: 65675 RVA: 0x004A4A2D File Offset: 0x004A2C2D
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!this.deciphered)
			{
				foreach (Gizmo gizmo in base.CompGetGizmosExtra())
				{
					yield return gizmo;
				}
				IEnumerator<Gizmo> enumerator = null;
			}
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Regen text",
					action = new Action(this.GenerateMessage)
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0601008C RID: 65676 RVA: 0x004A4A3D File Offset: 0x004A2C3D
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			if (this.deciphered)
			{
				yield break;
			}
			foreach (FloatMenuOption floatMenuOption in base.CompFloatMenuOptions(selPawn))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0601008D RID: 65677 RVA: 0x004A4A54 File Offset: 0x004A2C54
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.deciphered, "deciphered", false, false);
			Scribe_Values.Look<TaggedString>(ref this.message, "message", default(TaggedString), false);
		}

		// Token: 0x0400A7A8 RID: 42920
		public bool deciphered;

		// Token: 0x0400A7A9 RID: 42921
		public TaggedString message;

		// Token: 0x0400A7AA RID: 42922
		private static readonly List<Pawn> PossibleCorpses = new List<Pawn>();
	}
}
