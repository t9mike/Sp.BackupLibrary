namespace Vjb.Sp.BackupLibrary.Model
{
    public class LibraryInformation
    {
        public string SiteUrl { get; private set; }
        public string ListName { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string DestinationFolder { get; private set; }

        public LibraryInformation(string siteUrl, string listName, string userName, string password, string destinationFolder)
        {
            SiteUrl = siteUrl;
            ListName = listName;
            UserName = userName;
            Password = password;
            DestinationFolder = destinationFolder;
        }
    }
}