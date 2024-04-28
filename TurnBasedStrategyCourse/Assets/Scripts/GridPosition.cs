public struct GridPosition
{
    public int x;
    public int z;

    public GridPosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public override string ToString()
    {
        return "<color=#FF0000>x</color><color=#FFFFFF>: " + x + ",</color> <color=#0000FF>z</color><color=#FFFFFF>: " + z + "</color>";
    }
}