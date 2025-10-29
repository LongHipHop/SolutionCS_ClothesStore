namespace APIService.Repository.Interface
{
    public interface IRepositoryManager
    {
        IAccountRepostiory AccountRepostiory { get; }
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICartItemRepository CartItemRepository { get; }
        ICartRepository CartRepository { get; }
        IProductVariantRepository ProductVariantRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        IOrderRepository OrderRepository { get; }
        IPasswordResetRepository PasswordResetRepository { get; }
        IEmailVerificationRepository EmailVerificationRepository { get; }
    }
}
