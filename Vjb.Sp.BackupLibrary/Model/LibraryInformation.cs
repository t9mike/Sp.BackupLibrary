using Vjb.Sp.BackupLibrary.SpClient;
namespace Vjb.Sp.BackupLibrary.Model
{
    public class LibraryInformation
    {
        public string ListName { get; private set; }
        public string DestinationFolder { get; private set; }
        public SharepointClient SharepointClient { get; set; }

        public LibraryInformation(string listName, string destinationFolder, SharepointClient sharePointClient)
        {
            ListName = listName;
            DestinationFolder = destinationFolder;
            SharepointClient = sharePointClient;
        }
    }
}