using System.Text;

namespace ShoppingCartAPI.Helper
{
    public class General
    {
        public static string FolderPath = "Log";
        public static void WriteLogInTextFile(string message)
        {
            Directory.CreateDirectory(FolderPath);
            string filePath = Path.Combine(FolderPath, $"log-{DateTime.Now.ToString("yyyy-MM-dd - hh tt")}.log");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(message);
            File.AppendAllText(filePath, sb.ToString());
        }

    }
}
