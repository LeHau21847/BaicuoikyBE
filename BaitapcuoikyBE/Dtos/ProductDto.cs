namespace BaitapcuoikyBE.Dtos;

public record ProductCreateDto(string Name, decimal Price, string? Description, int Stock);
public record ProductUpdateDto(string Name, decimal Price, string? Description, int Stock);
