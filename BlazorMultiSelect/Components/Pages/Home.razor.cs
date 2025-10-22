using BlazorMultiSelect.Models;
using BlazorMultiSelect.Validation;

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

    private List<Spaceship> AvailableSpaceships { get; set; } = [
        new Spaceship{ Id = 1, Name = "Ariane", Description = "Test 1", Classification = "Light", IsValidatedDesign = true, MaximumAccommodation = 2, ProductionDate = DateTime.Now },
        new Spaceship{ Id = 2, Name = "Falcon 9", Description = "Test 2", Classification = "Heavy", IsValidatedDesign = true, MaximumAccommodation = 3, ProductionDate = DateTime.Now },
        new Spaceship{ Id = 3, Name = "Super Heavy", Description = "Test 3", Classification = "Superheavy", IsValidatedDesign = true, MaximumAccommodation = 5, ProductionDate = DateTime.Now },
        new Spaceship{ Id = 4, Name = "Starship", Description = "Test 4", Classification = "Interstellar", IsValidatedDesign = false, MaximumAccommodation = 200, ProductionDate = DateTime.Now },
    ];

    //private List<Starship> SelectedStarships { get; set; } = [];
    private List<Spaceship> SelectedStarships2 { get; set; } = [];

    public Home()
    {
        model.SelectedSpaceships.Add(AvailableSpaceships[1]);
    }

    private string Output = string.Empty;
    private void Submit()
    {
        Output = "Selected IDs: " + string.Join(", ", model.SelectedSpaceships.Select(x => x.Name));
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

    [AtLeastOneElement(ErrorMessage = "Minstens één ruimteschip is verplicht.")]
    public List<Spaceship> SelectedSpaceships { get; set; } = [];
}