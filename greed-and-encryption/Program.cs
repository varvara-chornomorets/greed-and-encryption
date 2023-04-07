Console.WriteLine(Directory.GetCurrentDirectory());
// sorry, i didn't manage to make not absolute path work, so i use absolute, which is different for you
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

foreach (var element in frequencies)
{
    Console.WriteLine(element);
}