using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C9F RID: 11423
	public class CompFloorEtching : CompFloorEtchingRambling
	{
		// Token: 0x06010082 RID: 65666 RVA: 0x004A45E8 File Offset: 0x004A27E8
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (respawningAfterLoad)
			{
				return;
			}
			LabyrinthMapComponent component = this.parent.Map.GetComponent<LabyrinthMapComponent>();
			if (component == null)
			{
				return;
			}
			float angleFlat = (component.labyrinthObelisk.Position - this.parent.Position).AngleFlat;
			if (angleFlat >= 315f || angleFlat < 45f)
			{
				this.direction = Rot4.North;
				return;
			}
			if (angleFlat >= 45f && angleFlat < 135f)
			{
				this.direction = Rot4.East;
				return;
			}
			if (angleFlat >= 135f && angleFlat < 225f)
			{
				this.direction = Rot4.South;
				return;
			}
			if (angleFlat >= 225f && angleFlat < 315f)
			{
				this.direction = Rot4.West;
			}
		}

		// Token: 0x06010083 RID: 65667 RVA: 0x004A46A8 File Offset: 0x004A28A8
		protected override void OnInteracted(Pawn caster)
		{
			this.deciphered = true;
			base.GenerateMessage();
			TaggedString label = "LetterLabelFloorEtchings".Translate();
			TaggedString text = "LetterFloorEtchings".Translate(caster.Named("PAWN"), this.message.Named("RAMBLINGS"), this.direction.ToStringHuman().ToLower().Named("DIRECTION"));
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, this.parent, null, null, null, null, 0, true);
		}

		// Token: 0x06010084 RID: 65668 RVA: 0x004A4734 File Offset: 0x004A2934
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.deciphered)
			{
				stringBuilder.AppendLineIfNotEmpty();
				stringBuilder.Append(string.Format("{0}: {1}", "FloorEtchingsInspectorExit".Translate(), this.direction.ToStringHuman()));
			}
			else
			{
				stringBuilder.Append(base.CompInspectStringExtra());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06010085 RID: 65669 RVA: 0x004A4796 File Offset: 0x004A2996
		public override string CompTipStringExtra()
		{
			if (!this.deciphered)
			{
				return "";
			}
			return string.Format("{0}: {1}", "FloorEtchingsInspectorExit".Translate(), this.direction.ToStringHuman());
		}

		// Token: 0x06010086 RID: 65670 RVA: 0x004A47CC File Offset: 0x004A29CC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<Rot4>(ref this.direction, "direction", default(Rot4), false);
		}

		// Token: 0x0400A7A7 RID: 42919
		private Rot4 direction;
	}
}
