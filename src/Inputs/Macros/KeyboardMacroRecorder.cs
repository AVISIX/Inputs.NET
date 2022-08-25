using Inputs.Hooks;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Inputs.Macros
{
    public class KeyboardMacroRecorder
    {
        #region Public Properties
        /// <summary>
        /// The current Record Collection.
        /// </summary>
        public KeyMacroCollection RecordCollection { get; private set; } = new KeyMacroCollection(0, null);

        /// <summary>
        /// Is a record currently running?
        /// </summary>
        public bool IsRecording { get; private set; } = false;

        /// <summary>
        /// The time that has elapsed every since the recording has started.
        /// </summary>
        public TimeSpan Elapsed => watch.Elapsed;
        #endregion

        #region Private Properties
        private List<KeyMacro> RunningRecords = new List<KeyMacro>();
        private KeyboardHook hook = new KeyboardHook();
        private Stopwatch watch = new Stopwatch();
        #endregion

        public KeyboardMacroRecorder()
        {
            hook.OnKeyPressed += Hook_OnKeyPressed;
            hook.OnKeyReleased += Hook_OnKeyUnpressed;
        }

        /// <summary>
        /// Start recording.
        /// </summary>
        public void Start()
        {
            watch.Reset();
            watch.Start();
            hook.Hook();
            RecordCollection.Collection.Clear();
            RecordCollection.TotalLength = 0;
            RunningRecords.Clear();
            IsRecording = true;
        }

        /// <summary>
        /// Stop recording.
        /// </summary>
        public void Stop()
        {
            watch.Stop();
            hook.Unhook();

            // save any record that wasnt finished 
            foreach (var record in RunningRecords)
            {
                if (RecordCollection.Collection.ContainsKey(record.Key) == false)
                    RecordCollection.Collection.Add(record.Key, new List<KeyMacro>());

                RecordCollection.Collection[record.Key].Add(record);
            }

            // save the length of the recording 
            RecordCollection.TotalLength = watch.Elapsed.TotalSeconds;

            IsRecording = false;
        }

        /// <summary>
        /// Save the Collection to a file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <exception cref="Exception"></exception>
        public void SaveToFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) == true)
                throw new Exception("Invalid FilePath.");

            RecordCollection.ToFile(filePath, true);
        }

        /// <summary>
        /// Load a collection from a filepath.
        /// </summary>
        /// <param name="filePath"></param>
        /// <exception cref="Exception"></exception>
        public void LoadFromFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) == true)
                throw new Exception("Invalid FilePath.");

            if (File.Exists(filePath) == false)
                throw new FileNotFoundException($"The file '{filePath}' doesn't exist.");

            RecordCollection = KeyMacroCollection.FromFile(filePath);
        }

        #region Core
        private void Hook_OnKeyPressed(VK vk, bool isInjected)
        {
            if (vk == VK.NULL)
                return;

            // if the key is already being recorded we dont care 
            if (RunningRecords.Any((KeyMacro r) => r.Key == vk))
                return;

            KeyMacro record = new KeyMacro(vk, watch.Elapsed.TotalSeconds);

            record.StartRecord();

            RunningRecords.Add(record);
        }

        private void Hook_OnKeyUnpressed(VK vk, bool isInjected)
        {
            if (vk == VK.NULL)
                return;

            // only handle this for key records that are currently running 
            if (RunningRecords.Any((KeyMacro record) => record.Key == vk) == false)
                return;

            KeyMacro current = RunningRecords.Where((KeyMacro record) => record.Key == vk).FirstOrDefault();

            current.StopRecord();

            if (RecordCollection.Collection.ContainsKey(vk) == false)
                RecordCollection.Collection.Add(vk, new List<KeyMacro>());

            RecordCollection.Collection[vk].Add(current);
            RunningRecords.Remove(current);
        }
        #endregion
    }
}
