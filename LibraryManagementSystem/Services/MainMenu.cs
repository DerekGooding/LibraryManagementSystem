using LibraryManagementSystem.Models.Menus;

namespace LibraryManagementSystem.Services;

internal static class MainMenu
{
    public static void Start()
    {
        WriteLine("============= WELCOME TO LIBRARY MANAGEMENT SYSTEM ==============\n");

        Menu menu = Initialize();

        while (true)
        {
            WriteLine("[MENU]:");
            menu.Ask();
            Clear();
        }
    }

    private static Menu Initialize()
    {
        LibManagementSystem library = new();
        Menu menu = new();
        menu.Add(new("X", "Exit", () => Environment.Exit(0)));
        menu.Add(new("0", "Display Library statistics", () => LibraryDetails(library)));
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

        menu.Add(new("4", "All Book Titles", library.ConsoleAllBookTitles));

        menu.Add(new("5", "Register New Member", () =>
        {
            WriteLine("\n[ACTION]: Register Member");
            library.RegisterMember();
        }));

        return menu;
    }

    private static void LibraryDetails(LibManagementSystem library)
    {
        WriteLine($"Total Books:    {library.BooksCount}");
        WriteLine($"Physical books: {library.PhysicalBookCount} of which {library.BorrowedPhysicalBooks} are borrowed");
        WriteLine($"E-books:        {library.EBookCount} of which {library.BorrowedEBooks} are borrowed");
        WriteLine();
        WriteLine($"Total Members:  {library.MembersCount}");
        WriteLine($"Teachers:       {library.TeacherCount}");
        WriteLine($"Students:       {library.StudentCount}");
        WriteLine();
        ReadKey();
    }
}