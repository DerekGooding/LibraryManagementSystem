using LibraryManagementSystem.Models.Menus;

namespace LibraryManagementSystem.Services;

internal static class MenuService
{
    private const string ADD_BOOK_ACTION_TEXT = "[ACTION]: Add Book";
    private const string BORROW_BOOK_ACTION_TEXT = "[ACTION]: Borrow book";
    private const string RETURN_BOOK_ACTION_TEXT = "[ACTION]: Return book";
    private const string REGISTER_MEMBER_TEXT = "[ACTION]: Register Member";

    public static void Start()
    {
        LibManagementSystem library = new();

        WriteLine("============= WELCOME TO LIBRARY MANAGEMENT SYSTEM ==============\n");

        while (true)
        {
            Menu menu = new();
            WriteLine("[MENU]:");
            menu.Add(new("-2", "Clear Console", Clear));
            menu.Add(new("-1", "Exit", () => Environment.Exit(0)));
            menu.Add(new("0", "See Menu", () => { }));
            menu.Add(new("1", "Add Book", () => { WriteLine($"\n{ADD_BOOK_ACTION_TEXT}"); library.AddBook(); }));
            menu.Add(new("2", "Borrow Book", () => { WriteLine($"\n{BORROW_BOOK_ACTION_TEXT}"); library.BorrowBook(); }));
            menu.Add(new("3", "Return Book", () => { WriteLine($"\n{RETURN_BOOK_ACTION_TEXT}"); library.ReturnBook(); }));
            menu.Add(new("4", "Total Physical Books", () => WriteAndWait($"Total physical books: {library.PhysicalBooks.Count}")));
            menu.Add(new("5", "Total E-Books", () => WriteAndWait($"Total e-books: {library.EBooks.Count}")));
            menu.Add(new("6", "Total Borrowed Physical Books", () => WriteAndWait($"Total borrowed physicalBooks: {library.TotalBorrowedPhysicalBooks}")));
            menu.Add(new("7", "Total Borrowed E-Books", () => WriteAndWait($"Total borrowed e-books: {library.TotalBorrowedEBooks}")));
            menu.Add(new("8", "All Book Titles", library.ConsoleAllBookTitles));
            menu.Add(new("9", "System's Total Books Count", () => WriteAndWait($"System's total books count: {library.TotalBooksCount}")));
            menu.Add(new("10", "Register New Member", () => { WriteAndWait($"\n{REGISTER_MEMBER_TEXT}"); library.RegisterMember(); }));
            menu.Add(new("11", "Total Members Count", () => WriteAndWait($"Total members count: {library.TotalMembersCount}")));
            menu.Add(new("12", "Total Teacher Members Count", () => WriteAndWait($"Total members count: {library.TotalTeacherMembersCount}")));
            menu.Add(new("13", "Total Student Members Count", () => WriteAndWait($"Total members count: {library.TotalStudentMembersCount}")));

            menu.Ask();
        }
    }

    private static void WriteAndWait(string text)
    {
        WriteLine(text);
        WriteLine();
        ReadKey();
    }
}