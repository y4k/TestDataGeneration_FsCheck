using FsCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataGeneration
{
    public static class Program
    {
        public static void Main()
        {
            Func<int[], bool> rev = xs => xs.Reverse().Reverse().SequenceEqual(xs);
            Prop.ForAll(Gen.ListOf(100, Gen.Choose(100, 200)).ToArbitrary(), x => x.Reverse().Reverse().SequenceEqual(x)).VerboseCheck();

            Gen.ListOf(10, Gen.Choose(0, 1000));

            Gen.Choose(0, 100).Select(x => Gen.Choose(x, x + 100));

            var l = Gen.Choose(0, 100).Sample(10, 1).ToList();
            l.Sort();

            foreach (var x in l)
            {
                Console.WriteLine(x);
            }



            Prop.ForAll<int>(a => new Func<bool>(() => 1 / a == 1 / a).When(a != 0)).VerboseCheck();

            var property = Prop.ForAll(MyClass.MyClasses(), mc => { return mc != null; });
            property.VerboseCheck();

            Prop.ForAll(MyClass.Choose(Enumerable.Range(0, 100).ToArray()).ToArbitrary(), x => x > 0).VerboseCheck();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }

        public class MyClass
        {
            public int Size { get; set; }

            public MyClass()
            {

            }

            public static Arbitrary<MyClass> MyClasses()
            {
                return Arb.Generate<PositiveInt>().Select(item => new MyClass() { Size = item.Item }).ToArbitrary();
            }

            public static Gen<TData> Choose<TData>(TData[] t)
            {
                return Gen.Choose(0, t.Length - 1).Select(i => t[i]);
            }

            public static Gen<bool> OneOf<TData>()
            {
                return Gen.OneOf(Gen.Constant(true), Gen.Constant(false));
            }

            public static Gen<MyClass> OneOf()
            {
                return Gen.OneOf( Gen.Fresh(() => { return new MyClass { Size = 1 }; }), Gen.Fresh(() => { return new MyClass { Size = 2 }; }) );
            }
        }
    }
}
