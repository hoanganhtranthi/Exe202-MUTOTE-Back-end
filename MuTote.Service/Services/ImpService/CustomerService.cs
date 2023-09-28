using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Verify.V2.Service;

namespace MuTote.Service.Services.ImpService
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private string accountSID, authToken,serviceID;
        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
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
        public async Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request)       {
            try
            {
                var customer = _mapper.Map<CreateCustomerRequest, Customer>(request);
                var s = _unitOfWork.Repository<Customer>().Find(s => s.Phone == request.Phone);
                if (s != null)
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Phone has already !!!", "");
                }
                var cus = _unitOfWork.Repository<Customer>().Find(s => s.Email == request.Email);
                if (cus != null)
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Email has already !!!", "");
                }
                CreatPasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                customer.PasswordHash = passwordHash;
                customer.PasswordSalt = passwordSalt;
                customer.Status = 1;
                await _unitOfWork.Repository<Customer>().CreateAsync(customer);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Customer, CustomerResponse>(customer);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Create Customer Error!!!", ex?.Message);
            }
        }
        public async Task<string> Verification(string request,string token)
        {
            try
            {
                #region checkIsUniquePhone
                var phone = _unitOfWork.Repository<Customer>().GetAll().Where(a => a.Phone == request).SingleOrDefault();
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
        public async Task<CustomerResponse> GetToUpdateStatus(int id)
        {
            try
            {
                Customer customer = null;
                customer = _unitOfWork.Repository<Customer>()
                    .Find(c => c.Id == id);

                if (customer == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, $"Not found customer with id{id.ToString()}", "");
                }
                customer.Status=0;
                await _unitOfWork.Repository<Customer>().Update(customer, customer.Id);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Customer, CustomerResponse>(customer);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Update customer error!!!!!", ex.Message);
            }
        }

        public async Task<CustomerResponse> GetCustomerById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Id Customer Invalid", "");
                }
                var response = await _unitOfWork.Repository<Customer>().GetAsync(u => u.Id == id);

                if (response == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, $"Not found customer with id {id.ToString()}", "");
                }

                return _mapper.Map<CustomerResponse>(response);
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

        public async Task<PagedResults<CustomerResponse>> GetCustomers(CustomerRequest request, PagingRequest paging)
        {
            try
            {

                var filter = _mapper.Map<CustomerResponse>(request);
                var customers = _unitOfWork.Repository<Customer>().GetAll()
                                           .ProjectTo<CustomerResponse>(_mapper.ConfigurationProvider)
                                           .DynamicFilter(filter)
                                           .ToList();
                var sort = PageHelper<CustomerResponse>.Sorting(paging.SortType, customers, paging.ColName);
                var result = PageHelper<CustomerResponse>.Paging(sort, paging.Page, paging.PageSize);
                return result;
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get customer list error!!!!!", ex.Message);
            }
        }

        public async Task<string> GetJwt(int accountId)
        {
            try
            {
                var account = await _unitOfWork.Repository<Customer>().GetById(accountId);
                if (account == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, $"Not found customer with id {accountId}", "");
                }
                return GenerateJwtToken(account);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get Jwt error!!!!!", ex.Message);
            }
        }

        public async Task<CustomerResponse> Login(LoginRequest request)
        {
            try
            {
                var user = _unitOfWork.Repository<Customer>().GetAll()
                   .FirstOrDefault(u => u.Phone.Equals(request.Phone.Trim()));

                if (user == null) throw new CrudException(HttpStatusCode.BadRequest, "User Not Found", "");
                if (!VerifyPasswordHash(request.Password.Trim(), user.PasswordHash, user.PasswordSalt))
                    throw new CrudException(HttpStatusCode.BadRequest, "Password is incorrect", "");
                if(user.Status==0) throw new CrudException(HttpStatusCode.BadRequest, "Your account is block", "");
                var cus = _mapper.Map<Customer, CustomerResponse>(user);
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

        public async Task<CustomerResponse> LoginByGoogle(string googleId)
        {
            try
            {
                var user = _unitOfWork.Repository<Customer>().GetAll()
                   .FirstOrDefault(u => u.GoogleId.Equals(googleId.Trim()) && u.Status==1);

                if (user == null) throw new CrudException(HttpStatusCode.BadRequest, $"User Not Found with googleId {googleId}", "");
                var cus = _mapper.Map<Customer, CustomerResponse>(user);
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

        public async Task<CustomerResponse> UpdateCustomer(int customerId, UpdateCustomerRequest request)
        {
            try
            {
                Customer customer = null;
                customer = _unitOfWork.Repository<Customer>()
                    .Find(c => c.Id == customerId);

                var cus = _unitOfWork.Repository<Customer>()
                    .GetAll().Where(c => c.Phone.Equals(request.Phone)).SingleOrDefault();

                var rs = _unitOfWork.Repository<Customer>()
                    .GetAll().Where(c => c.Email.Equals(request.Email)).SingleOrDefault();
                if (customer == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, $"Not found customer with id{customerId.ToString()}", "");
                }
                if (cus != null && cus.Id != customerId)
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Phone has already !!!", "");
                }
                if (rs != null && rs.Id != customerId)
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Email has already !!!", "");
                }
                _mapper.Map<UpdateCustomerRequest, Customer>(request, customer);
                if (request.OldPassword != null && request.NewPassword != null)
                {
                    if (!VerifyPasswordHash(request.OldPassword.Trim(), cus.PasswordHash, cus.PasswordSalt))
                        throw new CrudException(HttpStatusCode.BadRequest, "Old Password is not match", "");
                    CreatPasswordHash(request.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
                    customer.PasswordHash = passwordHash;
                    customer.PasswordSalt = passwordSalt;
                }
                await _unitOfWork.Repository<Customer>().Update(customer, customer.Id);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Customer, CustomerResponse>(customer);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Update customer error!!!!!", ex.Message);
            }
        }

        public async Task<CustomerResponse> UpdatePass(ResetPasswordRequest request)
        {
            try
            {
                Customer customer = null;
                customer = _unitOfWork.Repository<Customer>()
                    .Find(c => c.Phone.Equals(request.Phone));
                if (customer == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, $"Not found customer with phone{request.Phone}", "");
                }
                CreatPasswordHash(request.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
                customer.PasswordHash = passwordHash;
                customer.PasswordSalt = passwordSalt;
                await _unitOfWork.Repository<Customer>().Update(customer, customer.Id);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Customer, CustomerResponse>(customer);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Update Password customer error!!!!!", ex.Message);
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
        private string GenerateJwtToken(Customer customer)
        {
            string role;
            string pass = _config["AdminAccount:Password"];
            if (customer.Phone.Equals(_config["AdminAccount:Phone"]) && VerifyPasswordHash(pass.Trim(), customer.PasswordHash, customer.PasswordSalt))
                role = "admin";
            else role = "customer";

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["ApiSetting:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Name , customer.Name),
                new Claim(ClaimTypes.Email , customer.Email),
                new Claim("ImageUrl", customer.Avatar ?? ""),
                new Claim("GoogleId", customer.GoogleId ?? ""),
                new Claim(ClaimTypes.MobilePhone , customer.Phone),
                }),
                Expires = DateTime.UtcNow.AddYears(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
