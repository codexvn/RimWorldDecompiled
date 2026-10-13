using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E8E RID: 7822
	public class Area_Allowed : Area
	{
		// Token: 0x17001A89 RID: 6793
		// (get) Token: 0x0600ADA1 RID: 44449 RVA: 0x00318FE0 File Offset: 0x003171E0
		public override string Label
		{
			get
			{
				return this.labelInt;
			}
		}

		// Token: 0x17001A8A RID: 6794
		// (get) Token: 0x0600ADA2 RID: 44450 RVA: 0x00318FE8 File Offset: 0x003171E8
		public override Color Color
		{
			get
			{
				return this.colorInt;
			}
		}

		// Token: 0x17001A8B RID: 6795
		// (get) Token: 0x0600ADA3 RID: 44451 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool Mutable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A8C RID: 6796
		// (get) Token: 0x0600ADA4 RID: 44452 RVA: 0x00318FF0 File Offset: 0x003171F0
		public override int ListPriority
		{
			get
			{
				return 500;
			}
		}

		// Token: 0x17001A8D RID: 6797
		// (get) Token: 0x0600ADA5 RID: 44453 RVA: 0x00318FF7 File Offset: 0x003171F7
		// (set) Token: 0x0600ADA6 RID: 44454 RVA: 0x00319009 File Offset: 0x00317209
		public override string RenamableLabel
		{
			get
			{
				return this.labelInt ?? this.BaseLabel;
			}
			set
			{
				this.labelInt = value;
			}
		}

		// Token: 0x17001A8E RID: 6798
		// (get) Token: 0x0600ADA7 RID: 44455 RVA: 0x00319012 File Offset: 0x00317212
		public override string BaseLabel
		{
			get
			{
				return "Area";
			}
		}

		// Token: 0x0600ADA8 RID: 44456 RVA: 0x00319019 File Offset: 0x00317219
		public Area_Allowed()
		{
		}

		// Token: 0x0600ADA9 RID: 44457 RVA: 0x0031902C File Offset: 0x0031722C
		public Area_Allowed(AreaManager areaManager, string label = null) : base(areaManager)
		{
			this.areaManager = areaManager;
			if (!label.NullOrEmpty())
			{
				this.labelInt = label;
			}
			else
			{
				int num = 1;
				for (;;)
				{
					this.labelInt = "AreaDefaultLabel".Translate(num);
					if (areaManager.GetLabeled(this.labelInt) == null)
					{
						break;
					}
					num++;
				}
			}
			this.colorInt = new Color(Rand.Value, Rand.Value, Rand.Value);
			this.colorInt = Color.Lerp(this.colorInt, Color.gray, 0.5f);
		}

		// Token: 0x0600ADAA RID: 44458 RVA: 0x003190CC File Offset: 0x003172CC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.labelInt, "label", null, false);
			Scribe_Values.Look<Color>(ref this.colorInt, "color", default(Color), false);
		}

		// Token: 0x0600ADAB RID: 44459 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool AssignableAsAllowed()
		{
			return true;
		}

		// Token: 0x0600ADAC RID: 44460 RVA: 0x0031910B File Offset: 0x0031730B
		public void SetColor(Color newColor)
		{
			this.colorInt = newColor;
			base.InvalidateColorTexture();
		}

		// Token: 0x0600ADAD RID: 44461 RVA: 0x00319009 File Offset: 0x00317209
		public void SetLabel(string newLabel)
		{
			this.labelInt = newLabel;
		}

		// Token: 0x0600ADAE RID: 44462 RVA: 0x0031911A File Offset: 0x0031731A
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID.ToString() + "_Named_" + this.labelInt;
		}

		// Token: 0x0600ADAF RID: 44463 RVA: 0x00318FE0 File Offset: 0x003171E0
		public override string ToString()
		{
			return this.labelInt;
		}

		// Token: 0x04007781 RID: 30593
		private string labelInt;

		// Token: 0x04007782 RID: 30594
		private Color colorInt = Color.red;
	}
}
