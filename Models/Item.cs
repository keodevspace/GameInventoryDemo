// Representa a entidade de domínio do item no jogo.

namespace GameInventoryDemo.Models;

public class Item
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Stock { get; set; }
}