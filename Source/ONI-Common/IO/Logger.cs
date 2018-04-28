namespace ONI_Common.IO
{
    using System;
    using System.IO;

    public class Logger
    {
        private readonly string _fileName;

        public Logger(string fileName)
        {
            this._fileName = fileName;
        }

        public void Log(string message)
        {
            IOHelper.EnsureDirectoryExists(Paths.LogsPath);

            string path = Paths.LogsPath + Path.DirectorySeparatorChar + this._fileName;

            using (StreamWriter writer = new StreamWriter(path, true))
            {
                DateTime now = System.DateTime.Now;

                writer.WriteLine($"[{now.ToShortDateString()}, {now.TimeOfDay}] {message}\r\n");
                writer.Close();
            }
        }

        public void Log(Exception exception)
        {
            this.Log($"{exception?.Message}\n{exception?.StackTrace}");
        }

        // public void LogProperties(object target)
        // {
        // var builder = new StringBuilder();
        // foreach (var property in target.GetType().GetProperties())
        // {
        // builder.Append(property.Name);
        // builder.Append(":");
        // builder.Append(property.GetValue(target));
        // builder.Append("\n");
        // }
        // Log(builder.ToString());
    }
}