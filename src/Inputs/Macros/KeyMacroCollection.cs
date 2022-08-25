using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Inputs.Misc;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Inputs.Macros
{
    public class KeyMacroCollection
    {
        public KeyMacroCollection(double length = 0, Dictionary<VK, List<KeyMacro>> collection = null)
        {
            TotalLength = Math.Max(length, 0);
            Collection = collection ?? new Dictionary<VK, List<KeyMacro>>();
        }

        /// <summary>
        /// A unique ID for this collection.
        /// </summary>
        [JsonIgnore]
        public string Uid => Guid.NewGuid().ToString();

        /// <summary>
        /// The total length (in seconds) of the recording.
        /// </summary>
        [JsonProperty(PropertyName = "length")]
        public double TotalLength { get; set; } = 0;

        /// <summary>
        /// The Collection holding all the recorded keys.
        /// </summary>
        [JsonProperty(PropertyName = "collection")]
        public Dictionary<VK, List<KeyMacro>> Collection { get; private set; } = new Dictionary<VK, List<KeyMacro>>();

        #region Functions
        public string ToJson(
#if DEBUG 
            Formatting formatting = Formatting.Indented
#elif !DEBUG // In release, we do None to save space
            Formatting formatting = Formatting.None
#endif 
            )
        {
            return JsonConvert.SerializeObject(this, formatting);
        }

        /// <summary>
        /// Save this Collection to a file.
        /// </summary>
        /// <param name="filePath">The filepath of the file.</param>
        /// <param name="overwrite">Should it override, if one already exists?</param>
        /// <param name="formatting">How should the Json be formatted?</param>
        /// <exception cref="Exception"></exception>
        public void ToFile(string filePath, bool overwrite = true,
#if DEBUG 
            Formatting formatting = Formatting.Indented
#elif !DEBUG // In release, we do None to save space
            Formatting formatting = Formatting.None
#endif 
            )
        {
            if (string.IsNullOrWhiteSpace(filePath) == true)
                throw new Exception($"Invalid FilePath: {filePath}");

            string json = ToJson(formatting);

            if (File.Exists(filePath) == true)
            {
                if (overwrite == true)
                    File.Delete(filePath);
                else
                    throw new Exception($"File '{filePath}' already exists.");
            }

            using (FileStream stream = File.Create(filePath))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(json);
                buffer = Compression.Compress(buffer);

                using (MemoryStream reader = new MemoryStream(buffer))
                {
                    reader.CopyTo(stream);
                }
            }
        }

        /// <summary>
        /// Load a records list from a Json String.
        /// </summary>
        /// <param name="json"></param>
        public static KeyMacroCollection FromString(string json)
        {
            if (string.IsNullOrWhiteSpace(json) == true)
                throw new NullReferenceException($"'json' cannot be null or empty.");

            KeyMacroCollection result = new KeyMacroCollection();

            foreach (var token in JsonConvert.DeserializeObject<JObject>(json))
            {
                if (string.IsNullOrWhiteSpace(token.Key))
                    continue;

                var vk = KeyMapper.MapToVK(token.Key);

                if (vk == VK.NULL)
                    continue;

                if (result.Collection.ContainsKey(vk) == false)
                    result.Collection.Add(vk, new List<KeyMacro>());

                foreach (JObject record in (JArray)token.Value)
                {
                    if (record.ContainsKey("key") == false)
                        continue;

                    if (record.ContainsKey("startoffset") == false)
                        continue;

                    if (record.ContainsKey("playtime") == false)
                        continue;

                    // map it to the object 
                    result.Collection[vk].Add(record.ToObject<KeyMacro>());
                }
            }

            return result;
        }

        /// <summary>
        /// Load a Records-Collection into this Object from a File.
        /// </summary>
        /// <param name="filePath"></param>
        /// <exception cref="Exception"></exception>
        public static KeyMacroCollection FromFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) == true)
                throw new Exception($"Invalid FilePath: {filePath}");

            KeyMacroCollection result = new KeyMacroCollection();
            string text = Encoding.UTF8.GetString(Compression.Decompress(File.ReadAllBytes(filePath)));

            JObject json = JsonConvert.DeserializeObject<JObject>(text);

            result.TotalLength = double.Parse(json.GetValue("length").ToString());

            foreach (var token in json.GetValue("collection") as JObject)
            {
                if (string.IsNullOrWhiteSpace(token.Key))
                    continue;

                var vk = KeyMapper.MapToVK(token.Key);

                if (vk == VK.NULL)
                    continue;

                if (result.Collection.ContainsKey(vk) == false)
                    result.Collection.Add(vk, new List<KeyMacro>());

                foreach (JObject record in (JArray)token.Value)
                {
                    if (record.ContainsKey("key") == false)
                        continue;

                    if (record.ContainsKey("startoffset") == false)
                        continue;

                    if (record.ContainsKey("playtime") == false)
                        continue;

                    // map it to the object 
                    result.Collection[vk].Add(record.ToObject<KeyMacro>());
                }
            }

            return result;
        }
        #endregion
    }
}
