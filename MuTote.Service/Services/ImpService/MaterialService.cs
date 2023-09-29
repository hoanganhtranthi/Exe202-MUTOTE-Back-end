using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
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
    public class MaterialService : IMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MaterialService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<MaterialResponse> GetMaterialById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Id Material Invalid", "");
                }
                var response = _unitOfWork.Repository<Material>().GetAll().Include(c => c.CategoryMaterial).Include(c=>c.Designer).Where(u => u.Id == id).SingleOrDefault();

                if (response == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, $"Not found material with id {id.ToString()}", "");
                }

                return _mapper.Map<MaterialResponse>(response);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get Material By ID Error!!!", ex.InnerException?.Message);
            }
        }

        public async Task<PagedResults<MaterialResponse>> GetMaterials(MaterialRequest request, PagingRequest paging)
        {
            try
            {

                var filter = _mapper.Map<MaterialResponse>(request);
                var materials = _unitOfWork.Repository<Material>().GetAll().Include(c=>c.CategoryMaterial).Include(c => c.Designer)
                                           .ProjectTo<MaterialResponse>(_mapper.ConfigurationProvider)
                                           .DynamicFilter(filter)
                                           .ToList();
                var sort = PageHelper<MaterialResponse>.Sorting(paging.SortType, materials, paging.ColName);
                var result = PageHelper<MaterialResponse>.Paging(sort, paging.Page, paging.PageSize);
                return result;
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get material list error!!!!!", ex.Message);
            }
        }

        public async Task<MaterialResponse> InsertMaterial(CreateMaterialRequest material)
        {
            try
            {
                    var materialRequest =  _unitOfWork.Repository<Material>().GetAll().Include(c=>c.CategoryMaterial).Include(c => c.Designer).Where(u => u.Name == material.Name).SingleOrDefault();
                    if (material == null)
                    {
                        throw new CrudException(HttpStatusCode.BadRequest, "Material Invalid!!!", "");
                    }
                    if (materialRequest != null)
                    {
                        throw new CrudException(HttpStatusCode.BadRequest, "Material has already insert !!!", "");
                    }

                    var response = _mapper.Map<CreateMaterialRequest, Material>(material);
                    await _unitOfWork.Repository<Material>().CreateAsync(response);
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<MaterialResponse>(response);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Insert Material Error!!!", ex.InnerException?.Message);
            }
        }

        public async Task<MaterialResponse> DeleteMaterial(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Id Material Invalid", "");
                }
                var response =  _unitOfWork.Repository<Material>().GetAll().Include(c => c.CategoryMaterial).Include(c => c.Designer).Where(u => u.Id==id).SingleOrDefault();

                if (response == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, $"Not found material with id {id.ToString()}", "");
                }
               _unitOfWork.Repository<Material>().Delete(response);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<MaterialResponse>(response);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Delete Material By ID Error!!!", ex.InnerException?.Message);
            }
        }
    }
}
