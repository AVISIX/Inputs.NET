using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Inputs.Macros
{
    public delegate void KeyboardPlayerHandler(VK key);
    public delegate void KeyboardPlayerStatusHandler();

    /// <summary>
    /// The playback-status of a KeyboardMacroPlayer-object.
    /// </summary>
    public enum PlaybackStatus
    {
        Stopped = 0,
        Playing = 1,
        Paused = 2,
    }

    public class KeyboardMacroPlayer
    {
        private Thread worker = null;
        private Stopwatch watch = new Stopwatch();

        public KeyboardMacroPlayer(KeyMacroCollection collection = null)
        {
            OnFinished += new KeyboardPlayerStatusHandler(() =>
            {
                Status = PlaybackStatus.Stopped;
            });

            if (collection == null)
                return;

            CurrentCollection = collection;
        }

        #region Properties 
        private KeyMacroCollection _currentCollection = new KeyMacroCollection(0, null);
        /// <summary>
        /// The current collection that is being handled.
        /// </summary>
        public KeyMacroCollection CurrentCollection
        {
            get => _currentCollection;
            set
            {
                if (value == null)
                    return;

                // we dont want it to be override-able during playback 
                if (Status != PlaybackStatus.Stopped)
                    return;

                _currentCollection = value;
            }
        }

        /// <summary>
        /// The current status of the playback.
        /// </summary>
        public PlaybackStatus Status { get; private set; } = PlaybackStatus.Stopped;

        /// <summary>
        /// A callback that will be invoked whenever the Thread executes once.
        /// </summary>
        public Action<KeyboardMacroPlayer> ThreadCallback { get; set; } = null;

        /// <summary>
        /// The amount of time that has elapsed in this playback.
        /// </summary>
        public TimeSpan Elapsed => watch.Elapsed;
        #endregion

        #region Events 
        /// <summary>
        /// This event gets raised when a key gets clicked.
        /// </summary>
        public event KeyboardPlayerHandler OnKeyDown;

        /// <summary>
        /// This event gets raised when a key gets released.
        /// </summary>
        public event KeyboardPlayerHandler OnKeyUp;

        /// <summary>
        /// This event gets raised when the Playback started.
        /// </summary>
        public event KeyboardPlayerStatusHandler OnStarted;

        /// <summary>
        /// This event gets raised when the Playback stopped.
        /// </summary>
        public event KeyboardPlayerStatusHandler OnStopped;

        /// <summary>
        /// This event gets raised when the Playback gets paused
        /// </summary>
        public event KeyboardPlayerStatusHandler OnPaused;

        /// <summary>
        /// This event gets raised when the Playback finished. 
        /// </summary>
        public event KeyboardPlayerStatusHandler OnFinished;
        #endregion

        private void WaitForThreadFinish()
        {
            try
            {
                while (true)
                {
                    if (worker == null)
                        break;

                    if (worker.ThreadState == System.Threading.ThreadState.Stopped)
                        break;

                    if (worker.ThreadState == System.Threading.ThreadState.Aborted)
                        break;

                    if (worker.ThreadState == System.Threading.ThreadState.Unstarted)
                        break;

                    if (worker.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
                        break;

                    Thread.Sleep(10);
                }
            }
            catch { }
        }

        private void Reset()
        {
            watch.Stop();
            watch.Reset();
            heldKeys.Clear();
            indices = null;
        }

        #region Controls
        /// <summary>
        /// Play a KeyRecordCollection in a Thread.
        /// </summary>
        /// <param name="collection"></param>
        public void Play(KeyMacroCollection collection = null)
        {
            if (collection != null)
                CurrentCollection = collection;

            if (Status == PlaybackStatus.Playing)
                return;

            if (CurrentCollection == null)
                return;

            WaitForThreadFinish();

            if (indices == null)
            {
                indices = new Dictionary<VK, int>();

                // Init counters for each key-channel
                foreach (KeyValuePair<VK, List<KeyMacro>> keyChannel in CurrentCollection.Collection)
                {
                    if (indices.ContainsKey(keyChannel.Key) == false)
                        continue;

                    indices.Add(keyChannel.Key, 0);
                }

                OnStarted?.Invoke();
            }

            watch.Start();

            if (worker != null)
                worker = null;

            worker = new Thread(workerFunc);
            worker.IsBackground = true;
            worker.Start();
        }

        /// <summary>
        /// Stop the Playback completely (Also fully resets all internal properties).
        /// When calling Play again, playback will start from the beginning.
        /// </summary>
        public void Stop()
        {
            Status = PlaybackStatus.Stopped;
            WaitForThreadFinish();
            Reset();
            worker = null;
            OnStopped?.Invoke();
        }

        /// <summary>
        /// Pause the playback. Call "Start" again, to resume where you left off.
        /// Call Stop() to fully reset the playback, so you can start from 0.
        /// </summary>
        public void Pause()
        {
            Status = PlaybackStatus.Paused;

            WaitForThreadFinish();
            watch.Stop();

            worker = null;

            OnPaused?.Invoke();
        }
        #endregion


        private Dictionary<VK, int> indices = null;
        private List<VK> heldKeys = new List<VK>();

        private void workerFunc()
        {
            Status = PlaybackStatus.Playing;

            if (CurrentCollection == null)
            {
                OnFinished?.Invoke();
                return;
            }

            foreach (var key in heldKeys)
                OnKeyDown?.Invoke(key);

            try
            {
                // Loop while the status is playing, if its paused it will simply exit the loop. 
                while (Status == PlaybackStatus.Playing)
                {
                    // when its done, just break 
                    if (watch.Elapsed.TotalSeconds > CurrentCollection.TotalLength)
                    {
                        Reset();
                        OnFinished?.Invoke();
                        break;
                    }

                    // check if we need to stop holding the key 
                    foreach (VK key in heldKeys)
                    {
                        KeyMacro currentPlayingRecord = CurrentCollection.Collection[key][indices[key]];

                        if (watch.Elapsed.TotalSeconds > currentPlayingRecord.StartOffset + currentPlayingRecord.Playtime)
                        {
                            OnKeyUp?.Invoke(key);
                            heldKeys.Remove(key);
                            break;
                        }
                    }

                    foreach (KeyValuePair<VK, List<KeyMacro>> keyChannel in CurrentCollection.Collection)
                    {
                        if (heldKeys.Contains(keyChannel.Key) == true)
                            continue;

                        for (int i = 0; i < keyChannel.Value.Count; i++)
                        {
                            KeyMacro record = keyChannel.Value[i];

                            // if the watch hits the start time, play it
                            // also make sure if its already exceeded the playtime, to not play it that time 
                            if (watch.Elapsed.TotalSeconds >= record.StartOffset && watch.Elapsed.TotalSeconds < record.StartOffset + record.Playtime)
                            {
                                OnKeyDown?.Invoke(keyChannel.Key);
                                indices[keyChannel.Key] = i;
                                heldKeys.Add(keyChannel.Key);
                                break;
                            }
                        }
                    }

                    Thread.Sleep(10);

                    if (Status == PlaybackStatus.Playing)
                        ThreadCallback?.Invoke(this);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            // make sure to always take them off 
            foreach (var key in heldKeys)
                OnKeyUp?.Invoke(key);

            Status = PlaybackStatus.Stopped;
        }
    }
}
