namespace Greed_and_encryption;

	public class HuffmanNode : IComparable<HuffmanNode>
	{
		public int Frequency { get; set; }
		public char Symbol { get; set; }
		public HuffmanNode LeftChild { get; set; }
		public HuffmanNode RightChild { get; set; }


		// Constructor for a leaf node
		public HuffmanNode(char symbol, int frequency)
		{
			Symbol = symbol;
			Frequency = frequency;
		}

		// Constructor for a non-leaf node
		public HuffmanNode(HuffmanNode left, HuffmanNode right)
		{
			Frequency = left.Frequency + right.Frequency;
			LeftChild = left;
			RightChild = right;
		}


		// Implementation of the IComparable interface. Compares nodes by frequency.
		public int CompareTo(HuffmanNode other)
		{
			return this.Frequency - other.Frequency;
		}

		// Returns true if this node is a leaf node
		public bool IsLeaf()
		{
			return (LeftChild == null && RightChild == null);
		}
	}

