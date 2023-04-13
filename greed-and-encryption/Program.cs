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

        cur.RightChild.PrefixCode = cur.PrefixCode + "1";
        cur.Leftchild.PrefixCode += cur.PrefixCode + "0";
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

void EncodeToStringNumeric(string inputFilePath, string outputFilePath)
{
    var frequencies = GetFreqeunciesDictionary(inputFilePath);
    var prefixCodesRoot = GetRoot(frequencies);
    var prefixCodes = GetPrefixCodesFromRoot(prefixCodesRoot);
    using var writer = new StreamWriter(outputFilePath);
    foreach (var pair in prefixCodes)
    {
        var fullCode = pair.Value.PadLeft(16, '0');
        writer.Write($"{pair.Key}|{fullCode}|");
        Console.WriteLine($"key: {pair.Key}, code: {pair.Value}, full code: {fullCode}");
    }
        
    using var reader = new StreamReader(inputFilePath) ;
    var text = reader.ReadToEnd();
    foreach (var c in text)
    {
        string prefixCode = prefixCodes[c];
        int correspondingNumber= Convert.ToUInt16(prefixCode, 2);
        var correspondingSymbol = Convert.ToChar(correspondingNumber);
        writer.Write(correspondingSymbol);
    }
    reader.Close();
    Console.WriteLine(prefixCodes.Count);
}

void DecodeStringNumeric(string fileToDecodePath, string outputFilePath)
{
    using var reader = new StreamReader(fileToDecodePath);
    using var writer = new StreamWriter(outputFilePath);
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
    foreach (var c in actualText)
    {
        var correspondingNumber = Convert.ToUInt16(c);
        var binaryStr = Convert.ToString(correspondingNumber, 2);
        var fullBinaryStr = binaryStr.PadLeft(16, '0');
        writer.Write(decodingTable[fullBinaryStr]);
    }
    Console.WriteLine(decodingTable.Count);

}





void EncodeToStringNumericEfficient(string inputFilePath, string outputFilePath)
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
    var encodedText = "";
    foreach (var c in text)
    {
        string prefixCode = prefixCodes[c];
        encodedText += prefixCode;
    }

    var lfd = encodedText.Length;
    var dklfjgklfd = lfd % 8;
    var ldfjglkdfgl = 8 - dklfjgklfd;
    writer.Write($"{8 - encodedText.Length % 8}|");
    
    string binaryLine = "";
    int left = 0;
    for (int counter = 0; counter < encodedText.Length; counter = counter +1)
    {
        if (counter % 8 == 0 & counter != 0)
        {
            var myByte = Convert.ToByte(binaryLine, 2);
            var c = Convert.ToChar(myByte);
            writer.Write(c);
            Console.WriteLine("+");
            binaryLine = "";
        }

        binaryLine = binaryLine + encodedText[counter];
        left = counter % 8; ;
    }

    for (int i = 0; i < 8-left-1; i++)
    {
        binaryLine = binaryLine + "0";
    }
    var myByteLast = Convert.ToByte(binaryLine, 2);
    var cLast = Convert.ToChar(myByteLast);
    writer.Write(cLast);
    Console.WriteLine("+");
    reader.Close();
    writer.Close();
}
void DecodeStringNumericEfficient(string fileToDecodePath, string outputFilePath)
{
    using var reader = new StreamReader(fileToDecodePath);
    using var writer = new StreamWriter(outputFilePath);
    var content = reader.ReadToEnd();
    // creating a table for decoding
    var decodingTable = new Dictionary<string, char>();
    var contents = content.Split( '|' );
    var even = true;
    for (var i = 0; i < contents.Length - 2; i++)
    {
        if (even)
        {
            var symbol = contents[i].ToCharArray()[0];
            var code = contents[i + 1];
            decodingTable[code] = symbol;
        }
        even = !even;
    }

    var extraSymbolsNumber = int.Parse(contents[contents.Length - 2]);
    if (extraSymbolsNumber == 8)
    {
        extraSymbolsNumber = 0;
    }
    var actualTextWithExtra0 = contents[contents.Length-1];
    string binaryTextWith0 = "";
    foreach (var c in actualTextWithExtra0)
    {
        var correspondingNumber = Convert.ToByte(c);
        var binaryStr = Convert.ToString(correspondingNumber, 2);
        var fullBinaryStr = binaryStr.PadLeft(8, '0');
        binaryTextWith0 += fullBinaryStr;
    }

    var binaryText = binaryTextWith0.Substring(0, binaryTextWith0.Length - extraSymbolsNumber);
    var buffer = "";
    foreach (var c in binaryText)
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


EncodeToStringNumericEfficient("sherlock.txt", "example.txt");
DecodeStringNumericEfficient("example.txt", "result.txt");