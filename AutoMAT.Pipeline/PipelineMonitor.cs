using System.IO;

namespace AutoMAT.Pipeline
{
    class PipelineMonitor
    {
        FileSystemWatcher watcher;

        FileNotificationQueue queue;

        PipelineMapping mapping;

        public PipelineMonitor(FileNotificationQueue queue, PipelineMapping mapping)
        {
            this.queue = queue;
            this.mapping = mapping;

            watcher = new FileSystemWatcher(mapping.InputDirectory.FullName);
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Changed += new FileSystemEventHandler(CreatedOrChanged);
            watcher.Renamed += new RenamedEventHandler(Renamed);
            watcher.Created += new FileSystemEventHandler(CreatedOrChanged);
        }

        public void Start()
        {
            watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
        }

        void CreatedOrChanged(object obj, FileSystemEventArgs e)
        {
            if (IsAcceptedChange(e.Name))
            {
                queue.Enqueue(new FileChangeEvent { FullPath = e.FullPath, ChangeType = FileChangeType.CreatedOrUpdated, Source = mapping });
            }
        }

        void Renamed(object obj, RenamedEventArgs e)
        {
            if (IsAcceptedChange(e.Name))
            {
                queue.Enqueue(new FileChangeEvent { FullPath = e.FullPath, OldFullPath = e.OldFullPath, ChangeType = FileChangeType.Renamed, Source = mapping });
            }
        }

        bool IsAcceptedChange(string fileName)
        {
            switch (Path.GetExtension(fileName).ToLowerInvariant())
            {
                case ".psd":
                    break;
                case ".png":
                    break;
                case ".bmp":
                    break;
                case ".jpg":
                    break;
                case ".jpeg":
                    break;
                case ".tga":
                    break;
                case ".tif":
                    break;
                case ".psp":
                    break;
                case ".dds":
                    break;
                case ".iff":
                    break;
                case ".gif":
                    break;
                case ".jpe":
                    break;
                case ".jp2":
                    break;
                case ".pcx":
                    break;
                case ".raw":
                    break;
                default:
                    return false;
            }
            return true;
        }
    }
}