namespace BlazorMultiSelect.Components.Pages;

public partial class Home
{
    private FormModel model = new();

    private List<Fruit> Fruits = new()
    {
        new() { Id = "apple", Name = "Apple" },
        new() { Id = "banana", Name = "Banana" },
        new() { Id = "orange", Name = "Orange" },
        new() { Id = "mango", Name = "Mango" }
    };

    private IEnumerable<string> SelectedFruitNames => Fruits
        .Where(f => model.SelectedFruitIds.Contains(f.Id))
        .Select(f => f.Name);

    private void Submit()
    {
        // Just for demo: show selected IDs in console
        Console.WriteLine("Selected IDs: " + string.Join(", ", model.SelectedFruitIds));
    }
}

public class Fruit
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class FormModel
{
    public List<string> SelectedFruitIds { get; set; } = new();
}