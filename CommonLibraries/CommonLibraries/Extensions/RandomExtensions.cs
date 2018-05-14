using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonLibraries.Extensions
{
  public static class RandomExtensions
  {
    // Случайный объект, используемый этим методом.
    private static Random Random;

    // Возвращает случайные значения num_items.
    public static List<T> PickRandom<T>(
      this IEnumerable<T> values, int num_values)
    {
      int length = values.Count();

      // Создаем объект Random, если он не существует.
      if (Random == null) Random = new Random();

      // Не превышайте длину массива.
      if (num_values >= length)
        num_values = length - 1;

      // Создаем массив индексов 0 по значениям. Длина - 1.
      var indexes =
        Enumerable.Range(0, length).ToArray();

      // Создаем возвращаемый список.
      var results = new List<T>();

      // Рандомизировать первые индексы num_values.
      for (var i = 0; i < num_values; i++)
      {
        // Выберите случайную запись между i и значениями. Длина - 1.
        var j = Random.Next(i, length);

        // Поменяем значения.
        var temp = indexes[i];
        indexes[i] = indexes[j];
        indexes[j] = temp;

        // Сохранение i-го значения.
        results.Add(values.ElementAt(indexes[i]));
      }

      // Возврат выбранных элементов.
      return results;
    }
  }
}