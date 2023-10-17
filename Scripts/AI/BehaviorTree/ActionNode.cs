

using System;

public sealed class ActionNode : Node
{
    private Func<NodeConstant> action;

    public ActionNode(Func<NodeConstant> action)
    {
        this.action = action;
    }

    public NodeConstant Evaluate()
    {
        return action?.Invoke() ?? NodeConstant.FAILURE;
    }
}
