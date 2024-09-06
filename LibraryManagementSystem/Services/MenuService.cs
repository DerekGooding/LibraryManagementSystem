using LibraryManagementSystem.Models.Menus;

namespace LibraryManagementSystem.Services;

internal static class MenuService
{
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
            WriteLine("\n[ACTION]: Add Book");
            library.AddBook();
        }));
        menu.Add(new("2", "Borrow Book", () =>
        {
            WriteLine("\n[ACTION]: Borrow book");
            library.BorrowBook();
        }));
        menu.Add(new("3", "Return Book", () =>
        {
            WriteLine("\n[ACTION]: Return book");
            library.ReturnBook();
        }));
        menu.Add(new("4", "Total Physical Books", () => Info($"Total physical books: {library.PhysicalBookCount}")));
        menu.Add(new("5", "Total E-Books", () => Info($"Total e-books: {library.EBookCount}")));
        menu.Add(new("6", "Total Borrowed Physical Books", () => Info($"Total borrowed physicalBooks: {library.BorrowedPhysicalBooks}")));
        menu.Add(new("7", "Total Borrowed E-Books", () => Info($"Total borrowed e-books: {library.BorrowedEBooks}")));
        menu.Add(new("8", "All Book Titles", library.ConsoleAllBookTitles));
        menu.Add(new("9", "System's Total Books Count", () => Info($"System's total books count: {library.BooksCount}")));
        menu.Add(new("10", "Register New Member", () =>
        {
            WriteLine("\n[ACTION]: Register Member");
            library.RegisterMember();
        }));
        menu.Add(new("11", "Total Members Count", () => Info($"Total members count: {library.MembersCount}")));
        menu.Add(new("12", "Total Teacher Members Count", () => Info($"Total members count: {library.TeacherCount}")));
        menu.Add(new("13", "Total Student Members Count", () => Info($"Total members count: {library.StudentCount}")));

        return menu;
    }

    private static void Info(string text)
    {
        WriteLine(text);
        WriteLine();
        ReadKey();
    }
}