using System;
using Microsoft.SPOT;
using System.IO;

namespace NfxLab.MicroFramework.Logging
{
    public class FileAppender : IAppender
    {
        StreamWriter writer;

        public FileAppender(string path)
        {
            try
            {
                var stream = File.Open(path, FileMode.Append);
                writer = new StreamWriter(stream);
            }
            catch (Exception e)
            {
                Debug.Print("Can't initialize FileAppender : " + e);
            }
        }

        public void Write(string message)
        {
            try
            {
                if (writer != null)
                    writer.WriteLine(message);
            }
            catch (Exception e)
            {
                Debug.Print("Error while writing to FileAppender : " + e);
            }
        }
    }
}
