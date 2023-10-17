

using System.Collections.Generic;

public class SelectorNode : Node
{
    private List<Node> children;

    public SelectorNode(List<Node> children)
    {
        this.children = children;
    }

    public NodeConstant Evaluate()
    {
        if (children == null)
        {
            return NodeConstant.FAILURE;
        }

        foreach (Node child in children)
        {
            switch (child.Evaluate())
            {
                case NodeConstant.SUCCESS:
                    return NodeConstant.SUCCESS;
                case NodeConstant.RUNNING:
                    return NodeConstant.RUNNING;
            }
        }

        return NodeConstant.FAILURE;
    }
}
