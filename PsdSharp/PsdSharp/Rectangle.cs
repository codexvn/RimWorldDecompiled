using System.Drawing;

namespace PsdSharp;

public readonly struct Rectangle
{
	public Point TopLeft => _003CtopLeft_003EP;

	public Point BottomRight => _003CbottomRight_003EP;

	public uint Width => (uint)(_003CbottomRight_003EP.X - _003CtopLeft_003EP.X + 1);

	public uint Height => (uint)(_003CbottomRight_003EP.Y - _003CtopLeft_003EP.Y + 1);

	public Rectangle(Point topLeft, Point bottomRight)
	{
		_003CtopLeft_003EP = topLeft;
		_003CbottomRight_003EP = bottomRight;
	}

	public System.Drawing.Rectangle ToRectangle()
	{
		return new System.Drawing.Rectangle(_003CtopLeft_003EP.X, _003CtopLeft_003EP.Y, (int)Width, (int)Height);
	}
}
