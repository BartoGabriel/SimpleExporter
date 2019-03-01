using System.IO;

namespace SimpleExporter.Sample.ConsoleApp
{
    internal static class EmbeddedResource
    {
        public static string GetResourceTextFile(string filename)
        {
            string result;

            using (var stream =
                typeof(Program).Assembly.GetManifestResourceStream("SimpleExporter.Sample.ConsoleApp." + filename))
            {
                using (var sr = new StreamReader(stream))
                {
                    result = sr.ReadToEnd();
                }
            }

            return result;
        }
    }
}