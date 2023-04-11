using Greed_and_encryption;

string InputFilePath = @"C:\Users\Elisa\RiderProjects\greed-and-encryption\greed-and-encryption\SherLocked.txt";

string EncodedFilePath = @"C:\Users\Elisa\RiderProjects\greed-and-encryption\greed-and-encryption\encoded.txt";

string EncodedBytesFilePath = @"C:\Users\Elisa\RiderProjects\greed-and-encryption\greed-and-encryption\encodedBytes.txt";

string DecodedFilePath = @"C:\Users\Elisa\RiderProjects\greed-and-encryption\greed-and-encryption\decoded.txt";

string DecodedBytesFilePath = @"C:\Users\Elisa\RiderProjects\greed-and-encryption\greed-and-encryption\decodedBytes.txt";

string fileContent = File.ReadAllText(InputFilePath);

HuffmanTree huffmanTree = new HuffmanTree(fileContent);

huffmanTree.PrintEncodingTable();


Console.WriteLine("\n");

string encodedText = huffmanTree.Encode(fileContent);
Console.WriteLine("Encoded text: " + encodedText + "\n");

string decodedText = huffmanTree.Decode(encodedText);
Console.WriteLine("Decoded text using Decode: " + decodedText );

//Chars
huffmanTree.EncodeToFile(InputFilePath, EncodedFilePath);

string decodedFromFile = huffmanTree.DecodeFromFile(EncodedFilePath);
Console.WriteLine("Decoded text using DecodeFromFile: " + decodedFromFile + "\n");

huffmanTree.DecodeFromFileToFile(EncodedFilePath, DecodedFilePath);

//Bytes
huffmanTree.EncodeBytesToFile(InputFilePath, EncodedBytesFilePath);

string decodedBytesFromFile = huffmanTree.DecodeBytesFromFile(EncodedBytesFilePath);
Console.WriteLine("Decoded text using DecodeBytesFromFile: " + decodedBytesFromFile + "\n");

huffmanTree.DecodeBytesFromFileToFile(EncodedBytesFilePath, DecodedBytesFilePath);
