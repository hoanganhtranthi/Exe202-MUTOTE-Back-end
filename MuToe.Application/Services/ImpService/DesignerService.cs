
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MuTote.Application.DTO.Request;
using MuTote.Application.DTO.Response;
using MuTote.Application.GlobalExceptionHandling.Exceptions;
using MuTote.Application.Helpers;
using MuTote.Application.Services.ISerive;
using MuTote.Application.UnitOfWork;
using MuTote.Application.Utilities;
using MuTote.Domain.Enities;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Twilio;
using Twilio.Rest.Verify.V2.Service;

namespace MuTote.Application.Services.ImpService
{
    public class DesignerService:IDesignerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private string accountSID, authToken, serviceID;
        public DesignerService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _config = configuration;
            accountSID = _config["Twilio:AccountSID"];
            authToken = _config["Twilio:AuthToken"];
            serviceID = _config["Twilio:PathServiceSid"];
        }
        public static bool CheckVNPhone(string phoneNumber)
        {
            string strRegex = @"(^(0)(3[2-9]|5[6|8|9]|7[0|6-9]|8[0-6|8|9]|9[0-4|6-9])[0-9]{7}$)";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(phoneNumber))
            {
                return true;
            }
            else
                return false;
        }
        public async Task<DesignerResponse> CreateDesigner(CreateDesignerRequest request)
        {
            try
            {
                var designer = _mapper.Map<CreateDesignerRequest, Designer>(request);
                var s = _unitOfWork.Repository<Designer>().Find(s => s.Phone == request.Phone);
                if (s != null)
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Phone has already !!!", "");
                }
                var cus = _unitOfWork.Repository<Designer>().Find(s => s.Email == request.Email);
                if (cus != null)
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Email has already !!!", "");
                }
                CreatPasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                designer.PasswordHash = passwordHash;
                designer.PasswordSalt = passwordSalt;
                designer.Status = 1;
                await _unitOfWork.Repository<Designer>().CreateAsync(designer);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Designer, DesignerResponse>(designer);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Create Designer Error!!!", ex?.Message);
            }
        }
        public async Task<string> Verification(string request, string token)
        {
            try
            {
                #region checkIsUniquePhone
                var phone = _unitOfWork.Repository<Designer>().GetAll().Where(a => a.Phone == request).SingleOrDefault();
                if (phone != null) throw new CrudException(HttpStatusCode.BadRequest, "Phone has already, please login", request.ToString());
                #endregion
                else
                {
                    #region checkPhone
                    var checkPhone = CheckVNPhone(request);
                    if (checkPhone)
                    {
                        if (!request.StartsWith("+84"))
                        {
                            request = request.TrimStart(new char[] { '0' });
                            request = "+84" + request;
                        }
                    }
                    else
                    {
                        throw new CrudException(HttpStatusCode.BadRequest, "Wrong Phone", request.ToString());
                    }
                    #endregion
                    TwilioClient.Init(accountSID, authToken);

                    if (token != null)
                    {
                        var verificationCheck = VerificationCheckResource.Create(
                        to: request,
                        code: token,
                        pathServiceSid: serviceID
                    );

                        return verificationCheck.Status;
                    }
                    else
                    {
                        var verification = VerificationResource.Create(
                            channel: "sms",
                            to: request,
                            pathServiceSid: serviceID
                    );
                        return verification.Status;
                    }
                }
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Verification Error!!!", ex.InnerException?.Message);
            }
        }
        public async Task<DesignerResponse> GetToUpdateStatus(int id)
        {
            try
            {
                Designer designer = null;
                designer = _unitOfWork.Repository<Designer>()
                    .Find(c => c.Id == id);

                if (designer == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, $"Not found designer with id{id.ToString()}", "");
                }
                designer.Status = 0;
                await _unitOfWork.Repository<Designer>().Update(designer, designer.Id);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Designer, DesignerResponse>(designer);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Update designer error!!!!!", ex.Message);
            }
        }

        public async Task<DesignerResponse> GetDesignerById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Id Designer Invalid", "");
                }
                var response = await _unitOfWork.Repository<Designer>().GetAsync(u => u.Id == id);

                if (response == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, $"Not found designer with id {id.ToString()}", "");
                }

                return _mapper.Map<DesignerResponse>(response);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get User By ID Error!!!", ex.InnerException?.Message);
            }
        }

        public async Task<PagedResults<DesignerResponse>> GetDesigners(DesignerRequest request, PagingRequest paging)
        {
            try
            {

                var filter = _mapper.Map<DesignerResponse>(request);
                var designers = _unitOfWork.Repository<Designer>().GetAll()
                                           .ProjectTo<DesignerResponse>(_mapper.ConfigurationProvider)
                                           .DynamicFilter(filter)
                                           .ToList();
                var sort = PageHelper<DesignerResponse>.Sorting(paging.SortType, designers, paging.ColName);
                var result = PageHelper<DesignerResponse>.Paging(sort, paging.Page, paging.PageSize);
                return result;
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get designer list error!!!!!", ex.Message);
            }
        }

       

        public async Task<DesignerResponse> Login(LoginRequest request)
        {
            try
            {
                var user = _unitOfWork.Repository<Designer>().GetAll()
                   .FirstOrDefault(u => u.Phone.Equals(request.Phone.Trim()));

                if (user == null) throw new CrudException(HttpStatusCode.BadRequest, "Designer Not Found", "");
                if (!VerifyPasswordHash(request.Password.Trim(), user.PasswordHash, user.PasswordSalt))
                    throw new CrudException(HttpStatusCode.BadRequest, "Password is incorrect", "");
                if (user.Status == 0) throw new CrudException(HttpStatusCode.BadRequest, "Your account is block", "");
                var cus = _mapper.Map<Designer, DesignerResponse>(user);
                cus.Token = GenerateJwtToken(user);
                return cus;
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Progress Error!!!", ex.InnerException?.Message);
            }
        }

      

        public async Task<DesignerResponse> UpdateDesigner(int designerId, UpdateDesignerRequest request)
        {
            try
            {
                Designer designer = null;
                designer = _unitOfWork.Repository<Designer>()
                    .Find(c => c.Id == designerId);

                var cus = _unitOfWork.Repository<Designer>()
                    .GetAll().Where(c => c.Phone.Equals(request.Phone)).SingleOrDefault();

                var rs = _unitOfWork.Repository<Designer>()
                    .GetAll().Where(c => c.Email.Equals(request.Email)).SingleOrDefault();
                if (designer == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, $"Not found designer with id{designerId.ToString()}", "");
                }
                if (cus != null && cus.Id != designerId)
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Phone has already !!!", "");
                }
                if (rs != null && rs.Id != designerId)
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Email has already !!!", "");
                }
                _mapper.Map<UpdateDesignerRequest, Designer>(request, designer);
                if (request.OldPassword != null && request.NewPassword != null)
                {
                    if (!VerifyPasswordHash(request.OldPassword.Trim(), cus.PasswordHash, cus.PasswordSalt))
                        throw new CrudException(HttpStatusCode.BadRequest, "Old Password is not match", "");
                    CreatPasswordHash(request.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
                    designer.PasswordHash = passwordHash;
                    designer.PasswordSalt = passwordSalt;
                }
                await _unitOfWork.Repository<Designer>().Update(designer, designerId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Designer, DesignerResponse>(designer);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Update designer error!!!!!", ex.Message);
            }
        }

        public async Task<DesignerResponse> UpdatePass(ResetPasswordRequest request)
        {
            try
            {
                Designer designer = null;
                designer = _unitOfWork.Repository<Designer>()
                    .Find(c => c.Phone.Equals(request.Phone));
                if (designer == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, $"Not found designer with phone{request.Phone}", "");
                }
                CreatPasswordHash(request.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
                designer.PasswordHash = passwordHash;
                designer.PasswordSalt = passwordSalt;
                await _unitOfWork.Repository<Designer>().Update(designer, designer.Id);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Designer, DesignerResponse>(designer);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Update Password designer error!!!!!", ex.Message);
            }
        }
        private void CreatPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string GenerateJwtToken(Designer designer)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["ApiSetting:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, designer.Id.ToString()),
                new Claim(ClaimTypes.Role, "designer"),
                new Claim(ClaimTypes.Name , designer.Name),
                new Claim(ClaimTypes.Email , designer.Email),
                new Claim("ImageUrl", designer.Avatar ?? ""),
                new Claim("GoogleId", designer.GoogleId ?? ""),
                new Claim(ClaimTypes.MobilePhone , designer.Phone),
                }),
                Expires = DateTime.UtcNow.AddYears(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
