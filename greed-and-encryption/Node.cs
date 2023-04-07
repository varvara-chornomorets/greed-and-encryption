namespace greed_and_encryption;

public class Node
{
    public Node(int frequency, char? character, Node? rightChild, Node? leftchild)
    {
        Frequency = frequency;
        Character = character;
        RightChild = rightChild;
        Leftchild = leftchild;
    }

    public int Frequency { get; set; }
    public char? Character { get; set; }
    public Node? RightChild { get; set; }
    public Node? Leftchild { get; set; }

    public Node Merge(Node first, Node second)
    {
        return new Node(first.Frequency + second.Frequency, 
            null, 
            second, first);
    }
    
}