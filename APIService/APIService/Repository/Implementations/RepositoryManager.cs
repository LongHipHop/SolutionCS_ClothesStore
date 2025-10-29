using APIService.Models;
using APIService.Repository.Interface;

namespace APIService.Repository.Implementations
{
    public class RepositoryManager : IRepositoryManager
    {
        private ShopDbContext _db;
        private IAccountRepostiory _accountRepostiory;
        private IProductRepository _productRepository;
        private ICategoryRepository _categoryRepository;
        private ICartRepository _cartRepository;
        private ICartItemRepository _cartItemRepository;
        private IProductVariantRepository _productVariantRepository;
        private IPaymentRepository _paymentRepository;
        private IOrderRepository _orderRepository;
        private IPasswordResetRepository _passwordResetRepository;
        private IEmailVerificationRepository _emailVerificationRepository;

        public RepositoryManager(ShopDbContext db)
        {
            _db = db;
        }

        public IAccountRepostiory AccountRepostiory
        {
            get
            {
                if (_accountRepostiory == null)
                {
                    _accountRepostiory = new AccountRepostiory(_db);
                }
                return _accountRepostiory;
            }
        }

        public IProductRepository ProductRepository
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository = new ProductRepository(_db);
                }
                return _productRepository;
            }
        }
        
        public ICategoryRepository CategoryRepository
        {
            get
            {
                if(_categoryRepository == null)
                {
                    _categoryRepository = new CategoryRepository(_db);
                }
                return (_categoryRepository);
            }
        }

        public ICartItemRepository CartItemRepository
        {
            get
            {
                if(_cartItemRepository == null)
                {
                    _cartItemRepository = new CartItemRepository(_db);
                }
                return (_cartItemRepository);
            }
        }

        public IProductVariantRepository ProductVariantRepository
        {
            get
            {
                if(_productVariantRepository == null)
                {
                    _productVariantRepository = new ProductVariantRepository(_db);
                }
                return (_productVariantRepository);
            }
        }

        public ICartRepository CartRepository
        {
            get
            {
                if(_cartRepository == null)
                {
                    _cartRepository = new CartRepository(_db);
                }
                return (_cartRepository);
            }

        }

        public IPaymentRepository PaymentRepository
        {
            get
            {
                if(_paymentRepository == null)
                {
                    _paymentRepository = new PaymentRepository(_db);
                }
                return _paymentRepository;
            }
        }

        public IOrderRepository OrderRepository
        {
            get
            {
                if(_orderRepository == null)
                {
                    _orderRepository = new OrderRepository(_db);
                }
                return _orderRepository;
            }
        }

        public IPasswordResetRepository PasswordResetRepository
        {
            get
            {
                if(_passwordResetRepository == null)
                {
                    _passwordResetRepository = new PasswordResetRepository(_db);
                }
                return _passwordResetRepository;
            }
        }

        public IEmailVerificationRepository EmailVerificationRepository
        {
            get
            {
                if(_emailVerificationRepository == null)
                {
                    _emailVerificationRepository = new EmailVerificationRepository(_db);
                }
                return _emailVerificationRepository;
            }
        }
    }
}
