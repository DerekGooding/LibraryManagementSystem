namespace LibraryManagementSystem.Utils;

internal static class MenuSelector
{
    public static string SelectOption(
        List<string> options,
        string message = "Use the arrow keys to navigate and press Enter to select:",
        bool beepSound = true)
    {
        int currentSelection = 0;
        ConsoleKey key = ConsoleKey.None;

        WriteLine(message);

        // Store the initial cursor position
        int cursorTop = CursorTop;

        while (key != ConsoleKey.Enter)
        {
            // Display the options
            for (int i = 0; i < options.Count; i++)
            {
                // Move the cursor to the correct position
                SetCursorPosition(0, cursorTop + i);

                if (i == currentSelection)
                {
                    // Highlight the currently selected option
                    ForegroundColor = ConsoleColor.Green;
                    WriteLine($"> {options[i]}");
                    ResetColor();
                }
                else
                {
                    WriteLine($"  {options[i]}");
                }
            }

            // Capture the key press
            key = ReadKey(true).Key;
            if (beepSound) Beep();

            // Update the current selection based on the key press
            if (key == ConsoleKey.UpArrow)
            {
                currentSelection--;
                if (currentSelection < 0)
                    currentSelection = options.Count - 1;
            }
            else if (key == ConsoleKey.DownArrow)
            {
                currentSelection++;
                if (currentSelection >= options.Count)
                    currentSelection = 0;
            }
        }

        // Return the selected option
        return options[currentSelection];
    }
}