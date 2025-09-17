using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OopAssignments
{
    // ------------------ 1. PhoneBook with indexer ------------------
    public class PhoneBook
    {
        private Dictionary<string, string> contacts = new();
        public string this[string name]
        {
            get => contacts.ContainsKey(name) ? contacts[name] : "Not Found";
            set => contacts[name] = value;
        }
    }

    // ------------------ 2. WeeklySchedule with indexer ------------------
    public class WeeklySchedule
    {
        private Dictionary<string, string> schedule = new();
        public string this[string day]
        {
            get => schedule.ContainsKey(day) ? schedule[day] : "Free";
            set => schedule[day] = value;
        }
    }

    // ------------------ 3. Matrix with 2D indexer + operations ------------------
    public class Matrix
    {
        private int[,] data;
        public int Rows { get; }
        public int Cols { get; }

        public Matrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            data = new int[rows, cols];
        }

        public int this[int r, int c]
        {
            get => data[r, c];
            set => data[r, c] = value;
        }

        public static Matrix Add(Matrix a, Matrix b)
        {
            if (a.Rows != b.Rows || a.Cols != b.Cols) throw new ArgumentException("Different sizes!");
            Matrix res = new(a.Rows, a.Cols);
            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < a.Cols; j++)
                    res[i, j] = a[i, j] + b[i, j];
            return res;
        }
    }

    // ------------------ 4. Generic Stack<T> ------------------
    public class Stack<T>
    {
        private List<T> items = new();
        public void Push(T item) => items.Add(item);
        public T Pop() { var i = items[^1]; items.RemoveAt(items.Count - 1); return i; }
        public T Peek() => items[^1];
    }

    // ------------------ 5. Generic Pair<T,U> ------------------
    public class Pair<T, U>
    {
        public T First { get; set; }
        public U Second { get; set; }
        public Pair(T first, U second) { First = first; Second = second; }
    }

    // ------------------ 6. Cache with expiration ------------------
    public class Cache<TKey, TValue> where TValue : class
    {
        private class CacheItem
        {
            public TValue Value { get; set; }
            public DateTime Expiration { get; set; }
        }

        private Dictionary<TKey, CacheItem> store = new();

        public TValue this[TKey key]
        {
            get
            {
                if (store.ContainsKey(key) && store[key].Expiration > DateTime.Now)
                    return store[key].Value;
                return null;
            }
            set
            {
                store[key] = new CacheItem { Value = value, Expiration = DateTime.Now.AddSeconds(10) };
            }
        }
    }

    // ------------------ 7. ConvertList<T,U> ------------------
    public static class ListConverter
    {
        public static List<U> ConvertList<T, U>(List<T> list, Func<T, U> converter)
            => list.Select(converter).ToList();
    }

    // ------------------ 8. Repository<T> with CRUD + IEntity ------------------
    public interface IEntity
    {
        int Id { get; set; }
    }

    public class Repository<T> where T : IEntity
    {
        private List<T> items = new();
        public void Add(T item) => items.Add(item);
        public IEnumerable<T> GetAll() => items;
        public T GetById(int id) => items.FirstOrDefault(i => i.Id == id);
        public void Update(T entity)
        {
            var old = GetById(entity.Id);
            if (old != null)
            {
                items.Remove(old);
                items.Add(entity);
            }
        }
        public void Delete(int id) => items.RemoveAll(i => i.Id == id);
    }

    // ------------------ 9. ContactManager with Dictionary ------------------
    public class ContactManager
    {
        private Dictionary<string, string> contacts = new();
        public void AddContact(string name, string phone) => contacts[name] = phone;
        public void RemoveContact(string name) => contacts.Remove(name);
        public string Search(string name) => contacts.ContainsKey(name) ? contacts[name] : "Not Found";
    }

    // ------------------ 10. Shopping Cart ------------------
    public class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
    }

    public class Cart
    {
        private List<Product> products = new();
        private Dictionary<Product, int> quantities = new();
        private HashSet<string> discounts = new();

        public void Add(Product p, int qty = 1)
        {
            products.Add(p);
            if (quantities.ContainsKey(p)) quantities[p] += qty;
            else quantities[p] = qty;
        }

        public void AddDiscount(string code) => discounts.Add(code);

        public double GetTotal()
        {
            double total = products.Sum(p => p.Price);
            if (discounts.Contains("DISC10")) total *= 0.9;
            return total;
        }
    }

    // ------------------ 11. Nullable Average ------------------
    public static class NullableHelper
    {
        public static double Average(List<int?> list)
        {
            var filtered = list.Where(x => x.HasValue).Select(x => x.Value);
            return filtered.Any() ? filtered.Average() : 0;
        }
    }

    // ------------------ 12. Person class with nullable ------------------
    public class Person : IEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; } // nullable
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public override string ToString() => $"{FirstName} {MiddleName ?? ""} {LastName}";
    }

    // ------------------ 13. Extension Methods ------------------
    public static class IntExtensions
    {
        public static bool IsEven(this int x) => x % 2 == 0;
        public static bool IsOdd(this int x) => x % 2 != 0;
        public static bool IsPrime(this int n)
        {
            if (n < 2) return false;
            for (int i = 2; i <= Math.Sqrt(n); i++) if (n % i == 0) return false;
            return true;
        }
        public static string ToRoman(this int number)
        {
            var map = new Dictionary<int, string>
            {
                {1000,"M"},{900,"CM"},{500,"D"},{400,"CD"},
                {100,"C"},{90,"XC"},{50,"L"},{40,"XL"},
                {10,"X"},{9,"IX"},{5,"V"},{4,"IV"},{1,"I"}
            };
            string result = "";
            foreach (var kvp in map)
            {
                while (number >= kvp.Key)
                {
                    result += kvp.Value;
                    number -= kvp.Key;
                }
            }
            return result;
        }
        public static long Factorial(this int n) => n <= 1 ? 1 : n * Factorial(n - 1);
    }

    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt) => dt.AddDays(-(int)dt.DayOfWeek);
        public static DateTime EndOfWeek(this DateTime dt) => dt.StartOfWeek().AddDays(6);
        public static int Age(this DateTime dob) => (int)((DateTime.Now - dob).TotalDays / 365.25);
    }

    public static class CollectionExtensions
    {
        public static IEnumerable<List<T>> Batch<T>(this IEnumerable<T> source, int size)
        {
            var batch = new List<T>();
            foreach (var item in source)
            {
                batch.Add(item);
                if (batch.Count == size)
                {
                    yield return new List<T>(batch);
                    batch.Clear();
                }
            }
            if (batch.Count > 0) yield return batch;
        }
        public static IEnumerable<T> FindDuplicates<T>(this IEnumerable<T> source)
            => source.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key);
    }

    // ------------------ 14. Delegates & Calculator ------------------
    public delegate int Operation(int a, int b);
    public class Calculator
    {
        public int Execute(Operation op, int a, int b) => op(a, b);
    }

    // ------------------ 15. Notifier ------------------
    public delegate void Notify(string msg);
    public class Notifier
    {
        public Notify notify;
        public void Send(string msg) => notify?.Invoke(msg);
    }

    // ------------------ 16. Plugin System ------------------
    public delegate void PluginAction();
    public class PluginManager
    {
        public List<PluginAction> Plugins { get; } = new();
        public void RunAll() { foreach (var p in Plugins) p(); }
    }

    // ------------------ 17. Pipeline Processing ------------------
    public class Pipeline<T>
    {
        private List<Func<T, T>> steps = new();
        public void AddStep(Func<T, T> step) => steps.Add(step);
        public T Execute(T input) => steps.Aggregate(input, (acc, f) => f(acc));
    }

    // ------------------ 18. Grades with Lambdas ------------------
    public static class GradeHelper
    {
        public static double Average(List<int> grades) => grades.Average();
        public static int MaxGrade(List<int> grades) => grades.Max();
    }

    // ------------------ 19. Validator with lambda ------------------
    public static class Validator
    {
        public static bool Validate(string input, Func<string, bool> rule) => rule(input);
    }

    // ------------------ 20. Timer ------------------
    public class TimerEvent
    {
        public event Action Tick;
        public event Action Completed;
        public void Start(int interval, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Thread.Sleep(interval);
                Tick?.Invoke();
            }
            Completed?.Invoke();
        }
    }

    // ------------------ 21. Async File Helper ------------------
    public static class FileHelper
    {
        public static async Task WriteAsync(string path, string content)
        {
            try { await System.IO.File.WriteAllTextAsync(path, content); }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        }
        public static async Task<string> ReadAsync(string path)
        {
            try { return await System.IO.File.ReadAllTextAsync(path); }
            catch (Exception ex) { return $"Error: {ex.Message}"; }
        }
    }

    // ------------------ 22. Async API Service combine results ------------------
    public class ApiService
    {
        public async Task<string> GetDataAsync(string name)
        {
            await Task.Delay(300);
            return $"Data from {name}";
        }

        public async Task<string> GetCombinedAsync()
        {
            var t1 = GetDataAsync("API1");
            var t2 = GetDataAsync("API2");
            await Task.WhenAll(t1, t2);
            return $"{t1.Result} + {t2.Result}";
        }
    }

    // ------------------ 23. Background Queue ------------------
    public class BackgroundQueue
    {
        private Queue<Func<Task>> tasks = new();
        public void Enqueue(Func<Task> task) => tasks.Enqueue(task);
        public async Task RunAllAsync()
        {
            while (tasks.Count > 0)
            {
                try { await tasks.Dequeue()(); }
                catch { /* retry logic */ }
            }
        }
    }

    // ------------------ 24. Async Downloader multi ------------------
    public class Downloader
    {
        public async Task DownloadAsync(string url)
        {
            await Task.Delay(300);
            Console.WriteLine($"Downloaded from {url}");
        }
        public async Task DownloadAllAsync(List<string> urls)
        {
            var tasks = urls.Select(DownloadAsync);
            await Task.WhenAll(tasks);
        }
    }

    // ------------------ 25. Async EmailSender multi ------------------
    public class EmailSender
    {
        public async Task SendAsync(string email, string message)
        {
            int retries = 3;
            for (int i = 0; i < retries; i++)
            {
                try
                {
                    await Task.Delay(200);
                    Console.WriteLine($"Email to {email}: {message}");
                    return;
                }
                catch { if (i == retries - 1) throw; }
            }
        }

        public async Task SendAllAsync(List<(string, string)> emails)
        {
            var tasks = emails.Select(e => SendAsync(e.Item1, e.Item2));
            await Task.WhenAll(tasks);
        }
    }

    // ------------------ 26. Thread Safe Counter ------------------
    public class Counter
    {
        private int count;
        private object locker = new();
        public void Increment() { lock (locker) count++; }
        public void Decrement() { lock (locker) count--; }
        public int Value => count;
    }

    // ------------------ 27. Transaction Rollback ------------------
    public class Transaction
    {
        private List<Action> rollbackActions = new();
        public void Do(Action action, Action rollback)
        {
            try
            {
                action();
                rollbackActions.Add(rollback);
            }
            catch
            {
                Rollback();
            }
        }
        public void Rollback() { foreach (var r in rollbackActions) r(); rollbackActions.Clear(); }
    }

    // ------------------ 28. Validation Framework with Custom Exceptions ------------------
    public interface IValidator<T>
    {
        void Validate(T entity);
    }
    public class NameValidator : IValidator<Person>
    {
        public void Validate(Person p)
        {
            if (string.IsNullOrWhiteSpace(p.FirstName)) throw new InvalidNameException("Name required");
        }
    }

    public class InvalidNameException : Exception
    {
        public InvalidNameException(string msg) : base(msg) { }
    }

    // ------------------ 30. Main Program ------------------
    class Program
    {
        static async Task Main()
        {
            var phoneBook = new PhoneBook();
            phoneBook["Ali"] = "0100";
            Console.WriteLine(phoneBook["Ali"]);

            var repo = new Repository<Person>();
            repo.Add(new Person { Id = 1, FirstName = "Nayira", LastName = "Ali" });

            var cache = new Cache<string, string>();
            cache["x"] = "100";
            Console.WriteLine(cache["x"]);

            var cart = new Cart();
            cart.Add(new Product { Name = "Pen", Price = 10 }, 2);
            cart.AddDiscount("DISC10");
            Console.WriteLine(cart.GetTotal());

            var avg = NullableHelper.Average(new List<int?> { 1, 2, null, 3 });
            Console.WriteLine($"Nullable Avg: {avg}");

            var api = new ApiService();
            Console.WriteLine(await api.GetCombinedAsync());

            var downloader = new Downloader();
            await downloader.DownloadAllAsync(new List<string> { "http://1.com", "http://2.com" });

            var sender = new EmailSender();
            await sender.SendAllAsync(new List<(string, string)> { ("a@test.com", "Hello"), ("b@test.com", "Hi") });
        }
    }
}
