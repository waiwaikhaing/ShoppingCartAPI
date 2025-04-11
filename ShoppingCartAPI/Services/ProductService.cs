using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ShoppingCartAPI.EFDBContext;
using ShoppingCartAPI.Helper;
using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Services
{
    public interface IProductService
    {
        Task<ServiceDataResponse<List<Product>>> GetProducts();
        Task<ServiceDataResponse<List<Product>>> AddProduct(List<ProductDTO> itemDto);

    }
    public class ProductService : IProductService
    {
        private readonly AppDBContext _context;

        public ProductService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<ServiceDataResponse<List<Product>>> GetProducts()
        {
            ServiceDataResponse<List<Product>> response = new ServiceDataResponse<List<Product>>();
            List<Product> list = new List<Product>();
            try
            {
                list = await _context.Products
                  .Where(ci => ci.Active == true)
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
                    response.Message = "Success";
                }
                //write log
                var jsonOptions = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string msg = "Error :" + DateTime.Now + " >>>> " + JsonSerializer.Serialize(response, jsonOptions);
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

        public async Task<ServiceDataResponse<List<Product>>> AddProduct(List<ProductDTO> itemDto)
        {
            ServiceDataResponse<List<Product>> response = new ServiceDataResponse<List<Product>>();
            try
            {
                var result = 0;
                //to write log format
                var jsonOptions = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                if (itemDto.Count > 0)
                {
                    #region write log
                    string text = "Error :" + DateTime.Now + " >>>> " + JsonSerializer.Serialize(itemDto, jsonOptions);
                    General.WriteLogInTextFile(text);
                    #endregion

                    foreach (var item in itemDto)
                    {

                        var product = await _context.Products.FirstOrDefaultAsync(c => c.ProductName == item.ProductName
                                      && c.Active == true);

                        if (product == null)
                        {
                            product = new Product
                            {
                                ProductId = Guid.NewGuid().ToString(),
                                Active = true,
                                CreatedBy = Guid.NewGuid().ToString(),
                                CreatedOn = DateTime.Now
                            };
                            _context.Products.Add(product);
                        }
                        else
                        {
                            product.ModifiedBy = Guid.NewGuid().ToString();
                            product.ModifiedOn = DateTime.Now;
                        }

                        product.Qty = item.Qty;
                        product.ProductName = item.ProductName;
                        product.Price = item.UnitPrice;
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
    }
}
