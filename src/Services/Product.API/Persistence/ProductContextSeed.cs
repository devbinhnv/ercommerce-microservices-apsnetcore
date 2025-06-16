
using Product.API.Entities;
using ILogger =  Serilog.ILogger;

namespace Product.API.Persistence;

public class ProductContextSeed
{
    public static async Task SeedProductAsync(ProductContext context, ILogger logger)
    {
        if (!context.Products.Any())
        {
            context.Products.AddRange(GetCatalogProducts());
            await context.SaveChangesAsync();
            logger.Information($"Seeded data for Product DB associated with context {nameof(context)}");
        }
    }

    private static IEnumerable<CatalogProduct> GetCatalogProducts()
    {
        return new List<CatalogProduct>
        {
            new()
            {
                No = "PRD001",
                Name = "Bàn học thông minh",
                Summary = "Bàn học dành cho học sinh tiểu học, có giá sách kèm theo.",
                Description = "Bàn học với thiết kế hiện đại, chất liệu gỗ thông, độ bền cao. Phù hợp với không gian nhỏ.",
                Price = 14500.06m
            },
            new()
            {
                No = "PRD002",
                Name = "Ghế công thái học",
                Summary = "Ghế làm việc hỗ trợ lưng tốt, có thể điều chỉnh nhiều góc độ.",
                Description = "Ghế với thiết kế Ergonomic giúp giảm đau lưng và mỏi cổ khi làm việc lâu.",
                Price = 219000.39m
            },
            new()
            {
                No = "PRD003",
                Name = "Kệ sách treo tường",
                Summary = "Kệ sách tiết kiệm không gian, phù hợp với mọi loại phòng.",
                Description = "Kệ sách gỗ công nghiệp MDF phủ Melamine, dễ lắp đặt và trang trí.",
                Price = 780020m
            }
        };
    }
}
