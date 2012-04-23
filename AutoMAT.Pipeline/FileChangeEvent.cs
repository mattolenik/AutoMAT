using AutoMAT.Common;

namespace AutoMAT.Pipeline
{
    class FileChangeEvent
    {
        public FileChangeType ChangeType { get; set; }

        public string OldFullPath { get; set; }

        public string FullPath { get; set; }

        public PipelineMapping Source { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is FileChangeEvent))
            {
                return false;
            }
            var other = (FileChangeEvent)obj;
            return other.ChangeType == ChangeType && other.FullPath == FullPath && other.OldFullPath == OldFullPath;
        }

        public static bool operator ==(FileChangeEvent x, FileChangeEvent y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }
            if (object.ReferenceEquals(x, null))
            {
                return false;
            }
            return x.Equals(y);
        }

        public static bool operator !=(FileChangeEvent x, FileChangeEvent y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return (OldFullPath + FullPath + ChangeType.ToString()).GetHashCode();
        }

        public override string ToString()
        {
            return "Old name: {0}, new name: {1}, change type: {2}".FormatInvariant(OldFullPath, FullPath, ChangeType.GetName());
        }
    }
}