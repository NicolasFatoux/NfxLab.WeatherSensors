using System;
using System.Collections;
using System.Text;

namespace NfxLab.MicroFramework.Logging
{
    class LogFormatter
    {
        StringBuilder builder = new StringBuilder();

        public string Format(LogCategory category, object[] datas)
        {
            // Time
            builder.Append(DateTime.Now);
            builder.Append('\t');

            // Category
            switch (category)
            {
                case LogCategory.Info:
                    builder.Append("INFO");
                    break;
                case LogCategory.Debug:
                    builder.Append("DEBUG");
                    break;
                case LogCategory.Warning:
                    builder.Append("WARNING");
                    break;
                case LogCategory.Error:
                    builder.Append("ERROR");
                    break;
                default:
                    break;
            }

            // Data
            foreach (object data in datas)
            {
                builder.Append('\t');
                Append(data);
            }

            string result = builder.ToString();
            builder.Clear();

            return result;
        }

        void Append(object data)
        {
            if (data == null)
                Append("[NULL]");
            else if (data is Array)
                Append((IEnumerable)data);
            else if (data is Hashtable)
                Append((Hashtable)data);
            else if (data is IEnumerable)
                Append((IEnumerable)data);
            else if (data is Exception)
                Append((Exception)data);
            else
                builder.Append(data);
        }

        void Append(Exception e)
        {
            builder.AppendLine(e.GetType().Name);
            builder.AppendLine(e.Message);
            builder.AppendLine(e.StackTrace);

            if (e.InnerException != null)
            {
                builder.AppendLine();
                Append(e.InnerException);
            }
        }


        void Append(Hashtable table)
        {
            foreach (var key in table.Keys)
            {

                builder.Append(key);
                builder.Append(" : ");

                Append(table[key]);

                builder.AppendLine();
            }
        }

        void Append(IEnumerable collection)
        {
            int i = 0;
            foreach (object value in collection)
            {
                builder.Append(i);
                builder.Append(" : ");
                Append(value);
            }
        }
    }
}
