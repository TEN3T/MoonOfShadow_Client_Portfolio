

public class BehaviorTreeManager
{
    private Node rootNode;

    public BehaviorTreeManager(Node rootNode)
    {
        this.rootNode = rootNode;
    }

    public void Active()
    {
        rootNode.Evaluate();
    }
}
