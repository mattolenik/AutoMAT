using System.Collections.Generic;
using System.Threading;
using System;
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

        public void AddAndStart(PipelineMapping mapping)
        {
            var monitor = new PipelineMonitor(queue, mapping);
            monitors.Add(monitor);
            monitor.Start();
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

        Task UpdateMatAsync(string path, DirectoryInfo outputDirectory, ConversionOptions options)
        {
            var inputFile = new FileInfo(path);
            var outputFile = new FileInfo(Path.Combine(outputDirectory.FullName, inputFile.BareName() + ".mat"));
            return Converter.ConvertAsync(options, outputFile, inputFile);
        }

        void RenameMat(FileInfo oldSource, FileInfo newSource, DirectoryInfo outputDirectory)
        {
            var oldTarget = new FileInfo(Path.Combine(outputDirectory.FullName, oldSource.BareName() + ".mat"));
            if (oldTarget.Exists)
            {
                oldTarget.MoveTo(Path.Combine(outputDirectory.FullName, newSource.BareName() + ".mat"));
            }
        }
    }
}