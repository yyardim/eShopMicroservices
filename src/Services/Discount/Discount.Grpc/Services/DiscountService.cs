using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;

namespace Discount.Grpc.Services;

public class DiscountService
    (DiscountContext dbContext, ILogger<DiscountService> logger)
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        IQueryable<Coupon> query = Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.AsNoTracking(dbContext.Coupons);
        Coupon? coupon = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(
            query,
            c => c.ProductName == request.ProductName,
            context.CancellationToken);

        coupon ??= new Coupon
        {
            ProductName = request.ProductName,
            Amount = 0,
            Description = "No discount available"
        };

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Discount retrieved for ProductName: {ProductName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);

        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        Coupon coupon = request.Coupon.Adapt<Coupon>()
            ?? throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid coupon data"));

        _ = dbContext.Coupons.Add(coupon);
        _ = await dbContext.SaveChangesAsync();

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Discount created for ProductName: {ProductName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);

        CouponModel couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        Coupon coupon = request.Coupon.Adapt<Coupon>()
            ?? throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid coupon data"));

        _ = dbContext.Coupons.Update(coupon);
        _ = await dbContext.SaveChangesAsync();

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Discount updated for ProductName: {ProductName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);

        CouponModel couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        Coupon? coupon = dbContext.Coupons
            .FirstOrDefault(c => c.ProductName == request.ProductName) 
            ?? throw new RpcException(new Status(StatusCode.NotFound, $"Discount not found for ProductName: {request.ProductName}"));

        dbContext.Coupons.Remove(coupon);
        await dbContext.SaveChangesAsync();

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Discount deleted for ProductName: {ProductName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);

        return new DeleteDiscountResponse { Success = true };
    }
}
