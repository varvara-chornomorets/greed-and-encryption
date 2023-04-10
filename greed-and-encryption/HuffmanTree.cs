namespace Greed_and_encryption;

	public class HuffmanTree
	{
		private HuffmanNode root;


		//It uses a dictionary to keep track of the frequency of each character and a MinHeap to build the Huffman tree.
		public HuffmanTree(string text)
		{

			// Create a frequency table to keep track of the number of occurrences of each character in the input 
			Dictionary<char, int> frequencyTable = new Dictionary<char, int>();

			foreach (char symbol in text)
			{
				if (frequencyTable.ContainsKey(symbol))
				{
					frequencyTable[symbol]++;
				}
				else
				{
					frequencyTable[symbol] = 1;
				}
			}
			MinHeap<HuffmanNode> heap = new MinHeap<HuffmanNode>();

			foreach (KeyValuePair<char, int> entry in frequencyTable)
			{
				// Create a new HuffmanNode for the character-frequency pair and add it to the MinHeap
				heap.Add(new HuffmanNode(entry.Key, entry.Value));
			}

			// Build the Huffman tree by removing the two lowest-frequency nodes from the MinHeap and combining them into a new node
			while (heap.Count > 1)	
			{
				HuffmanNode left = heap.Remove();
				HuffmanNode right = heap.Remove();
				heap.Add(new HuffmanNode(left, right));
			}

			// Set the root of the Huffman tree to be the remaining node in the MinHeap
			root = heap.Remove();
		}

		
		private Dictionary<char, string> BuildEncodingTable()
		{
			Dictionary<char, string> encodingTable = new Dictionary<char, string>();

			// Call the recursive helper method to traverse the Huffman tree and assign binary encodings to each leaf node
			BuildEncodingTable(root, "", encodingTable);

			return encodingTable;
		}

		private void BuildEncodingTable(HuffmanNode node, string prefix, Dictionary<char, string> encodingTable)
		{
			
			if (node == null)
			{
				return;
			}

			// If the node is a leaf node, add its binary encoding to the encoding table
			if (node.IsLeaf())
			{
				encodingTable[node.Symbol] = prefix;
			}
			// recursively call the helper method on the left and right child nodes, appending '0' or '1' to the prefix as needed
			else
			{
				BuildEncodingTable(node.LeftChild, prefix + "0", encodingTable);
				BuildEncodingTable(node.RightChild, prefix + "1", encodingTable);
			}
		}


		public string Encode(string text)
		{
			// Build encoding table from Huffman tree
			Dictionary<char, string> encodingTable = BuildEncodingTable();
			
			string encodedText = "";

			// Encode the input text using the encoding table
			foreach (char symbol in text)
			{
				encodedText += encodingTable[symbol];
			}

			return encodedText;
		}


		public string Decode(string encodedText)
		{
			string decodedText = "";
			HuffmanNode current = root;

			// Traverse the Huffman tree based on the bits in the encoded text
			foreach (char bit in encodedText)
			{
				if (bit == '0')
				{
					current = current.LeftChild;
				}
				else if (bit == '1')
				{
					current = current.RightChild;
				}

				// If we've reached a leaf node, add the corresponding symbol to the decoded text
				// and go back to the root of the tree
				if (current.IsLeaf())
				{
					decodedText += current.Symbol;
					current = root;
				}
			}

			return decodedText;
		}


		public string EncodeToFile(string inputFilePath, string outputFilePath)
		{
			// Read the input text from the file
			string text = File.ReadAllText(inputFilePath);

			// Create a Huffman tree and build the encoding table
			HuffmanTree huffmanTree = new HuffmanTree(text);
			Dictionary<char, string> encodingTable = huffmanTree.BuildEncodingTable();

			// Encode the text
			string encodedText = huffmanTree.Encode(text);

			// Write the encoded text and the encoding table to the output file
			using (StreamWriter writer = new StreamWriter(outputFilePath))
			{
				// Write the encoded text to the file
				writer.WriteLine(encodedText);
				writer.WriteLine($"\n");
				// Write the encoding table to the file
				foreach (KeyValuePair<char, string> entry in encodingTable)
				{
					writer.WriteLine($"{entry.Key}:{entry.Value}");
				}
			}

			// Return the path of the output file
			return outputFilePath;
		}


		public string DecodeFromFile(string inputFilePath, string outputFilePath)
		{
			// Read the encoded text and the encoding table from the file
			string encodedText = "";
			Dictionary<string, char> decodingTable = new Dictionary<string, char>();
			using (StreamReader reader = new StreamReader(inputFilePath))
			{
				// Read the encoded text from the file
				encodedText = reader.ReadLine();

				// Read the encoding table from the file
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					if (line == "") // skip empty line
						continue;

					string[] parts = line.Split(':');
					char symbol = parts[0][0];
					string code = parts[1];
					decodingTable[code] = symbol;
				}
			}

			// Decode the text
			string decodedText = "";
			string prefix = "";
			foreach (char bit in encodedText)
			{
				prefix += bit;
				if (decodingTable.ContainsKey(prefix))
				{
					char symbol = decodingTable[prefix];
					decodedText += symbol;
					prefix = "";
				}
			}

			// Write the decoded text to the output file
			File.WriteAllText(outputFilePath, decodedText);

			// Return the path of the output file
			return outputFilePath;
		}


		public void PrintEncodingTable()
		{
			Dictionary<char, string> encodingTable = BuildEncodingTable();

			Console.WriteLine("Encoding Table:");
			foreach (KeyValuePair<char, string> entry in encodingTable)
			{
				Console.WriteLine($"   {entry.Key} : {entry.Value}");
			}
				
		}
	}


