using System.Data;
using System.Diagnostics.SymbolStore;
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
    using var writer = new StreamWriter(outputFilePath);
    foreach (var pair in prefixCodes)
    {
        writer.Write($"{pair.Key}|{pair.Value}|");
    }
        
    using var reader = new StreamReader(inputFilePath) ;
    var text = reader.ReadToEnd();
    foreach (var c in text)
    {
        writer.Write(prefixCodes[c]);
    }
    reader.Close();
}

void DecodeString(string fileToDecodePath, string outputFilePath)
{
    using var reader = new StreamReader(fileToDecodePath);
    using var writer = new StreamWriter(outputFilePath);
    {
        
    }
    var content = reader.ReadToEnd();
    // creating a table for decoding
    var decodingTable = new Dictionary<string, char>();
    var contents = content.Split( '|' );
    var even = true;
    for (var i = 0; i < contents.Length - 1; i++)
    {
        if (even)
        {
            var symbol = contents[i].ToCharArray()[0];
            var code = contents[i + 1];
            decodingTable[code] = symbol;
        }
        even = !even;
    }

    var actualText = contents[^1];
    var buffer = "";
    foreach (var c in actualText)
    {
        buffer += c;
        try 
        {
            var symbol= decodingTable[buffer];
            writer.Write(symbol);
            buffer = "";
        }
        catch
        {
            // ignored
        }
    }


}


EncodeToString("sherlock.txt", "example.txt");
DecodeString("example.txt", "decoded.txt");