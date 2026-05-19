namespace Catalog.API.Products.CreateProduct;

public record CreateProductRequest(
    string Name,
    List<string> Category, 
    string Description, 
    string ImageFile, 
    decimal Price);
    
public record CreateProductResponse(Guid Id);

public class CreateProductEndPoint : ICarterModule
{
    private const string Route = "/products";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        try
        {
            _ = app.MapPost(pattern: Route,
                handler: static async (CreateProductRequest request, ISender sender) =>
                {
                    CreateProductCommand command = request.Adapt<CreateProductCommand>();
                    CreateProductResult result = await sender.Send(command);
                    CreateProductResponse response = result.Adapt<CreateProductResponse>();
                    
                    return Results.Created($"/products/{response.Id}", response);
                })
                .WithName("CreateProduct")
                .Produces<CreateProductResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Create Product")
                .WithDescription("Create Product");
        }
        catch (Exception ex)
        {
            throw new Exception("Error creating product", ex);
        }
    }
}
