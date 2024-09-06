using LibraryManagementSystem.Utils;

namespace LibraryManagementSystem.Models.Books;

// uniqueness = title + author + type
internal abstract class Book
{
    public const int BOOK_ID_LENGTH = 8;

    public enum BookType
    {
        Physical,
        EBook,
    }

    private string _title = string.Empty;
    private string _author = string.Empty;

    public static List<string> BookTypeNames = [.. Enum.GetNames(typeof(BookType))];
    public string BookId { get; } = CustomUtils.GenerateUniqueID(0, BOOK_ID_LENGTH);

    public string Title
    {
        get => _title;
        protected set => _title = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentNullException($"Trying to set invalid book's title = '{value}'")
                : value.Trim().ToLower();
    }

    public string Author
    {
        get => _author;
        protected set => _author = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentNullException($"Trying to set invalid book's author name = '{value}'")
                : value.Trim();
    }

    public string ISBN { get; } = CustomUtils.GenerateUniqueID();
    public bool IsBorrowed { get; protected set; }
    public BookType Type { get; protected set; }

    protected Book(string title, string author, BookType type)
    {
        Title = title;
        Author = author;
        Type = type;
    }

    public override string ToString() => $"Book details\n\tbook id: '{BookId}',\n\ttitle: '{Title}'," +
            $"\n\tauthor: '{Author}',\n\tISBN: '{ISBN}',\n\tisBorrowed: {IsBorrowed}," +
            $"\n\ttype: {Type}";

    // uniqueness = title + author + type
    public override bool Equals(object obj) => obj is Book otherBook && otherBook.Title.Equals(Title) && otherBook.Author.Equals(Author) && otherBook.Type == Type;

    public override int GetHashCode()
    {
        int hashTitle = Title.GetHashCode();
        int hashAuthor = Author.GetHashCode();
        int hashType = Type.GetHashCode();

        return hashTitle ^ hashAuthor ^ (hashType * 17);
    }

    public static bool IsValidId(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return false;

        id = id.Trim();

        return id.Length == BOOK_ID_LENGTH;
    }

    public static BookType? SelectType(string message = "Use the arrow keys to navigate and press Enter to select book type:")
    {
        string input = MenuSelector.SelectOption(BookTypeNames, message);
        bool isValid = Enum.TryParse(input, false, out BookType bookType);
        return isValid ? bookType : null;
    }

    public void Borrow() => IsBorrowed = true;

    public void Return() => IsBorrowed = false;
}