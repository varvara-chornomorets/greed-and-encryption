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
			
			// recursively call the helper method on the left and right child nodes, appending '0' or '1'
			// to the prefix as needed
			else
			{
				BuildEncodingTable(node.LeftChild, prefix + "1", encodingTable);
				BuildEncodingTable(node.RightChild, prefix + "0", encodingTable);
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
				if (bit == '1')
				{
					current = current.LeftChild;
				}
				else if (bit == '0')
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
		
		public void EncodeToFile(string inputFilePath, string outputFilePath)
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
				// Write the encoding table to the file
				foreach (KeyValuePair<char, string> entry in encodingTable)
				{
					writer.WriteLine($"{entry.Key}:{entry.Value}");
				}
				
				// Write the encoded text to the file
				writer.WriteLine(encodedText);
			}
		}

		public void EncodeBytesToFile(string inputFilePath, string outputFilePath)
		{
			using (var reader = new StreamReader(inputFilePath))
			using (var writer = new BinaryWriter(new FileStream(outputFilePath, FileMode.Create)))
			{

				string input = reader.ReadToEnd();

				// applying Huffman encoding to the string
				HuffmanTree huffmanTree = new HuffmanTree(input); 
				Dictionary<char, string> encodingTable = huffmanTree.BuildEncodingTable(); 
				string encoded = huffmanTree.Encode(input);
				
				// write down the length of the encoded text so that you can decode it correctly later
				writer.Write(encoded.Length);

				// write the encoding table to the binary file 
				writer.Write(encodingTable.Count); 
				foreach (KeyValuePair<char, string> entry in encodingTable) 
				{ 
					writer.Write(entry.Key); 
					writer.Write(entry.Value.Length); 
					writer.Write(entry.Value.ToCharArray()); 
				}
				
				//  write the encoded text to a binary file
				for (int i = 0; i < encoded.Length; i += 8)
				{
					string chunk = encoded.Substring(i, Math.Min(8, encoded.Length - i));
					byte b = Convert.ToByte(chunk.PadRight(8, '0'), 2);
					writer.Write(b);
				}
			}
		}
		
		public string DecodeFromFile(string inputFilePath)
		{
			Dictionary<string, char> decodingTable = new Dictionary<string, char>();
			string encodedText = "";
			char[] Trimchar = { ' ', ':' };
			using (StreamReader reader = new StreamReader(inputFilePath))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					// Check if the line contains a key-value pair for the decoding table
					int separatorIndex = line.IndexOf(':');
					if (separatorIndex != -1)
					{
						char key = line[0];
						string value = line.Substring(separatorIndex + 1);
						decodingTable[value] = key;
					}
					else
					{
						// The line contains the encoded text
						encodedText += line;
					}
				}
			}

			// Decode the text using the decoding table
			string decodedText = "";
			string prefix = "";
			foreach (char elem in encodedText)
			{
				prefix += elem;
				if (decodingTable.ContainsKey(prefix))
				{
					decodedText += decodingTable[prefix];
					prefix = "";
				}
			}

			return decodedText.TrimEnd(Trimchar);
		}

		public void DecodeFromFileToFile(string inputFilePath, string outputFilePath)
		{
			string decoded = DecodeFromFile(inputFilePath);

			File.WriteAllText(outputFilePath, decoded);
		}
		
		public string DecodeBytesFromFile(string inputFilePath)
		{
			using (var reader = new BinaryReader(new FileStream(inputFilePath, FileMode.Open)))
			{
				// read the length of the encoded text that we recorded during encoding
				int encodedLength = reader.ReadInt32();

				// read the encoding table from the binary file 
				int tableSize = reader.ReadInt32(); 
				Dictionary<string, char> decodingTable = new Dictionary<string, char>(tableSize); 
				for (int i = 0; i < tableSize; i++) 
				{ 
					char c = reader.ReadChar(); 
					int encodingLength = reader.ReadInt32(); 
					char[] encodingChars = reader.ReadChars(encodingLength); 
					string encoding = new string(encodingChars); 
					decodingTable[encoding] = c; 
				} 
				
				// read the encoded text from the binary file
				byte[] bytes = reader.ReadBytes((int) Math.Ceiling((double) encodedLength / 8));
				string encoded = "";
				for (int i = 0; i < bytes.Length; i++)
				{
					encoded += Convert.ToString(bytes[i], 2).PadLeft(8, '0');
				}

				encoded = encoded.Substring(0, encodedLength);
				
				// decode the text using the decoding table 
				string decoded = ""; 
				string currentCode = ""; 
				foreach (char c in encoded) 
				{ 
					currentCode += c; 
					if (decodingTable.ContainsKey(currentCode)) 
					{ 
						decoded += decodingTable[currentCode]; 
						currentCode = ""; 
					} 
				} 
 
				return decoded; 
			} 
		}

		public void DecodeBytesFromFileToFile(string inputFilePath, string outputFilePath)
			{
				string decoded = DecodeBytesFromFile(inputFilePath);

				File.WriteAllText(outputFilePath, decoded);

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


