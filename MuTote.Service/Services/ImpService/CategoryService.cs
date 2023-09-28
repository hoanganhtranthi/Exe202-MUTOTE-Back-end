using AutoMapper;
using AutoMapper.QueryableExtensions;
using MuTote.Data.Enities;
using MuTote.Data.UnitOfWork;
using MuTote.Service.DTO.Request;
using MuTote.Service.DTO.Response;
using MuTote.Service.Exceptions;
using MuTote.Service.Helpers;
using MuTote.Service.Services.ISerive;
using MuTote.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.Services.ImpService
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

        public async Task<CategoryResponse> DeleteCategory(int id, Helpers.Enum.CategoryChoice choice)
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

        public async Task<CategoryResponse> GetCategoryById(int id, Helpers.Enum.CategoryChoice choice)
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

        public async Task<PagedResults<CategoryResponse>> GetCategorys(CategoryRequest request, PagingRequest paging, Helpers.Enum.CategoryChoice choice)
        {
            try
            {
                var filter = _mapper.Map<CategoryResponse>(request);
                IList<CategoryResponse> categorys = new List<CategoryResponse>();
                if (choice.ToString().Equals("Product"))
                    categorys = _unitOfWork.Repository<CategoryProduct>().GetAll()
                                           .ProjectTo<CategoryResponse>(_mapper.ConfigurationProvider)
                                           .DynamicFilter(filter)
                                           .ToList();
                else categorys = _unitOfWork.Repository<CategoryMaterial>().GetAll()
                                           .ProjectTo<CategoryResponse>(_mapper.ConfigurationProvider)
                                           .DynamicFilter(filter)
                                           .ToList();
                var sort = PageHelper<CategoryResponse>.Sorting(paging.SortType, categorys, paging.ColName);
                var result = PageHelper<CategoryResponse>.Paging(sort, paging.Page, paging.PageSize);
                return result;
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get category list error!!!!!", ex.Message);
            }
        }

        public async Task<CategoryResponse> InsertCategory(CreateCategoryRequest category, Helpers.Enum.CategoryChoice choice)
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

        public async Task<CategoryResponse> UpdateCategory(int id, CreateCategoryRequest request, Helpers.Enum.CategoryChoice choice)
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
