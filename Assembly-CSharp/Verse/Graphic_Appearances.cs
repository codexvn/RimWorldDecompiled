using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200088D RID: 2189
	public class Graphic_Appearances : Graphic
	{
		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x06003786 RID: 14214 RVA: 0x00128F44 File Offset: 0x00127144
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[(int)StuffAppearanceDefOf.Smooth.index].MatSingle;
			}
		}

		// Token: 0x06003787 RID: 14215 RVA: 0x00128F5C File Offset: 0x0012715C
		private ThingDef StuffOfThing(Thing thing)
		{
			IConstructible constructible = thing as IConstructible;
			if (constructible != null)
			{
				return constructible.EntityToBuildStuff();
			}
			return thing.Stuff;
		}

		// Token: 0x06003788 RID: 14216 RVA: 0x00128F80 File Offset: 0x00127180
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			return this.SubGraphicFor(thing).MatAt(rot, thing);
		}

		// Token: 0x06003789 RID: 14217 RVA: 0x00128F90 File Offset: 0x00127190
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			this.path = req.path;
			this.color = req.color;
			this.drawSize = req.drawSize;
			List<StuffAppearanceDef> allDefsListForReading = DefDatabase<StuffAppearanceDef>.AllDefsListForReading;
			this.subGraphics = new Graphic[allDefsListForReading.Count];
			for (int i = 0; i < this.subGraphics.Length; i++)
			{
				StuffAppearanceDef stuffAppearance = allDefsListForReading[i];
				string text = req.path;
				if (!stuffAppearance.pathPrefix.NullOrEmpty())
				{
					text = stuffAppearance.pathPrefix + "/" + text.Split('/', StringSplitOptions.None).Last<string>();
				}
				Texture2D texture2D = (from x in ContentFinder<Texture2D>.GetAllInFolder(text)
				where x.name.EndsWith(stuffAppearance.defName)
				select x).FirstOrDefault<Texture2D>();
				if (texture2D != null)
				{
					this.subGraphics[i] = GraphicDatabase.Get<Graphic_Single>(text + "/" + texture2D.name, req.shader, this.drawSize, this.color);
				}
			}
			for (int j = 0; j < this.subGraphics.Length; j++)
			{
				if (this.subGraphics[j] == null)
				{
					this.subGraphics[j] = this.subGraphics[(int)StuffAppearanceDefOf.Smooth.index];
				}
			}
		}

		// Token: 0x0600378A RID: 14218 RVA: 0x001290E2 File Offset: 0x001272E2
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			if (newColorTwo != Color.white)
			{
				Log.ErrorOnce("Cannot use Graphic_Appearances.GetColoredVersion with a non-white colorTwo.", 9910251);
			}
			return GraphicDatabase.Get<Graphic_Appearances>(this.path, newShader, this.drawSize, newColor, Color.white, this.data, null);
		}

		// Token: 0x0600378B RID: 14219 RVA: 0x0012911F File Offset: 0x0012731F
		public override Material MatSingleFor(Thing thing)
		{
			return this.SubGraphicFor(thing).MatSingleFor(thing);
		}

		// Token: 0x0600378C RID: 14220 RVA: 0x0012912E File Offset: 0x0012732E
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			this.SubGraphicFor(thing).DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}

		// Token: 0x0600378D RID: 14221 RVA: 0x00129144 File Offset: 0x00127344
		public Graphic SubGraphicFor(Thing thing)
		{
			StuffAppearanceDef smooth = StuffAppearanceDefOf.Smooth;
			if (thing != null)
			{
				return this.SubGraphicFor(this.StuffOfThing(thing));
			}
			return this.subGraphics[(int)smooth.index];
		}

		// Token: 0x0600378E RID: 14222 RVA: 0x00129178 File Offset: 0x00127378
		public Graphic SubGraphicFor(ThingDef stuff)
		{
			StuffAppearanceDef app = StuffAppearanceDefOf.Smooth;
			if (stuff != null && stuff.stuffProps.appearance != null)
			{
				app = stuff.stuffProps.appearance;
			}
			return this.SubGraphicFor(app);
		}

		// Token: 0x0600378F RID: 14223 RVA: 0x001291AE File Offset: 0x001273AE
		public Graphic SubGraphicFor(StuffAppearanceDef app)
		{
			return this.subGraphics[(int)app.index];
		}

		// Token: 0x06003790 RID: 14224 RVA: 0x001291C0 File Offset: 0x001273C0
		public override string ToString()
		{
			string[] array = new string[5];
			array[0] = "Appearance(path=";
			array[1] = this.path;
			array[2] = ", color=";
			int num = 3;
			Color color = this.color;
			array[num] = color.ToString();
			array[4] = ", colorTwo=unsupported)";
			return string.Concat(array);
		}

		// Token: 0x04002983 RID: 10627
		protected Graphic[] subGraphics;
	}
}
