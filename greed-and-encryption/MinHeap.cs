namespace Greed_and_encryption;

	// A generic minimum heap implementation where T must implement the IComparable interface
	internal class MinHeap<T> where T : IComparable<T>
	{
		private List<T> heap = new List<T>();

		// Returns the number of items in the heap
		public int Count => heap.Count;

		public void Add(T item)
		{
			heap.Add(item);
			int currentIndex = heap.Count - 1;

			// Moves the newly added item to its correct position in the heap
			while (currentIndex > 0)
			{
				int parentIndex = (currentIndex - 1) / 2;


				// If the new item is greater than or equal to its parent, stops moving
				if (heap[currentIndex].CompareTo(heap[parentIndex]) >= 0)
				{
					break;
				}

				// Swaps the new item with its parent
				T temp = heap[currentIndex];
				heap[currentIndex] = heap[parentIndex];
				heap[parentIndex] = temp;

				currentIndex = parentIndex;
			}
		}

		// Removes and returns the item at the root of the heap (the minimum item)
		public T Remove()
		{
			if (heap.Count == 0)
			{
				throw new InvalidOperationException("Heap is empty.");
			}

			// Remove the root item and replace it with the last item in the heap
			T result = heap[0];
			heap[0] = heap[heap.Count - 1];
			heap.RemoveAt(heap.Count - 1);

			int currentIndex = 0;


			// Move down the new root item to its correct position in the heap
			while (true)
			{
				int leftChildIndex = 2 * currentIndex + 1;
				int rightChildIndex = 2 * currentIndex + 2;

				// If the current node has no children, stops moving down
				if (leftChildIndex >= heap.Count)
				{
					break;
				}

				int smallerChildIndex = leftChildIndex;

				// If the right child exists and is smaller than the left child, uses it instead
				if (rightChildIndex < heap.Count && heap[rightChildIndex].CompareTo(heap[leftChildIndex]) < 0)
				{
					smallerChildIndex = rightChildIndex;
				}

				// If the current node is smaller than or equal to its smallest child, stops moving down
				if (heap[currentIndex].CompareTo(heap[smallerChildIndex]) <= 0)
				{
					break;
				}


				// Swaps the current node with its smallest child
				T temp = heap[currentIndex];
				heap[currentIndex] = heap[smallerChildIndex];
				heap[smallerChildIndex] = temp;

				currentIndex = smallerChildIndex;
			}

			return result;
		}
	}

