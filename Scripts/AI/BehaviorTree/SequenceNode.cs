

using System.Collections.Generic;

public class SequenceNode : Node
{
    private List<Node> children;

    public SequenceNode(List<Node> children)
    {
        this.children = children;
    }

    public NodeConstant Evaluate()
    {
        if (children == null || children.Count == 0)
        {
            return NodeConstant.FAILURE;
        }

        foreach (Node child in children)
        {
            switch (child.Evaluate())
            {
                case NodeConstant.SUCCESS:
                    continue;
                case NodeConstant.FAILURE:
                    return NodeConstant.FAILURE;
                case NodeConstant.RUNNING:
                    return NodeConstant.RUNNING;
            }
        }

        return NodeConstant.SUCCESS;
    }
}
