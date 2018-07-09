using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CMCore.Helpers
{
    public class FileSettings
    {
        public string[] AcceptedFileTypes { get; set; }
        public IDictionary<string, string> ContentTypes { get; set; }

        public bool IsSupported(string fileName)
        {
            return AcceptedFileTypes.Any(e => e == Path.GetExtension(fileName).ToLower());
        }

        public string GetContentType(string path)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();

            return !ContentTypes.ContainsKey(ext) ? null : ContentTypes[ext];
        }

        internal object GetContentType(object pathName)
        {
            throw new NotImplementedException();
        }
    }
}