using System.Collections.Generic;
using System.Threading;
using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using AutoMAT.Common;

namespace AutoMAT.Pipeline
{
    class Pipeline
    {
        readonly List<PipelineMonitor> monitors = new List<PipelineMonitor>();
        readonly FileNotificationQueue queue = new FileNotificationQueue();
        volatile bool running = false;
        Thread worker;

        public Pipeline()
        {
        }

        public void Start()
        {
            queue.Clear();
            foreach (var monitor in monitors)
            {
                monitor.Start();
            }
            running = true;
            worker = new Thread(new ThreadStart(DoConversions));
            worker.Start();
        }

        public void Stop()
        {
            running = false;
            foreach (var monitor in monitors)
            {
                monitor.Stop();
            }
            queue.Clear();
        }

        public void AddAndStart(params PipelineMapping[] mappings)
        {
            foreach (var mapping in mappings)
            {
                if (Directory.Exists(mapping.InputDirectory) && Directory.Exists(mapping.OutputDirectory))
                {
                    var monitor = new PipelineMonitor(queue, mapping);
                    monitors.Add(monitor);
                    monitor.Start();
                }
            }
        }
        
        public void RemoveAndStop(PipelineMapping mapping)
        {
            var monitor = monitors.Where(m => m.Mapping == mapping).SingleOrDefault();
            if (monitor == null)
            {
                throw new InvalidOperationException("Monitor with mapping not found");
            }
            monitor.Stop();
            monitors.Remove(monitor);
        }

        public void Reset()
        {
            Stop();
            monitors.Clear();
        }

        void DoConversions()
        {
            while (running)
            {
                FileChangeEvent evt = queue.Dequeue();
                if (evt != null)
                {
                    switch(evt.ChangeType)
                    {
                        case FileChangeType.CreatedOrUpdated:
                            UpdateMatAsync(evt.FullPath, evt.Source.OutputDirectory, evt.Source.Options);
                            break;

                        case FileChangeType.Renamed:
                            RenameMat(new FileInfo(evt.OldFullPath), new FileInfo(evt.FullPath), evt.Source.OutputDirectory);
                            break;
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        Task UpdateMatAsync(string path, string outputDirectory, ConversionOptions options)
        {
            var inputFile = new FileInfo(path);
            var outputFile = new FileInfo(Path.Combine(outputDirectory, inputFile.BareName() + ".mat"));
            if (inputFile.Exists)
            {
                Directory.CreateDirectory(outputDirectory);
                return Converter.ConvertAsync(options, outputFile, inputFile);
            }
            return null;
        }

        void RenameMat(FileInfo oldSource, FileInfo newSource, string outputDirectory)
        {
            var oldTarget = new FileInfo(Path.Combine(outputDirectory, oldSource.BareName() + ".mat"));
            if (oldTarget.Exists)
            {
                Directory.CreateDirectory(outputDirectory);
                oldTarget.MoveTo(Path.Combine(outputDirectory, newSource.BareName() + ".mat"));
            }
        }
    }
}