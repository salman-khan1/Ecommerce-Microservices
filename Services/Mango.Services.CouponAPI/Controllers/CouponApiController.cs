using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponApiController : ControllerBase
    {

        private readonly AppDbContext dbContext;
        private ResponseDto _responseDto;
        IMapper _mapper;

        public CouponApiController(AppDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            _responseDto = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDto Get() {
            try
            {
                IEnumerable<Coupon> objList = dbContext.Coupons.ToList();
                 _responseDto.Result=_mapper.Map<IEnumerable<CouponDto>>(objList);
            }
            catch (Exception ex)
            {
              _responseDto.Success = false;
              _responseDto.Message = ex.Message;

            }
            return _responseDto;
        }
        [HttpGet("{id}")]
        public ResponseDto Get(int id) {
            try
            {
                Coupon obj = dbContext.Coupons.First(x=>x.CouponId==id);
                //CouponDto couponDto = new CouponDto()
                //{
                //    CouponId= obj.CouponId,
                //    CouponCode=obj.CouponCode,
                //    DiscountAmount=obj.DiscountAmount,
                //    MinAmount=obj.MinAmount,

                //}; 
                _responseDto.Result = _mapper.Map<Coupon>(obj);
               
            }
            catch (Exception ex)
            {

                _responseDto.Success = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpGet("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {
                Coupon obj = dbContext.Coupons.FirstOrDefault(x => x.CouponCode.ToLower() == code.ToLower());
                if (obj == null)
                {
                    _responseDto.Success=false;
                }
                //CouponDto couponDto = new CouponDto()
                //{
                //    CouponId= obj.CouponId,
                //    CouponCode=obj.CouponCode,
                //    DiscountAmount=obj.DiscountAmount,
                //    MinAmount=obj.MinAmount,

                //}; 
                _responseDto.Result = _mapper.Map<Coupon>(obj);

            }
            catch (Exception ex)
            {

                _responseDto.Success = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Post([FromBody] CouponDto couponDto) {

            try
            {
                Coupon couponObj = _mapper.Map<Coupon>(couponDto);
                dbContext.Coupons.Add(couponObj);
                dbContext.SaveChanges();
                _responseDto.Result = _mapper.Map<CouponDto>(couponObj);
            }
            catch (Exception ex)
            {

                _responseDto.Success = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        } 
        
        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] CouponDto couponDto) {

            try
            {
                Coupon couponObj = _mapper.Map<Coupon>(couponDto);
                dbContext.Coupons.Update(couponObj);
                dbContext.SaveChanges();
                _responseDto.Result = _mapper.Map<CouponDto>(couponObj);
            }
            catch (Exception ex)
            {

                _responseDto.Success = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        } 
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id) {

            try
            {
                Coupon couponObj = dbContext.Coupons.First(x => x.CouponId == id);
                dbContext.Coupons.Remove(couponObj);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                _responseDto.Success = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
    }
}
