using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CMCore.Models
{
    public class FileSettings
    {
        public string[] AcceptedFileTypes { get; set; }

        public bool IsSupported(string fileName)
        {
            return AcceptedFileTypes.Any(e => e == Path.GetExtension(fileName).ToLower());
        }

        public string GetContentType(string path)
        {
            var types = GetTypesDir();
            var ext = Path.GetExtension(path).ToLowerInvariant();

            return !types.ContainsKey(ext) ? null : types[ext];
        }

        private Dictionary<string, string> GetTypesDir()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".mp3", "application/mp3"},
                {".mp4", "video/mp4"}
            };
        }

    }
}