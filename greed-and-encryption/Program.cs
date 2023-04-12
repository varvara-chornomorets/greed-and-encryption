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

Dictionary<char, int> frequencies = GetFreqeunciesDictionary("sherlock.txt");

PriorityQueue<Node, int> huffmanTree = new PriorityQueue<Node, int>();
foreach (var element in frequencies)
{
    huffmanTree.Enqueue( new Node(element.Value, element.Key, null, null),element.Value );
}

while (huffmanTree.Count > 1)
{
    Node curLeft =  huffmanTree.Dequeue();
    Node curRight = huffmanTree.Dequeue();
    Node merged = curLeft.Merge(curRight);
    huffmanTree.Enqueue(merged, merged.Frequency);
    Console.WriteLine(merged.Frequency);
}











/* foreach (var element in frequencies)
{
    Console.WriteLine(element);
}

Console.WriteLine("           ");
Console.WriteLine("           ");
Console.WriteLine("           ");
Console.WriteLine("           ");
Console.WriteLine("           ");
Console.WriteLine("           ");
for (int i = 0; i < frequencies.Count; i++)
{
    Console.WriteLine(huffmanTree.Dequeue());
}
*/