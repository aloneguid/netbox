namespace NetBox.FileFormats.Ini {
    using global::System;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Linq;
    using global::System.Text;

    class StructuredIniFile {
        private const string _sectionBegin = "[";
        private const string _sectionEnd = "]";
        private static readonly char[] _sectionTrims = { '[', ']' };

        private readonly IniSection _globalSection;
        private readonly List<IniSection> _sections = new List<IniSection>();
        private readonly Dictionary<string, IniKeyValue> _fullKeyNameToValue = new Dictionary<string, IniKeyValue>(StringComparer.InvariantCultureIgnoreCase);

        public StructuredIniFile() {
            _globalSection = new IniSection(null);
            _sections.Add(_globalSection);
        }

        public string? this[string key] {
            get {
                if(key == null)
                    return null;

                return !_fullKeyNameToValue.TryGetValue(key, out IniKeyValue? value) ? null : value.Value;
            }
            set {
                if(key == null)
                    return;

                IniSection.SplitKey(key, out string? sectionName, out string keyName);
                IniSection? section = sectionName == null
                   ? _globalSection
                   : _sections.FirstOrDefault(s => s.Name == sectionName);
                if(section == null) {
                    section = new IniSection(sectionName);
                    _sections.Add(section);
                }
                IniKeyValue? ikv = section.Set(keyName, value);

                //update the local cache
                if(ikv != null) {
                    if(value == null) {
                        _fullKeyNameToValue.Remove(key);
                    } else {
                        _fullKeyNameToValue[key] = ikv;
                    }
                }
            }
        }

        public static StructuredIniFile FromString(string content, bool parseInlineComments = true) {
            using(Stream input = new MemoryStream(Encoding.UTF8.GetBytes(content))) {
                return FromStream(input, parseInlineComments);
            }
        }

        public static StructuredIniFile FromStream(Stream inputStream, bool parseInlineComments = true) {
            if(inputStream == null)
                throw new ArgumentNullException(nameof(inputStream));

            var file = new StructuredIniFile();

            using(var reader = new StreamReader(inputStream)) {
                IniSection section = file._globalSection;

                string? line;
                while((line = reader.ReadLine()) != null) {
                    line = line.Trim();

                    if(line.StartsWith(_sectionBegin)) {
                        //start new section
                        line = line.Trim();
                        section = new IniSection(line);
                        file._sections.Add(section);
                    } else if(line.StartsWith(IniComment.CommentSeparator)) {
                        //whole line is a comment
                        string comment = line.Substring(1).Trim();
                        section.Add(new IniComment(comment));
                    } else {
                        IniKeyValue? ikv = IniKeyValue.FromLine(line, parseInlineComments);
                        if(ikv == null)
                            continue;

                        section.Add(ikv);
                        string fullKey = section.Name == null
                           ? ikv.Key
                           : $"{section.Name}{IniSection.SectionKeySeparator}{ikv.Key}";
                        file._fullKeyNameToValue[fullKey] = ikv;

                    }
                }
            }

            return file;
        }

        public void WriteTo(Stream outputStream) {
            if(outputStream == null)
                throw new ArgumentNullException(nameof(outputStream));

            using(var writer = new StreamWriter(outputStream)) {
                foreach(IniSection section in _sections) {
                    if(section.Name != null) {
                        writer.WriteLine();
                        writer.WriteLine($"{_sectionBegin}{section.Name}{_sectionEnd}");
                    }

                    section.WriteTo(writer);
                }
            }
        }

        //private static 
    }
}