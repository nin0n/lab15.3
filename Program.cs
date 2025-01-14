namespace lab15_3
{
    using System;
    using System.Threading.Tasks;

    public sealed class SingleRandomizer
    {
        // Поле для единственного экземпляра класса, созданного ленивым способом.
        private static readonly Lazy<SingleRandomizer> _instance =
            new Lazy<SingleRandomizer>(() => new SingleRandomizer());

        // Экземпляр генератора случайных чисел
        private readonly Random _random;

        private SingleRandomizer()
        {
            _random = new Random(); // Инициализация генератора случайных чисел.
        }

        public static SingleRandomizer Instance => _instance.Value;

        public int GetNext(int minValue, int maxValue)
        {
            lock (_random) // Блокировка для обеспечения потокобезопасности.
            {
                return _random.Next(minValue, maxValue);
            }
        }

        public double GetNextDouble()
        {
            lock (_random) // Блокировка для обеспечения потокобезопасности.
            {
                return _random.NextDouble();
            }
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Пример работы SingleRandomizer:");

            // Генерация случайных чисел из одного потока.
            for (int i = 0; i < 5; i++)
            {
                int randomValue = SingleRandomizer.Instance.GetNext(1, 100);
                Console.WriteLine($"Случайное число (однопоточный): {randomValue}");
            }

            Console.WriteLine("\nГенерация случайных чисел из нескольких потоков:");

            // Генерация случайных чисел из нескольких потоков.
            Parallel.For(0, 5, _ =>
            {
                int randomValue = SingleRandomizer.Instance.GetNext(1, 100);
                Console.WriteLine($"Случайное число (многопоточный): {randomValue}");
            });
        }
    }
}
