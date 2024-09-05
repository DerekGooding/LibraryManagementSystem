using LibraryManagementSystem.Utils;
using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Models.Books;

// uniqueness = title + author + type
internal abstract class Book
{
    private const int BOOK_ID_LENGTH = 8;

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
        protected set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException($"Trying to set invalid book's title = '{value}'");
            _title = value.Trim().ToLower();
        }
    }

    public string Author
    {
        get => _author;
        protected set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException($"Trying to set invalid book's author name = '{value}'");
            _author = value.Trim();
        }
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
    public override bool Equals(object obj)
    {
        if (obj is Book otherBook)
        {
            // check if both book's title, author and type are same or not
            return otherBook.Title.Equals(Title) && otherBook.Author.Equals(Author) && otherBook.Type == Type;
        }
        return false;
    }

    public override int GetHashCode()
    {
        int hashTitle = Title.GetHashCode();
        int hashAuthor = Author.GetHashCode();
        int hashType = Type.GetHashCode();

        return hashTitle ^ hashAuthor ^ (hashType * 17);
    }

    public static bool IsValidBookId(string bookId)
    {
        if (string.IsNullOrWhiteSpace(bookId)) return false;

        bookId = bookId.Trim();

        return bookId.Length == BOOK_ID_LENGTH;
    }

    public static bool SelectBookTypeUsingMenuSelector(out BookType result, string message = "Use the arrow keys to navigate and press Enter to select book type:")
    {
        string selectedBookTypeInput = MenuSelector.SelectOption(BookTypeNames, message);
        bool isValidSelectedBookType = Enum.TryParse(selectedBookTypeInput, false, out BookType validBookType);
        result = validBookType;
        return isValidSelectedBookType;
    }

    public void BorrowBook() => IsBorrowed = true;

    public void ReturnBook() => IsBorrowed = false;
}