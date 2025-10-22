using BlazorMultiSelect.Models;
using BlazorMultiSelect.Validation;

namespace BlazorMultiSelect.Components.Pages;

public partial class Home
{
    private FormModel model = new();

    private List<Fruit> AvailableFruits = new()
    {
        new() { Id = 1, Name = "Apple" },
        new() { Id = 2, Name = "Banana" },
        new() { Id = 3, Name = "Orange" },
        new() { Id = 4, Name = "Mango" }
    };

    private List<Spaceship> AvailableSpaceships { get; set; } = [
        new Spaceship{ Id = 1, Name = "Ariane", Description = "Test 1", Classification = "Light", IsValidatedDesign = true, MaximumAccommodation = 2, ProductionDate = DateTime.Now },
        new Spaceship{ Id = 2, Name = "Falcon 9", Description = "Test 2", Classification = "Heavy", IsValidatedDesign = true, MaximumAccommodation = 3, ProductionDate = DateTime.Now },
        new Spaceship{ Id = 3, Name = "Super Heavy", Description = "Test 3", Classification = "Superheavy", IsValidatedDesign = true, MaximumAccommodation = 5, ProductionDate = DateTime.Now },
        new Spaceship{ Id = 4, Name = "Starship", Description = "Test 4", Classification = "Interstellar", IsValidatedDesign = false, MaximumAccommodation = 200, ProductionDate = DateTime.Now },
    ];

    //private List<Starship> SelectedStarships { get; set; } = [];
    private List<Spaceship> SelectedStarships2 { get; set; } = [];
    private bool ShowSpaceshipMultiSelect { get; set; } = true;

    public Home()
    {
        model.SelectedSpaceships.Add(AvailableSpaceships[1]);
    }

    private string Output = string.Empty;
    private string OutputFuits = string.Empty;
    private void Submit()
    {
        Output = "Selected space ships: " + string.Join(", ", model.SelectedSpaceships.Select(x => x.Name));
        OutputFuits = "Selected fruits: " + string.Join(", ", model.SelectedFruits.Select(x => x.Name));
    }

    private void HideAndShow()
    {
        ShowSpaceshipMultiSelect = !ShowSpaceshipMultiSelect;
    }
}

public class Fruit
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class FormModel
{
    public List<Fruit> SelectedFruits { get; set; } = [];

    [AtLeastOneElement(ErrorMessage = "Minstens één ruimteschip is verplicht.")]
    public List<Spaceship> SelectedSpaceships { get; set; } = [];
}