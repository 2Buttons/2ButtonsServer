using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CommonLibraries.Extensions
{
  public class RandomizerExtension
  {
    //public static IList<T> Randomize<T>(this IList<T> items)
    //{
    //  Random rand = new Random();

    //  //For each spot in the array, pick
    //  // a random item to swap into that spot.
    //  for (int i = 0; i < items.Count - 1; i++)
    //  {
    //    int j = rand.Next(i, items.Count);
    //    T temp = items[i];
    //    items[i] = items[j];
    //    items[j] = temp;
    //  }
    //}


    public static void Shuffle<T>(List<T> list)
    {
      Random rand = new Random();

      for (int i = list.Count - 1; i >= 1; i--)
      {
        int j = rand.Next(i + 1);

        T tmp = list[j];
        list[j] = list[i];
        list[i] = tmp;
      }
    }
  }
}
