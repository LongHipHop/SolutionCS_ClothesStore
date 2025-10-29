using APIService.Models;
using APIService.Models.DTOs;
using APIService.Repository.Interface;
using APIService.Service.Interface;
using System.Diagnostics;

namespace APIService.Service.Implementations
{
    public class CartService : ICartService
    {
        private readonly IRepositoryManager _repositoryManager;
        public CartService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<int> AddToCartAsync(int accountId, int productId, int colorId, int sizeId, int quantity)
        {
            Console.WriteLine($"[DEBUG] ProductId: {productId}, ColorId: {colorId}, SizeId: {sizeId}");

            var variant = await _repositoryManager.ProductVariantRepository.GetProductVariantsAsync(productId, colorId, sizeId);
            Debug.Write(variant);
            if (variant == null)
                return 2;

            var cart = await _repositoryManager.CartRepository.GetCartByAccountIdAsync(accountId);
            if (cart == null)
            {
                cart = new Carts
                {
                    AccountId = accountId,
                    CreatedAt = DateTime.UtcNow,
                };
                await _repositoryManager.CartRepository.CreateCart(cart);
            }

            var cartItem = await _repositoryManager.CartItemRepository.GetCartItemAsync(cart.Id, variant.Id);

            if(cartItem != null)
            {
                cartItem.Quantity += quantity;
                await _repositoryManager.CartItemRepository.UpdateCartItem(cartItem);
            }
            else
            {
                var newItem = new CartItems
                {
                    CartId = cart.Id,
                    ProductVariantId = variant.Id,
                    Quantity = quantity
                };
                await _repositoryManager.CartItemRepository.CreateCartItem(newItem);
            }

            return 0;
        }

        public async Task<int> ClearCartAsync(int accountId)
        {
            try
            {
                var cartExist = await _repositoryManager.CartRepository.GetCartByAccountIdAsync(accountId);
                if (cartExist == null)
                {
                    return 2;
                }

                
                await _repositoryManager.CartRepository.ClearCartAsync(accountId);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }

        public async Task<CartDTO?> GetCartByAccountIdAsync(int accountId)
        {
            var cart = await _repositoryManager.CartRepository.GetCartWithItemsByAccountIdAsync(accountId);
            if (cart == null) return null;

            return new CartDTO
            {
                CartId = cart.Id,
                AccountId = cart.AccountId,
                CreatedAt = cart.CreatedAt,
                Items = cart.CartItems.Select(i => new CartItemDTO
                {
                    ProductId = i.ProductVariant.ProductId,
                    ProductName = i.ProductVariant.Product.ProductName,
                    ProductImage = i.ProductVariant.Product.Image,
                    Color = i.ProductVariant.Color.ColorName,
                    Size = i.ProductVariant.Size.SizeName,
                    Quantity = i.Quantity,
                    Price = i.ProductVariant.Price,
                    Total = i.Quantity * i.ProductVariant.Price
                }).ToList()
            };
        }

        public async Task RemoveSelectedItemsAsync(List<int> productIds, int accountId)
        {
            if(productIds == null || productIds.Count == 0)
            {
                throw new ArgumentException("List can't empty.");
            }

            await _repositoryManager.CartItemRepository.RemoveSelectedItemAsync(productIds, accountId);
        }
    }
}
