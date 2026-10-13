namespace Waylith;

public struct Resolution
{
	public int Width;

	public int Height;

	public double RefreshRate;

	public Resolution(int width, int height, double refreshRate)
	{
		Width = width;
		Height = height;
		RefreshRate = refreshRate;
	}
}
