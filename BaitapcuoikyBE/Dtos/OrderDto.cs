namespace BaitapcuoikyBE.Dtos;

public record OrderItemDto(int ProductId, int Quantity);
public record OrderCreateDto(int CustomerId, List<OrderItemDto> Items);
