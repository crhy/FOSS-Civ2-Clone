
using System.Collections.Generic;
using System.IO;

namespace Civ2engine
{
    public static class TextFileParser
    {
        public static void ParseFile(string filePath, IFileHandler handler)
        {
            if(string.IsNullOrWhiteSpace(filePath)) return;
            
            using var file = new StreamReader(filePath);
            
            string line;
            string section = null;
            List<string> contents = null;
            var reading = false;
            while ((line = file.ReadLine()) != null)
            {
                if (reading)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith(';'))
                    {
                        handler.ProcessSection(section, contents);
                        reading = false;
                    }
                    else
                    {
                        contents.Add(line);
                    }
                }
                else
                {
                    if (!line.StartsWith('@')) continue;
                    section = line[1..];
                    reading = true;
                    contents = new List<string>();
                }
            }
        }
    }

    public interface IFileHandler
    {
        void ProcessSection(string section, List<string> contents);
    }
}