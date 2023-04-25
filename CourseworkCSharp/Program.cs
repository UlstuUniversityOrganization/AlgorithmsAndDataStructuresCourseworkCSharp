using System;
using System.Net.Http.Headers;

public class GFG<T> where T : IComparable<T>
{
    public class Node
    {
        public T key;
        public Node left, right;

        public Node(T item)
        {
            key = item;
            left = right = null;
        }
    }

    public Node root;

    public GFG()
    {
        root = null;
    }

    public static int countUnits(T value)
    {
        int units = 0;

        if (typeof(T) == typeof(byte))
            units = Convert.ToString(Convert.ToByte(value), 2).Count(c => c == '1');
        if (typeof(T) == typeof(ushort))
            units = Convert.ToString(Convert.ToUInt16(value), 2).Count(c => c == '1');
        if (typeof(T) == typeof(uint))
            units = Convert.ToString(Convert.ToUInt32(value), 2).Count(c => c == '1');
        if (typeof(T) == typeof(long))
            units = Convert.ToString(Convert.ToInt64(value), 2).Count(c => c == '1');

        return units;
    }

    public void insert(T key)
    {
        root = insertRec(root, key);
    }

    public Node insertRec(Node root, T key)
    {

        if (root == null)
        {
            root = new Node(key);
            return root;
        }

        int keyUnits = countUnits(key);
        int rootKeyUnits = countUnits(root.key);

        int comparation = keyUnits.CompareTo(rootKeyUnits);

        if (comparation < 0)
            root.left = insertRec(root.left, key);
        else if (comparation == 0 || comparation > 0)
            root.right = insertRec(root.right, key);

        return root;
    }

    public void inorderRec(Node root, T[] arr, ref int i)
    {
        if (root != null)
        {
            inorderRec(root.left, arr, ref i);

            arr[i] = root.key;
            i++;

            //Console.Write(root.key + " ");
            inorderRec(root.right, arr, ref i);
        }
    }
    public void treeins(T[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            insert(arr[i]);
        }
    }
}



public static class Program
{
    public static void OutputArray<T>(T[] arr) where T : IComparable<T>
    {
        for (int i = 0; i < arr.Length; i++)
            Console.Write(GFG<T>.countUnits(arr[i]) + " ");
        Console.WriteLine();
    }

    public static byte[] GetArrayWithRandomBytes(long sampleSize)
    {
        Random rand = new Random();
        byte[] arr = new byte[sampleSize];
        rand.NextBytes(arr);
        return arr;
    }

    public static ushort[] GetArrayWithRandomUSorts(long sampleSize)
    {
        Random rand = new Random();

        ushort[] shortArray = new ushort[sampleSize];
        for (int i = 0; i < sampleSize; i++)
        {
            byte[] arr = new byte[2];
            rand.NextBytes(arr);
            shortArray[i] = BitConverter.ToUInt16(arr, 0);
        }   
        return shortArray;
    }

    public static uint[] GetArrayWithRandomUInts(long sampleSize)
    {
        Random rand = new Random();

        uint[] uintArray = new uint[sampleSize];
        for (int i = 0; i < sampleSize; i++)
        {
            byte[] arr = new byte[4];
            rand.NextBytes(arr);
            uintArray[i] = BitConverter.ToUInt32(arr, 0);
        }
        return uintArray;
    }

    public static long[] GetArrayWithRandomLongs(long sampleSize)
    {
        Random rand = new Random();

        long[] longArray = new long[sampleSize];
        for (int i = 0; i < sampleSize; i++)
        {
            byte[] arr = new byte[8];
            rand.NextBytes(arr);
            longArray[i] = BitConverter.ToInt64(arr, 0);
        }
        return longArray;
    }

    public static double GetTreeSortTime<T>(long sampleSize, int presorted = 0) where T : IComparable<T>
    { 
        T[] arr = null;
        if (typeof(T) == typeof(byte))
            arr = GetArrayWithRandomBytes(sampleSize) as T[];
        else if (typeof(T) == typeof(ushort))
            arr = GetArrayWithRandomUSorts(sampleSize) as T[];
        else if (typeof(T) == typeof(uint))
            arr = GetArrayWithRandomUInts(sampleSize) as T[];
        else if (typeof(T) == typeof(long))
            arr = GetArrayWithRandomLongs(sampleSize) as T[];


        if (presorted == 1)
            Array.Sort(arr, (T a, T b) => { return GFG<T>.countUnits(a).CompareTo(GFG<T>.countUnits(b)); });
        else if (presorted == -1)
            Array.Sort(arr, (T a, T b) => { return GFG<T>.countUnits(b).CompareTo(GFG<T>.countUnits(a)); });

        var start = DateTime.Now;

        GFG<T> tree = new GFG<T>();
        tree.treeins(arr);
        int x = 0;
        tree.inorderRec(tree.root, arr, ref x);

        var end = DateTime.Now;

        return (end - start).TotalSeconds;
    }

    public static void OuputTreeSortTimeData<T>(int iterationsCount, int increment, int repeatsCount, int presorted = 0) where T : IComparable<T>
    {
        int sampleSize = 0;
        for (int i = 0; i < iterationsCount; i++)
        {
            double avarageTime = 0;
            for (int j = 0; j < repeatsCount; j++)
                avarageTime += GetTreeSortTime<T>(sampleSize, presorted);
            avarageTime /= repeatsCount;

            Console.WriteLine(avarageTime);
            sampleSize += increment;
        }
    }

    public static void Main(String[] args)
    {
        System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";

        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

        OuputTreeSortTimeData<long>(30, 50, 12, -1);
    }
}