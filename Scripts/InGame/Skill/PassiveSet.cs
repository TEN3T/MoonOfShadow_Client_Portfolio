
public struct PassiveSet
{
    public float param { get; private set; }
    public SKILLCONSTANT.CALC_MODE calcMode { get; private set; }

    public PassiveSet(float param, SKILLCONSTANT.CALC_MODE calcMode)
    {
        this.param = param;
        this.calcMode = calcMode;
    }
}
