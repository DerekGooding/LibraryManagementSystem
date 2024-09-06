namespace LibraryManagementSystem.Models.Menus;

internal class Menu(List<MenuOption>? options = null)
{
    private List<MenuOption> Options { get; } = options ?? [];

    private IEnumerable<MenuOption> CheckedOptions => Options.Where(static x => x.Check?.Invoke() != false);

    public void Add(MenuOption option) => Options.Add(option);

    public MenuOption? Find(Predicate<MenuOption> match) => CheckedOptions.FirstOrDefault(x => match(x));

    internal int Count => Options.Count;

    public string Print() => string.Join('\n', CheckedOptions.Select(x => x.Print()));
}