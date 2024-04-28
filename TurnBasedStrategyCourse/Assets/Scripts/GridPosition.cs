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
        return "<color=#E46252>x</color><color=#FFFFFF> " + x + ",</color> <color=#5E88F5>z</color><color=#FFFFFF> " + z + "</color>";
    }
}