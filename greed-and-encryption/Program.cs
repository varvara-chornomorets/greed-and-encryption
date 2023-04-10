using Greed_and_encryption;

string InputFilePath = @"C:\Users\Elisa\RiderProjects\Greed-and-encryption\Greed and encryption\SherLocked.txt";

string OutPutFilePath = @"C:\Users\Elisa\RiderProjects\Greed-and-encryption\Greed and encryption\encoded.txt";

string DecodedOutPutFilePath = @"C:\Users\Elisa\RiderProjects\Greed-and-encryption\Greed and encryption\decoded.txt";

string fileContent = File.ReadAllText(InputFilePath);

HuffmanTree huffmanTree = new HuffmanTree(fileContent);

huffmanTree.PrintEncodingTable();


Console.WriteLine("\n");

string encodedText = huffmanTree.Encode(fileContent);
Console.WriteLine("Encoded text: " + encodedText + "\n");

string decodedText = huffmanTree.Decode(encodedText);
Console.WriteLine("Decoded text: " + decodedText + "\n");


//string encodeToFile = huffmanTree.EncodeToFile(InputFilePath, OutPutFilePath);
//Console.WriteLine("Encoded file written to " + encodeToFile);

//string decodeToFile = huffmanTree.DecodeFromFile(OutPutFilePath, DecodedOutPutFilePath);
//Console.WriteLine("Decoded text from file: " + decodeToFile);
