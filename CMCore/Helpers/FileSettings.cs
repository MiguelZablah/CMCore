using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace CMCore.Models
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

    }
}