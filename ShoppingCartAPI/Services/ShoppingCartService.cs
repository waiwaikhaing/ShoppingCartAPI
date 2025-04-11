using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ShoppingCartAPI.EFDBContext;
using ShoppingCartAPI.Helper;
using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Services
{
    public interface IShoppingCartService
    {
        Task<ServiceDataResponse<List<CartItemDetailDTO>>> GetCartItemsAsync(string userId);
        Task<ServiceDataResponse<List<CartItem>>> AddItemToCartAsync(string userId, List<CartItemDTO> itemDto);
        Task<ServiceResponse> RemoveItemFromCartAsync(string cartItemId);
        Task<ServiceDataResponse<List<CartItemResponselDTO>>> CheckoutAsync(string userId);
    }
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly AppDBContext _context;

        public ShoppingCartService(AppDBContext context)
        {
            _context = context;
        }

        #region get item
        public async Task<ServiceDataResponse<List<CartItemDetailDTO>>> GetCartItemsAsync(string userId)
        {
            ServiceDataResponse<List<CartItemDetailDTO>> response = new ServiceDataResponse<List<CartItemDetailDTO>>();
            List<CartItemDetailDTO> list = new List<CartItemDetailDTO>();
            try
            {
                list = await _context.CartItemViews
                        .Where(v => v.UserId == userId && v.Status == "InCart")
                        .ToListAsync();

                if (list != null && list.Count > 0)
                {
                    response.Success = true;
                    response.Message = "Success";
                    response.Data = list;
                }
                else
                {
                    response.Success = true;
                    response.Message = "no data";
                }
                //write log
                var jsonOptionss = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string msg = "Info :" + DateTime.Now + " >>>> " + JsonSerializer.Serialize(response, jsonOptionss);
                General.WriteLogInTextFile(msg);
            }
            catch (Exception ex)
            {
                string msg = "Error :" + DateTime.Now + ">>>>" + ex.Message;
                General.WriteLogInTextFile(msg);

                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion

        #region add item
        public async Task<ServiceDataResponse<List<CartItem>>> AddItemToCartAsync(string userId, List<CartItemDTO> itemDto)
        {
            ServiceDataResponse<List<CartItem>> response = new ServiceDataResponse<List<CartItem>>();
            try
            {
                var result = 0; 
                var jsonOptions = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                if (itemDto.Count > 0)
                {
                    //write log
                    
                    string text = "info :" + DateTime.Now + " >>>> " + JsonSerializer.Serialize(response, jsonOptions);
                    General.WriteLogInTextFile(text);
                    foreach (var item in itemDto)
                    {
                        var product = await _context.Products.FirstOrDefaultAsync(c => c.ProductId == item.ProductId
                    && c.Active == true);

                        if (product == null)
                            throw new Exception("Product not found");

                        var existingItem = await _context.CartItems
                            .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == item.ProductId
                            && ci.Active == true);

                        if (existingItem == null)
                        {
                            existingItem = new CartItem
                            {
                                CartItemId = Guid.NewGuid().ToString(),
                                Active = true,
                                CreatedOn = DateTime.Now,
                                CreatedBy = userId
                            };
                            _context.CartItems.Add(existingItem);
                        }
                        else
                        {
                            product.ModifiedBy = userId;
                            product.ModifiedOn = DateTime.Now;
                        }

                        existingItem.UserId = userId;
                        existingItem.Qty = item.Qty;
                        existingItem.ProductId = item.ProductId;
                        existingItem.Price = product.Price;
                        existingItem.Status = "InCart";
                    }
                    result = await _context.SaveChangesAsync();
                }
                if (result > 0)
                {
                    response.Success = true;
                    response.Message = "Success";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed";
                }

                //write log
                string msg = "Info :" + DateTime.Now + " >>>> " + JsonSerializer.Serialize(response, jsonOptions);
                General.WriteLogInTextFile(msg);
            }
            catch (Exception ex)
            {
                string msg = "Error :" + DateTime.Now + ">>>>" + ex.Message;
                General.WriteLogInTextFile(msg);
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion

        #region Remove 
        public async Task<ServiceResponse> RemoveItemFromCartAsync(string cartItemId)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                var cartItem = await _context.CartItems.FindAsync(cartItemId);
                if (cartItem == null)
                    throw new Exception("Cart item not found");

                cartItem.Active = false;
                var res = await _context.SaveChangesAsync();

                if (res > 0)
                {
                    response.Success = true;
                    response.Message = "Success";
                }
                else
                {
                    response.Success = true;
                    response.Message = "failed";
                }

                //write log
                var jsonOptions = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string msg = "Info :" + DateTime.Now + " >>>> " + JsonSerializer.Serialize(response, jsonOptions);
                General.WriteLogInTextFile(msg);
            }
            catch (Exception ex)
            {
                string msg = "Error :" + DateTime.Now + ">>>>" + ex.Message;
                General.WriteLogInTextFile(msg);

                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion

        #region check out 
        public async Task<ServiceDataResponse<List<CartItemResponselDTO>>> CheckoutAsync(string userId)
        {
            ServiceDataResponse<List<CartItemResponselDTO>> response = new ServiceDataResponse<List<CartItemResponselDTO>>();
            List<CartItemResponselDTO> notEnoughStockItemsList = new List<CartItemResponselDTO>();

            try
            {
                var items = await _context.CartItems.Where(ci => ci.UserId == userId && ci.Active == true
                && ci.Status != "Ordered").ToListAsync();
                if (!items.Any())
                    throw new Exception("Cart is empty");
                var result = 0;
                bool isNotEnough = false;
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        CartItemResponselDTO notEnoughStockItems = new CartItemResponselDTO();
                        var product = await _context.Products
                            .Where(ci => ci.ProductId == item.ProductId && ci.Active == true)
                            .FirstOrDefaultAsync();
                        //checking enough or not enough qty
                        if (product != null)
                        {
                            if (product.Qty >= item.Qty) {
                                product.Qty = product.Qty - item.Qty;
                                item.Status = "Ordered";

                                notEnoughStockItems.ProductName = product.ProductName;
                                notEnoughStockItems.CartItemId = item.CartItemId;
                                // notEnoughStockItems.Message = "already ordered";
                                notEnoughStockItems.Message = $"Product: {product.ProductName} (Available: {product.Qty}, Requested: {item.Qty})";

                            }
                            else
                            {
                                isNotEnough = true;
                                notEnoughStockItems.ProductName = product.ProductName;
                                notEnoughStockItems.CartItemId = item.CartItemId;
                                notEnoughStockItems.Message = $"Product: {product.ProductName} (Available: {product.Qty}, Requested: {item.Qty})";
                            }
                        }
                        notEnoughStockItemsList.Add(notEnoughStockItems);
                    }
                    //if qty is not enough , didnot order
                    if(!isNotEnough) result = await _context.SaveChangesAsync();
                }

                if (!isNotEnough)
                {
                    if (result > 0)
                    {
                        response.Success = true;
                        response.Message = "Success";
                        response.Data = notEnoughStockItemsList;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Failed";
                    }
                }
                else
                {
                        response.Success = false;
                        response.Message = "Not Enough";
                        response.Data = notEnoughStockItemsList;
                }

                    //write log
                    var jsonOptions = new JsonSerializerOptions
                    {
                        WriteIndented = true
                    };

                string msg = "Info :" + DateTime.Now + " >>>> " + JsonSerializer.Serialize(response, jsonOptions);
                General.WriteLogInTextFile(msg);

                return response;
            }
            catch (Exception ex)
            {
                string msg = "Error :" + DateTime.Now + ">>>>" + ex.Message;
                General.WriteLogInTextFile(msg);
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }
        #endregion
    }
}
