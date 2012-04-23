using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoMAT.Pipeline
{
    class FileNotificationQueue
    {
        Dictionary<string, IList<FileChangeEvent>> events = new Dictionary<string, IList<FileChangeEvent>>(StringComparer.OrdinalIgnoreCase);
        object intersection = new object();

        public int Count
        {
            get
            {
                lock (intersection)
                {
                    return events.Values.Count;
                }
            }
        }

        public void Enqueue(FileChangeEvent evt)
        {
            lock (intersection)
            {
                if (evt.ChangeType == FileChangeType.Renamed && events.ContainsKey(evt.OldFullPath))
                {
                    events.Remove(evt.OldFullPath);
                }
                if (!events.ContainsKey(evt.FullPath))
                {
                    events[evt.FullPath] = new List<FileChangeEvent>();
                }
                if (events[evt.FullPath].LastOrDefault() != evt)
                {
                    events[evt.FullPath].Add(evt);
                }
            }
        }

        public FileChangeEvent Dequeue()
        {
            lock (intersection)
            {
                string nextKey = events.Keys.FirstOrDefault();
                if (nextKey == null)
                {
                    return null;
                }
                IList<FileChangeEvent> queue;
                while (true)
                {
                    queue = events[nextKey];
                    if (queue.Count == 0)
                    {
                        events.Remove(nextKey);
                        nextKey = events.Keys.FirstOrDefault();
                        if (nextKey == null)
                        {
                            return null;
                        }
                    }
                    else
                    {
                        var result = queue.First();
                        queue.RemoveAt(0);
                        return result;
                    }
                }
            }
        }

        public FileChangeEvent Peek()
        {
            lock (intersection)
            {
                string nextKey = events.Keys.FirstOrDefault();
                if (nextKey == null)
                {
                    return null;
                }
                IList<FileChangeEvent> queue;
                while (true)
                {
                    queue = events[nextKey];
                    if (queue.Count == 0)
                    {
                        events.Remove(nextKey);
                        nextKey = events.Keys.FirstOrDefault();
                        if (nextKey == null)
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return queue.First();
                    }
                }
            }
        }

        public void Clear()
        {
            lock (intersection)
            {
                events.Clear();
            }
        }
    }
}