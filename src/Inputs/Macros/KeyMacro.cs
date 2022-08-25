using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Inputs.Macros
{
    public class KeyMacro
    {
        /// <summary>
        /// The Key which should get played.
        /// </summary>
        [JsonProperty(PropertyName = "key")]
        public VK Key { get; set; }

        /// <summary>
        /// The time this record will start to play, offset from the global starting point.
        /// </summary>
        [JsonProperty(PropertyName = "startoffset")]
        public double StartOffset { get; set; }

        /// <summary>
        /// The Playtime/Length of this record (in seconds).
        /// </summary>
        [JsonProperty(PropertyName = "playtime")]
        public double Playtime { get; set; }

        public KeyMacro(VK key, double offset, double playTime = 0.0)
        {
            Key = key;
            StartOffset = offset;
            Playtime = playTime;
        }

        Stopwatch watch = new Stopwatch();

        /// <summary>
        /// Start internal stopwatch of the key record.
        /// </summary>
        public void StartRecord()
        {
            watch.Reset();
            watch.Start();
        }

        /// <summary>
        /// Stop the internal stopwatch of the key record.
        /// </summary>
        public void StopRecord()
        {
            Playtime = watch.Elapsed.TotalSeconds;
            watch.Stop();
        }
    }
}
