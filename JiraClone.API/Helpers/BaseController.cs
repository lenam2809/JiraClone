using AutoMapper;
using JiraClone.Data.Enums;
using JiraClone.Utils.BaseService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JiraClone.API.Helpers
{
    [ApiController]
    public class BaseController : ControllerBase
    {

        private Lazier<BaseService> _baseService;
        protected BaseService BaseService
        {
            get
            {
                if (_baseService == null) _baseService = new Lazier<BaseService>(HttpContext.RequestServices);

                return _baseService.Value;
            }
        }

        private Lazier<IMapper> _mapper;
        protected IMapper Mapper
        {
            get
            {
                if (_mapper == null) _mapper = new Lazier<IMapper>(HttpContext.RequestServices);

                return _mapper.Value;
            }
        }

        private Lazier<ILogger<BaseController>> _logger;
        protected ILogger<BaseController> Logger
        {
            get
            {
                if (_logger == null) _logger = new Lazier<ILogger<BaseController>>(HttpContext.RequestServices);

                return _logger.Value;
            }
        }
        
        public BaseController()
        {


        }

        #region Private
        [NonAction]
        public string GetTenTrangThai(int trangThai)
        {
            string tenTrangThai = trangThai switch
            {
                1 => "Chờ duyệt",
                2 => "Đã duyệt",
                3 => "Từ chối",
                4 => "Đã cập nhật",
                _ => "Đã đồng bộ",
            };
            return tenTrangThai;
        }
        #endregion


    }
    public class Lazier<T> : Lazy<T> where T : class
    {
        public Lazier(IServiceProvider provider)
            : base(() => provider.GetRequiredService<T>())
        {
        }
    }
}
