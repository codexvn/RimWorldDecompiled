using System;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02002CB4 RID: 11444
	public class CompGeneratedNames : ThingComp
	{
		// Token: 0x170028A0 RID: 10400
		// (get) Token: 0x0601010A RID: 65802 RVA: 0x004A6213 File Offset: 0x004A4413
		public CompProperties_GeneratedName Props
		{
			get
			{
				return (CompProperties_GeneratedName)this.props;
			}
		}

		// Token: 0x170028A1 RID: 10401
		// (get) Token: 0x0601010B RID: 65803 RVA: 0x004A6220 File Offset: 0x004A4420
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x0601010C RID: 65804 RVA: 0x004A6228 File Offset: 0x004A4428
		public static string GenerateName(CompProperties_GeneratedName props)
		{
			return GenText.CapitalizeAsTitle(GrammarResolver.Resolve("r_weapon_name", new GrammarRequest
			{
				Includes = 
				{
					props.nameMaker
				}
			}, null, false, null, null, null, true));
		}

		// Token: 0x0601010D RID: 65805 RVA: 0x004A6268 File Offset: 0x004A4468
		public override string TransformLabel(string label)
		{
			if (this.parent.StyleSourcePrecept != null)
			{
				return label;
			}
			if (this.parent.GetComp<CompBladelinkWeapon>() != null)
			{
				return this.name + ", " + label;
			}
			return this.name + " (" + label + ")";
		}

		// Token: 0x0601010E RID: 65806 RVA: 0x004A62B9 File Offset: 0x004A44B9
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.name = CompGeneratedNames.GenerateName(this.Props);
		}

		// Token: 0x0601010F RID: 65807 RVA: 0x004A62D3 File Offset: 0x004A44D3
		public override void PostExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
		}

		// Token: 0x0400A7DC RID: 42972
		private string name;
	}
}
