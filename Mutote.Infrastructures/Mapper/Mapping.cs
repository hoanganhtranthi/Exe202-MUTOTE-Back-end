
using AutoMapper;
using MuTote.Application.DTO.Request;
using MuTote.Application.DTO.Response;
using MuTote.Domain.Enities;

namespace MuTote.Infrastructures.Mapper
{
    public class Mapping : Profile
    {
        public Mapping() {

            CreateMap<Customer, CustomerResponse>().ReverseMap();
            CreateMap<Customer, CustomerRequest>().ReverseMap();
            CreateMap<UpdateCustomerRequest, Customer>().ReverseMap();
            CreateMap<CreateCustomerRequest, Customer>().ReverseMap();
            CreateMap<CustomerResponse, CustomerRequest>().ReverseMap();

            CreateMap<CategoryProduct, CategoryResponse>().ReverseMap();
            CreateMap<CreateCategoryRequest, CategoryProduct>().ReverseMap();

            CreateMap<CategoryMaterial, CategoryResponse>().ReverseMap();
            CreateMap<CreateCategoryRequest, CategoryMaterial>().ReverseMap();

            CreateMap<Material, MaterialRequest>().ReverseMap();
            CreateMap<Material, MaterialResponse>().ReverseMap();
            CreateMap<MaterialRequest, MaterialResponse>().ReverseMap();
            CreateMap<CreateMaterialRequest, Material>().ReverseMap();

            CreateMap<Product, ProductRequest>().ReverseMap();
            CreateMap<Product, ProductResponse>().ReverseMap();
            CreateMap<ProductRequest, ProductResponse>().ReverseMap();
            CreateMap<CreateProductRequest, Product>().ReverseMap();

            CreateMap<Designer, DesignerResponse>().ReverseMap();
            CreateMap<Designer, DesignerRequest>().ReverseMap();
            CreateMap<UpdateDesignerRequest, Designer>().ReverseMap();
            CreateMap<CreateDesignerRequest, Designer>().ReverseMap();
            CreateMap<DesignerRequest, DesignerResponse>().ReverseMap();

            CreateMap<Order, OrderResponse>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailResponse>().ReverseMap();
            CreateMap<OrderDetailRequest, OrderDetail>();
            CreateMap<CreateOrderRequest, Order>();
            CreateMap<OrderRequest, Order>().ReverseMap();
            CreateMap<OrderRequest, OrderResponse>().ReverseMap();
            CreateMap<OrderDetailRequest, OrderDetailResponse>().ReverseMap();

            CreateMap<WishList, WishListRequest>().ReverseMap();
            CreateMap<WishList, WishListResponse>().ReverseMap();
            CreateMap<WishListRequest, WishListResponse>().ReverseMap();
            CreateMap<CreateWishListRequest, WishList>().ReverseMap();
        }

    }
}
