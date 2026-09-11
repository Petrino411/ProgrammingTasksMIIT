namespace SortAlgs;

class Program
{
    static void Main()
    {
        const int size = 100_000;

        var random = new Random();

        var source = new int[size];

        for (var i = 0; i < source.Length; i++)
        {
            source[i] = random.Next(-100,100);
        }

        var bubbleArray = (int[])source.Clone();
        var insertionArray = (int[])source.Clone();
        var quickArray = (int[])source.Clone();
        

        var bubbleThread = new Thread(() =>
        {
            var comparisons = BubbleSort(bubbleArray);

            Console.WriteLine(
                $"Пузырьковая сортировка завершена. Сравнений: {comparisons:N0}");
        });

        var insertionThread = new Thread(() =>
        {
            var comparisons = InsertionSort(insertionArray);

            Console.WriteLine(
                $"Сортировка вставками завершена. Сравнений: {comparisons:N0}");
        });

        var quickThread = new Thread(() =>
        {
            var comparisons = QuickSort(quickArray);

            Console.WriteLine(
                $"Быстрая сортировка Хоара завершена. Сравнений: {comparisons:N0}");
        });

        Console.WriteLine($"Размер массива: {size:N0}");
        Console.WriteLine("Запускаем сортировки одновременно...\n");

        bubbleThread.Start();
        insertionThread.Start();
        quickThread.Start();

        bubbleThread.Join();
        insertionThread.Join();
        quickThread.Join();

        Console.WriteLine("\nВсе сортировки завершены.");
    }



    static long BubbleSort(int[] array)
    {
        long comparisons = 0;

        for (var i = 0; i < array.Length - 1; i++)
        {
            var swapped = false;

            for (var j = 0; j < array.Length - 1 - i; j++)
            {
                comparisons++;

                if (array[j] > array[j + 1])
                {
                    (array[j], array[j + 1]) = (array[j + 1], array[j]);
                    swapped = true;
                }
            }
            if (!swapped)
                break;
        }

        return comparisons;
    }

  

    static long InsertionSort(int[] array)
    {
        long comparisons = 0;

        for (var i = 1; i < array.Length; i++)
        {
            var current = array[i];
            var j = i - 1;

            while (j >= 0)
            {
                comparisons++;

                if (array[j] <= current)
                    break;

                array[j + 1] = array[j];
                j--;
            }

            array[j + 1] = current;
        }

        return comparisons;
    }


    static long QuickSort(int[] array)
    {
        long comparisons = 0;

        var sorted = QuickSortRecursive(array, ref comparisons);

        Array.Copy(sorted, array, array.Length);

        return comparisons;
    }
    

    static int[] QuickSortRecursive(int[] array, ref long comparisons)
    {
        if (array.Length <= 1)
            return array;

        
        var pivot = array[array.Length / 2];

        var left = new List<int>();
        var equal = new List<int>();
        var right = new List<int>();

        foreach (var value in array)
        {
            comparisons++;

            if (value < pivot)
            {
                left.Add(value);
            }
            else
            {
                comparisons++;

                if (value > pivot)
                {
                    right.Add(value);
                }
                else
                {
                    equal.Add(value);
                }
            }
        }

        var sortedLeft = QuickSortRecursive(left.ToArray(), ref comparisons);
        var sortedRight = QuickSortRecursive(right.ToArray(), ref comparisons);

        var result = new int[
            sortedLeft.Length +
            equal.Count +
            sortedRight.Length
        ];

        var index = 0;

        foreach (var value in sortedLeft)
            result[index++] = value;

        foreach (var value in equal)
            result[index++] = value;

        foreach (var value in sortedRight)
            result[index++] = value;

        return result;
    }
}