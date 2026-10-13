namespace UnityEngine;

public struct CharacterInfo
{
	public int index;

	public Rect uv;

	public Rect vert;

	public float width;

	public int size;

	public FontStyle style;

	public bool flipped;

	public int advance
	{
		get
		{
			return (int)width;
		}
		set
		{
			width = value;
		}
	}

	public int glyphWidth
	{
		get
		{
			return (int)vert.width;
		}
		set
		{
			vert.width = value;
		}
	}

	public int glyphHeight
	{
		get
		{
			return (int)(0f - vert.height);
		}
		set
		{
			vert.height = -value;
		}
	}

	public int bearing
	{
		get
		{
			return (int)vert.x;
		}
		set
		{
			vert.x = value;
		}
	}

	public int minX
	{
		get
		{
			return (int)vert.x;
		}
		set
		{
			vert.x = value;
		}
	}

	public int maxX
	{
		get
		{
			return (int)(vert.x + vert.width);
		}
		set
		{
			vert.width = (float)value - vert.x;
		}
	}

	public int maxY
	{
		get
		{
			return (int)vert.y;
		}
		set
		{
			vert.y = value;
		}
	}

	public int minY
	{
		get
		{
			return (int)(vert.y + vert.height);
		}
		set
		{
			vert.height = (float)value - vert.y;
		}
	}

	public Vector2 uvBottomLeft
	{
		get
		{
			if (!flipped)
			{
				return new Vector2(uv.x, uv.y);
			}
			return new Vector2(uv.x, uv.yMax);
		}
		set
		{
			uv = (flipped ? new Rect(value.x, uv.y, uv.width, value.y - uv.y) : new Rect(value.x, value.y, uv.xMax - value.x, uv.yMax - value.y));
		}
	}

	public Vector2 uvBottomRight
	{
		get
		{
			if (!flipped)
			{
				return new Vector2(uv.xMax, uv.y);
			}
			return new Vector2(uv.x, uv.y);
		}
		set
		{
		}
	}

	public Vector2 uvTopRight
	{
		get
		{
			if (!flipped)
			{
				return new Vector2(uv.xMax, uv.yMax);
			}
			return new Vector2(uv.xMax, uv.y);
		}
		set
		{
		}
	}

	public Vector2 uvTopLeft
	{
		get
		{
			if (!flipped)
			{
				return new Vector2(uv.x, uv.yMax);
			}
			return new Vector2(uv.xMax, uv.yMax);
		}
		set
		{
		}
	}
}
