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
        WriteLine("============= WELCOME TO LIBRARY MANAGEMENT SYSTEM ==============\n");

        Menu menu = MainMenu();

        while (true)
        {
            WriteLine("[MENU]:");
            menu.Ask();
            Clear();
        }
    }

    private static Menu MainMenu()
    {
        LibManagementSystem library = new();
        Menu menu = new();
        menu.Add(new("X", "Exit", () => Environment.Exit(0)));
        menu.Add(new("1", "Add Book", () =>
        {
            WriteLine($"\n{ADD_BOOK_ACTION_TEXT}");
            library.AddBook();
        }));
        menu.Add(new("2", "Borrow Book", () =>
        {
            WriteLine($"\n{BORROW_BOOK_ACTION_TEXT}");
            library.BorrowBook();
        }));
        menu.Add(new("3", "Return Book", () =>
        {
            WriteLine($"\n{RETURN_BOOK_ACTION_TEXT}");
            library.ReturnBook();
        }));
        menu.Add(new("4", "Total Physical Books", () => Info($"Total physical books: {library.PhysicalBooks.Count}")));
        menu.Add(new("5", "Total E-Books", () => Info($"Total e-books: {library.EBooks.Count}")));
        menu.Add(new("6", "Total Borrowed Physical Books", () => Info($"Total borrowed physicalBooks: {library.TotalBorrowedPhysicalBooks}")));
        menu.Add(new("7", "Total Borrowed E-Books", () => Info($"Total borrowed e-books: {library.TotalBorrowedEBooks}")));
        menu.Add(new("8", "All Book Titles", library.ConsoleAllBookTitles));
        menu.Add(new("9", "System's Total Books Count", () => Info($"System's total books count: {library.TotalBooksCount}")));
        menu.Add(new("10", "Register New Member", () =>
        {
            WriteLine($"\n{REGISTER_MEMBER_TEXT}");
            library.RegisterMember();
        }));
        menu.Add(new("11", "Total Members Count", () => Info($"Total members count: {library.TotalMembersCount}")));
        menu.Add(new("12", "Total Teacher Members Count", () => Info($"Total members count: {library.TotalTeacherMembersCount}")));
        menu.Add(new("13", "Total Student Members Count", () => Info($"Total members count: {library.TotalStudentMembersCount}")));

        return menu;
    }

    private static void Info(string text)
    {
        WriteLine(text);
        WriteLine();
        ReadKey();
    }
}