namespace LibraryManagementSystem.Interfaces.Infrastructure.Services;

internal interface ILibraryService
{
    void RegisterMember();

    void AddBook();

    void BorrowBook();

    void ReturnBook();

    void ConsoleAllBookTitles();
}