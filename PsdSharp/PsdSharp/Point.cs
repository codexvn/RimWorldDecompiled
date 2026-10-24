namespace PsdSharp;

public readonly struct Point
{
	public int X => _003Cx_003EP;

	public int Y => _003Cy_003EP;

	public Point(int x, int y)
	{
		_003Cx_003EP = x;
		_003Cy_003EP = y;
	}
}
