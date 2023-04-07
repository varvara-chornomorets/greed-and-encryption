namespace greed_and_encryption;

public class Node
{
    public Node(int frequency, string character, Node? rightChild, Node? leftchild)
    {
        Frequency = frequency;
        Character = character;
        RightChild = rightChild;
        Leftchild = leftchild;
    }

    public int Frequency { get; set; }
    public string Character { get; set; }
    public Node? RightChild { get; set; }
    public Node? Leftchild { get; set; }

    public Node Merge(Node first, Node second)
    {
        return new Node(first.Frequency + second.Frequency, 
            first.Character + second.Character, 
            second, first);
    }
    
}