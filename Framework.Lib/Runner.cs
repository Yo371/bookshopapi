using System.Diagnostics;
using System.Reflection;

namespace Framework.Lib;

public class Runner
{
    public static void Main(string[] args)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "cmd",
            Arguments = $"/c dotnet test {Assembly.GetExecutingAssembly().Location}",
            Verb = "runas"
        };

        var process = Process.Start(processStartInfo);
        process.WaitForExit();
    }
}