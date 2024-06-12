
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MuTote.Application.Repository;
using MuTote.Application.Service;
using MuTote.Application.Services.ImpService;
using MuTote.Application.Services.ISerive;
using MuTote.Application.UnitOfWork;
using MuTote.Infrastructures.DBContext;
using MuTote.Infrastructures.Mapper;
using MuTote.Infrastructures.Repository;
using MuTote.Infrastructures.UnitOfWork;
using MuTote.Service.Services.ISerive;

namespace ThinkTank.Infrastructures
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructuresService(this IServiceCollection services, IConfiguration configuration)
        {
            #region DI_SERVICES      
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IFileStorageService, FirebaseStorageService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IMaterialService, MaterialService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IDesignerService, DesignerService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IWishListService, WishListService>();
            #endregion

            #region DI_REPOSITORY
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            #endregion

            //Database Connection
            services.AddDbContext<MutoteContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultSQLConnection"));
            });

            services.AddAutoMapper(typeof(Mapping));
            return services;
        }
    }
}