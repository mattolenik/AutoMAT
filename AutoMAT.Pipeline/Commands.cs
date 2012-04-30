using System.Windows.Input;

namespace AutoMAT.Pipeline
{
    static class Commands
    {
        static readonly DirectoryBrowseCommand directoryBrowse = new DirectoryBrowseCommand();

        static readonly SaveCommand save = new SaveCommand();

        static readonly AddMappingCommand addMapping = new AddMappingCommand();

        static readonly DeleteMappingCommand deleteMapping = new DeleteMappingCommand();

        static readonly RevertCommand revert = new RevertCommand();

        public static ICommand DirectoryBrowse { get { return directoryBrowse; } }

        public static ICommand Save { get { return save; } }

        public static ICommand AddMapping { get { return addMapping; } }

        public static ICommand DeleteMapping { get { return deleteMapping; } }

        public static ICommand Revert { get { return revert; } }
    }
}