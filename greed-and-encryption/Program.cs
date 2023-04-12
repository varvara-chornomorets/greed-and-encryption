using System.Data;
using greed_and_encryption;

Dictionary<char?, int> GetFreqeunciesDictionary(string path)
{
    using StreamReader reader = new StreamReader(path);
    
    string text = reader.ReadToEnd();
    
    Dictionary<char?, int> frequencies = new Dictionary<char?, int>();

    foreach (var c in text)
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

    return frequencies;
    reader.Close();
}

Node GetRoot(Dictionary<char?, int> frequenciesDictionary)
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
    return root;
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



void EncodeToString(string inputFilePath, string outputFilePath)
{
    var frequencies = GetFreqeunciesDictionary(inputFilePath);
    var prefixCodesRoot = GetRoot(frequencies);
    var prefixCodes = GetPrefixCodesFromRoot(prefixCodesRoot);
    using (StreamWriter writer = new StreamWriter(outputFilePath))
    {
        foreach (var pair in prefixCodes)
        {
            char c = pair.Key.Value;
            string asciiBinary = Convert.ToString(c, 2);
            for (int i = 0; i < 8 - asciiBinary.Length; i++)
            {
                asciiBinary = "0" + asciiBinary;
            }
            writer.Write(asciiBinary, prefixCodes[c]);
        }
        writer.Write("aaaaaaaa");
    

        using StreamReader reader = new StreamReader(inputFilePath) ;
        var text = reader.ReadToEnd();
        foreach (var c in text)
        {
            writer.Write(prefixCodes[c]);
        }
        reader.Close();
    }
}

void DecodeString(string fileToDecodePath, string outputFilePath)
{
    using StreamReader reader = new StreamReader(fileToDecodePath);
    var content = reader.ReadToEnd();
    var cur = "";
    int counter = 1;
    while (cur != "aaaaaaaa")
    {
        cur = content.Substring(8 * (counter-1), 8);
        counter++;
        Console.WriteLine(cur);

    }


}


// EncodeToString("sherlock.txt", "example.txt");
// DecodeString("example.txt", "decoded.txt");