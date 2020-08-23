using System.IO;

namespace Demo
{
    class UsingDeclaration
    {
        static void Sample1()
        {
            using Stream source = File.OpenRead("test.txt");
            using Stream target = new MemoryStream();
            source.CopyTo(target);

            source.Position = 0;

            // etc...
        }

        static void Sample2()
        {
            using Stream source = File.OpenRead("test.txt");
            using Stream target = new MemoryStream();

            source.CopyTo(target);
            source.Position = 0;
            // etc...
        }
    }
}
