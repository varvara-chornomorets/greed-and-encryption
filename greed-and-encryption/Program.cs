using greed_and_encryption;

Console.WriteLine(Directory.GetCurrentDirectory());
// sorry, i didn't manage to make not absolute path work, so i use absolute,
// which is different for you

string variaPath = "C:/Users/chorn/Desktop/it/greed-and-encryption/greed-and-encryption/sherlock.txt";
string[] lines = File.ReadAllLines(variaPath);
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

PriorityQueue<Node, int> huffmanTree = new PriorityQueue<Node, int>();
foreach (var element in frequencies)
{
    huffmanTree.Enqueue( new Node(element.Value, element.Key, null, null),element.Value );
}

while (huffmanTree.Count > 0)
{
    Node node =  huffmanTree.Dequeue();
    Console.WriteLine($"{node.Character} + {node.Frequency}");
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