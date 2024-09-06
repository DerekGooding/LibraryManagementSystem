using LibraryManagementSystem.Utils;

namespace LibraryManagementSystem.Models.Books;

internal class EBook : Book
{
    private string _downloadLink = string.Empty;
    public string DownloadLink { get => _downloadLink; protected set => _downloadLink = ValidateURL(value); }

    public EBook(string title, string author, string downloadLink) : base(title, author, BookType.EBook) => DownloadLink = downloadLink;

    public override string ToString() => base.ToString() + $"\n\tdownload link: '{DownloadLink}'";

    private static string ValidateURL(string url)
        => Validator.IsURL(ref url) ? url : throw new ArgumentException("Invalid URL can't be set as a download link of an EBook");

    public void Download() => WriteLine($"Downloaded e-book with title {Title}");
}