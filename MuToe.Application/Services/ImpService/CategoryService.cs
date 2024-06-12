

using AutoMapper;
using AutoMapper.QueryableExtensions;
using MuTote.Application.DTO.Request;
using MuTote.Application.DTO.Response;
using MuTote.Application.GlobalExceptionHandling.Exceptions;
using MuTote.Application.Services.ISerive;
using MuTote.Application.UnitOfWork;
using MuTote.Domain.Enities;
using System.Net;
using static MuTote.Domain.Enums.Enum;

namespace MuTote.Application.Services.ImpService
{
    public class CategoryService:ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryResponse> DeleteCategory(int id, CategoryChoice choice)
        {
            try
            {
                if (choice.ToString().Equals("Product"))
                {
                    var category = await _unitOfWork.Repository<CategoryProduct>().GetAsync(c => c.Id == id);
                    if (category == null)
                    {
                        throw new CrudException(HttpStatusCode.NotFound, "Not found category with id", "");
                    }
                    _unitOfWork.Repository<CategoryProduct>().Delete(category);
                    await _unitOfWork.CommitAsync();
                    return _mapper.Map<CategoryProduct, CategoryResponse>(category);

                }
                else
                {
                    var category = await _unitOfWork.Repository<CategoryMaterial>().GetAsync(c => c.Id == id);
                    if (category == null)
                    {
                        throw new CrudException(HttpStatusCode.NotFound, "Not found category with id", "");
                    }
                    _unitOfWork.Repository<CategoryMaterial>().Delete(category);
                    await _unitOfWork.CommitAsync();
                    return _mapper.Map<CategoryMaterial, CategoryResponse>(category);
                }
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Delete Category Error!!!", ex.InnerException?.Message);
            }
        }

        public async Task<CategoryResponse> GetCategoryById(int id, CategoryChoice choice)
        {
            try
            {
                if (id <= 0) throw new CrudException(HttpStatusCode.BadRequest, "Id Category Invalid", "");
                if (choice.ToString().Equals("Product"))
                {
                    var response = await _unitOfWork.Repository<CategoryProduct>().GetAsync(c => c.Id == id);

                    if (response == null)
                    {
                        throw new CrudException(HttpStatusCode.NotFound, $"Not found category with id{id.ToString()}", "");
                    }

                    return _mapper.Map<CategoryResponse>(response);
                }
                else
                {
                    var response = await _unitOfWork.Repository<CategoryMaterial>().GetAsync(c => c.Id == id);

                    if (response == null)
                    {
                        throw new CrudException(HttpStatusCode.NotFound, $"Not found category with id{id.ToString()}", "");
                    }

                    return _mapper.Map<CategoryResponse>(response);
                }
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get Category By ID Error!!!", ex.InnerException?.Message);
            }
        }

        public async Task<List<CategoryResponse>> GetCategorys(CategoryChoice choice)
        {
            try
            {
                List<CategoryResponse> categorys = new List<CategoryResponse>();
                if (choice.ToString().Equals("Product"))
                    categorys = _unitOfWork.Repository<CategoryProduct>().GetAll()
                                           .ProjectTo<CategoryResponse>(_mapper.ConfigurationProvider)
                                           .ToList();
                else categorys = _unitOfWork.Repository<CategoryMaterial>().GetAll()
                                           .ProjectTo<CategoryResponse>(_mapper.ConfigurationProvider)
                                           .ToList();
                return categorys;
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get category list error!!!!!", ex.Message);
            }
        }
        public async Task<CategoryResponse> InsertCategory(CreateCategoryRequest category, CategoryChoice choice)
        {
            try
            {


                if (choice.ToString().Equals("Product"))
                {
                    var cateRequest = await _unitOfWork.Repository<CategoryProduct>().GetAsync(u => u.Name == category.Name);
                    if (category == null)
                    {
                        throw new CrudException(HttpStatusCode.BadRequest, "Category Invalid!!!", "");
                    }
                    if (cateRequest != null)
                    {
                        throw new CrudException(HttpStatusCode.BadRequest, "Category has already insert !!!", "");
                    }

                    var response = _mapper.Map<CreateCategoryRequest, CategoryProduct>(category);
                    await _unitOfWork.Repository<CategoryProduct>().CreateAsync(response);
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<CategoryResponse>(response);
                }
                else
                {
                    var cateRequest = await _unitOfWork.Repository<CategoryMaterial>().GetAsync(u => u.Name == category.Name);
                    if (category == null)
                    {
                        throw new CrudException(HttpStatusCode.BadRequest, "Category Invalid!!!", "");
                    }
                    if (cateRequest != null)
                    {
                        throw new CrudException(HttpStatusCode.BadRequest, "Category has already insert !!!", "");
                    }

                    var response = _mapper.Map<CreateCategoryRequest, CategoryMaterial>(category);
                    await _unitOfWork.Repository<CategoryMaterial>().CreateAsync(response);
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<CategoryResponse>(response);
                }
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Insert Category Error!!!", ex.InnerException?.Message);
            }
        }

        public async Task<CategoryResponse> UpdateCategory(int id, CreateCategoryRequest request, CategoryChoice choice)
        {
            try
            {
                if (choice.ToString().Equals("Product"))
                {
                    CategoryProduct category = _unitOfWork.Repository<CategoryProduct>()
                    .Find(c => c.Id == id);
                    var cateRequest = await _unitOfWork.Repository<CategoryProduct>().GetAsync(u => u.Name == category.Name);

                    if (category == null)
                    {
                        throw new CrudException(HttpStatusCode.NotFound, $"Not found category with id{id.ToString()}", "");
                    }

                    if (cateRequest.Id != id)
                    {
                        throw new CrudException(HttpStatusCode.BadRequest, "Category has already !!!", "");
                    }
                    _mapper.Map<CreateCategoryRequest, CategoryProduct>(request, category);

                    await _unitOfWork.Repository<CategoryProduct>().Update(category, id);
                    await _unitOfWork.CommitAsync();
                    return _mapper.Map<CategoryProduct, CategoryResponse>(category);
                }
                else
                {
                    CategoryMaterial category = _unitOfWork.Repository<CategoryMaterial>()
                  .Find(c => c.Id == id);
                    var cateRequest = await _unitOfWork.Repository<CategoryMaterial>().GetAsync(u => u.Name == request.Name);

                    if (category == null)
                    {
                        throw new CrudException(HttpStatusCode.NotFound, $"Not found category with id{id.ToString()}", "");
                    }

                    if (cateRequest.Id != id)
                    {
                        throw new CrudException(HttpStatusCode.BadRequest, "Category has already !!!", "");
                    }
                    _mapper.Map<CreateCategoryRequest, CategoryMaterial>(request, category);

                    await _unitOfWork.Repository<CategoryMaterial>().Update(category, id);
                    await _unitOfWork.CommitAsync();
                    return _mapper.Map<CategoryMaterial, CategoryResponse>(category);
                }
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Update category error!!!!!", ex.Message);
            }
        }
    }
}
