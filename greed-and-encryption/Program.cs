using System.Data;
using greed_and_encryption;

Dictionary<char, int> GetFreqeunciesDictionary(string path)
{
    string[] lines = File.ReadAllLines(path);
    Dictionary<char, int> frequencies = new Dictionary<char, int>();
    foreach(var line in lines)
    {
        foreach (var c in line)
        {
            try
            {
                frequencies[c] += 1;
            }
            catch(Exception ex)
            {
                frequencies[c] = 1;
            }
        }
    }

    return frequencies;
}

Dictionary<char?, string> GetCodesForEachSymbol(Dictionary<char, int> frequenciesDictionary)
{
    PriorityQueue<Node, int> huffmanTree = new PriorityQueue<Node, int>();
    foreach (var element in frequenciesDictionary)
    {
        huffmanTree.Enqueue( new Node(element.Value, element.Key, null, null),element.Value );
    }

    Node merged = new Node(0, null, null, null);
    while (huffmanTree.Count > 1)
    {
        Node curLeft =  huffmanTree.Dequeue();
        Node curRight = huffmanTree.Dequeue();
        merged = curLeft.Merge(curRight);
        huffmanTree.Enqueue(merged, merged.Frequency);
    }

    Node root = merged;
    return GetPrefixCodesFromRoot(root);
}

Dictionary<char?, string> GetPrefixCodesFromRoot(Node root)
{
    var result = new Dictionary<char?, string>();
    Stack<Node> stack = new Stack<Node>();
    stack.Push(root);
    
    while (stack.Count > 0)
    {
        Node cur = stack.Pop();
        if (cur.RightChild == null)
        {
            result[cur.Character] = cur.PrefixCode;
            continue;
        }

        cur.RightChild.PrefixCode = cur.PrefixCode + "0";
        cur.Leftchild.PrefixCode += cur.PrefixCode + "1";
        stack.Push(cur.RightChild);
        stack.Push(cur.Leftchild);
    }

    return result;
}



var frequencies = GetFreqeunciesDictionary("sherlock.txt");
var prefixCodes = GetCodesForEachSymbol(frequencies);
foreach (var pair in prefixCodes)
{
    Console.WriteLine($"element is {pair.Key} and prefix code is {pair.Value}");
}
