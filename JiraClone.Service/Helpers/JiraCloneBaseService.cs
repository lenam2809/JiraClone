using AutoMapper.QueryableExtensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Service.Helpers
{
    public class JiraCloneBaseService : BaseService
    {
        public JiraCloneBaseService(EPSRepository repository, IMapper mapper) : base(repository, mapper)
        {

        }

        public override PagingResult<TDto> FilterPaged<TEntity, TDto>(PagingParams<TDto> pagingParams, params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class
        {
            if (pagingParams == null)
            {
                throw new ArgumentNullException("pagingParams");
            }

            var result = new PagingResult<TDto>() { PageSize = pagingParams.ItemsPerPage, CurrentPage = pagingParams.Page };

            IQueryable<TEntity> entityQuery = _repository.Filter<TEntity>();

            IQueryable<TDto> query = entityQuery.ProjectTo<TDto>(_mapper.ConfigurationProvider);

            var pagingPredicates = pagingParams.GetPredicates();
            if (pagingPredicates != null && pagingPredicates.Any())
            {
                query = query.WhereMany(pagingPredicates);
            }

            if (predicates != null && predicates.Any())
            {
                query = query.WhereMany(predicates);
            }

            result.TotalRows = query.Count();

            // Ordering
            if (pagingParams.SortExpression != null)
            {
                query = query.OrderBy(pagingParams.SortExpression);

                if (pagingParams.Start > 0)
                {
                    query = query.Skip(pagingParams.Start);
                }
                // Skipping only work after ordering
                else if (pagingParams.StartingIndex > 0)
                {
                    query = query.Skip(pagingParams.StartingIndex);
                }
            }

            if (pagingParams.ItemsPerPage != -1 && pagingParams.ItemsPerPage <= 0)
            {
                pagingParams.ItemsPerPage = 100;
            }

            if (pagingParams.ItemsPerPage > 0)
            {
                query = query.Take(pagingParams.ItemsPerPage);
            }

            result.Data = query.ToList();

            return result;
        }

        public override PagingResult<TDto> FilterPaged<TEntity, TDto>(Expression<Func<TEntity, TDto>> mapping, PagingParams<TDto> pagingParams, params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class
        {
            if (pagingParams == null)
            {
                throw new ArgumentNullException("pagingParams");
            }

            var result = new PagingResult<TDto>() { PageSize = pagingParams.ItemsPerPage, CurrentPage = pagingParams.Page };

            IQueryable<TEntity> entityQuery = _repository.Filter<TEntity>();

            //if (pagingParams is IUnitTraversal<TEntity>)
            //{
            //    (pagingParams as IUnitTraversal<TEntity>).Traversing(_repository.All<UnitAncestor>(), ref entityQuery);
            //}

            IQueryable<TDto> query = entityQuery.Select(mapping);

            var pagingPredicates = pagingParams.GetPredicates();
            if (pagingPredicates != null && pagingPredicates.Any())
            {
                query = query.WhereMany(pagingPredicates);
            }

            if (predicates != null && predicates.Any())
            {
                query = query.WhereMany(predicates);
            }

            result.TotalRows = query.Count();

            // Ordering
            if (pagingParams.SortExpression != null)
            {
                if (pagingParams.SortBy == "NEWID")
                {
                    query = query.OrderBy(x => Guid.NewGuid());
                }
                else
                {
                    query = query.OrderBy(pagingParams.SortExpression);
                }

                // Skipping only work after ordering
                if (pagingParams.Start > 0)
                {
                    query = query.Skip(pagingParams.Start);
                }
                // Skipping only work after ordering
                else if (pagingParams.StartingIndex > 0)
                {
                    query = query.Skip(pagingParams.StartingIndex);
                }
            }

            if (pagingParams.ItemsPerPage != -1 && pagingParams.ItemsPerPage <= 0)
            {
                pagingParams.ItemsPerPage = 100;
            }

            if (pagingParams.ItemsPerPage > 0)
            {
                query = query.Take(pagingParams.ItemsPerPage);
            }

            result.Data = query.ToList();

            return result;
        }

        public async override Task<PagingResult<TDto>> FilterPagedAsync<TEntity, TDto>(PagingParams<TDto> pagingParams, params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class
        {
            if (pagingParams == null)
            {
                throw new ArgumentNullException("pagingParams");
            }

            var result = new PagingResult<TDto>() { PageSize = pagingParams.ItemsPerPage, CurrentPage = pagingParams.Page };

            IQueryable<TEntity> entityQuery = _repository.Filter<TEntity>();

            //if (pagingParams is IUnitTraversal<TEntity>)
            //{
            //    (pagingParams as IUnitTraversal<TEntity>).Traversing(_repository.All<UnitAncestor>(), ref entityQuery);
            //}

            IQueryable<TDto> query = entityQuery.ProjectTo<TDto>(_mapper.ConfigurationProvider);

            var pagingPredicates = pagingParams.GetPredicates();
            if (pagingPredicates != null && pagingPredicates.Any())
            {
                query = query.WhereMany(pagingPredicates);
            }

            if (predicates != null && predicates.Any())
            {
                query = query.WhereMany(predicates);
            }

            result.TotalRows = await query.CountAsync();
            // Ordering
            if (pagingParams.SortExpression != null)
            {
                if (pagingParams.SortBy == "NEWID")
                {
                    query = query.OrderBy(x => Guid.NewGuid());
                }
                else
                {
                    query = query.OrderBy(pagingParams.SortExpression);
                }
                if (pagingParams.Start > 0)
                {
                    query = query.Skip(pagingParams.Start);
                }
                // Skipping only work after ordering
                else if (pagingParams.StartingIndex > 0)
                {
                    query = query.Skip(pagingParams.StartingIndex);
                }
            }

            if (pagingParams.ItemsPerPage != -1 && pagingParams.ItemsPerPage <= 0)
            {
                pagingParams.ItemsPerPage = 100;
            }

            if (pagingParams.ItemsPerPage > 0)
            {
                query = query.Take(pagingParams.ItemsPerPage);
            }
            result.Data = await query.ToListAsync();

            return result;
        }

        public async Task<List<TDto>> GetDataAsync<TEntity, TDto>(PagingParams<TDto> pagingParams, params Expression<Func<TDto, bool>>[] predicates)
           where TEntity : class
        {
            if (pagingParams == null)
            {
                throw new ArgumentNullException("pagingParams");
            }



            IQueryable<TEntity> entityQuery = _repository.Filter<TEntity>();

            //if (pagingParams is IUnitTraversal<TEntity>)
            //{
            //    (pagingParams as IUnitTraversal<TEntity>).Traversing(_repository.All<UnitAncestor>(), ref entityQuery);
            //}

            IQueryable<TDto> query = entityQuery.ProjectTo<TDto>(_mapper.ConfigurationProvider);

            var pagingPredicates = pagingParams.GetPredicates();
            if (pagingPredicates != null && pagingPredicates.Any())
            {
                query = query.WhereMany(pagingPredicates);
            }

            if (predicates != null && predicates.Any())
            {
                query = query.WhereMany(predicates);
            }


            // Ordering
            if (pagingParams.SortExpression != null)
            {
                if (pagingParams.SortBy == "NEWID")
                {
                    query = query.OrderBy(x => Guid.NewGuid());
                }
                else
                {
                    query = query.OrderBy(pagingParams.SortExpression);
                }
                if (pagingParams.Start > 0)
                {
                    query = query.Skip(pagingParams.Start);
                }
                // Skipping only work after ordering
                else if (pagingParams.StartingIndex > 0)
                {
                    query = query.Skip(pagingParams.StartingIndex);
                }
            }

            if (pagingParams.ItemsPerPage != -1 && pagingParams.ItemsPerPage <= 0)
            {
                pagingParams.ItemsPerPage = 100;
            }

            if (pagingParams.ItemsPerPage > 0)
            {
                query = query.Take(pagingParams.ItemsPerPage);
            }
            return await query.ToListAsync();


        }
        public async override Task<PagingResult<TDto>> FilterPagedAsync<TEntity, TDto>(Expression<Func<TEntity, TDto>> mapping, PagingParams<TDto> pagingParams, params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class
        {
            if (pagingParams == null)
            {
                throw new ArgumentNullException("pagingParams");
            }

            var result = new PagingResult<TDto>() { PageSize = pagingParams.ItemsPerPage, CurrentPage = pagingParams.Page };

            IQueryable<TEntity> entityQuery = _repository.Filter<TEntity>();

            //if (pagingParams is IUnitTraversal<TEntity>)
            //{
            //    (pagingParams as IUnitTraversal<TEntity>).Traversing(_repository.All<UnitAncestor>(), ref entityQuery);
            //}

            IQueryable<TDto> query = entityQuery.Select(mapping);

            var pagingPredicates = pagingParams.GetPredicates();
            if (pagingPredicates != null && pagingPredicates.Any())
            {
                query = query.WhereMany(pagingPredicates);
            }

            if (predicates != null && predicates.Any())
            {
                query = query.WhereMany(predicates);
            }

            result.TotalRows = await query.CountAsync();

            // Ordering
            if (pagingParams.SortExpression != null)
            {
                query = query.OrderBy(pagingParams.SortExpression);

                if (pagingParams.Start > 0)
                {
                    query = query.Skip(pagingParams.Start);
                }
                // Skipping only work after ordering
                else if (pagingParams.StartingIndex > 0)
                {
                    query = query.Skip(pagingParams.StartingIndex);
                }
            }

            if (pagingParams.ItemsPerPage != -1 && pagingParams.ItemsPerPage <= 0)
            {
                pagingParams.ItemsPerPage = 100;
            }

            if (pagingParams.ItemsPerPage > 0)
            {
                query = query.Take(pagingParams.ItemsPerPage);
            }

            result.Data = await query.ToListAsync();

            return result;
        }
        /// <summary>
        /// Cannb
        /// </summary>
        /// <param name="files"></param>
        /// <param name="TaiKhoanID"></param>
        /// <returns></returns>
        public async Task<string> CreateFile(List<IFormFile> files, int TaiKhoanID)
        {
            List<EPS.Data.Entities.Attachment> lstFileUpload = new List<EPS.Data.Entities.Attachment>();
            FileUploadCreateDto oFileUpload;
            foreach (var file in files)
            {
                if (file != null && file.Length > 0)
                {
                    oFileUpload = new FileUploadCreateDto(file);
                    if (TaiKhoanID > 0)
                    {
                        oFileUpload.UserId = TaiKhoanID;
                    }
                    await CreateAsync<FileUpload, FileUploadCreateDto>(oFileUpload);
                    lstFileUpload.Add(new EPS.Data.Entities.Attachment() { Id = oFileUpload.Id, Title = oFileUpload.Title, Url = "fileupload/download/" + oFileUpload.IdGuid, UrlView = "fileupload/view/" + oFileUpload.IdGuid + "/" + oFileUpload.Title, Type = oFileUpload.Type, Format = oFileUpload.Title.Substring(oFileUpload.Title.LastIndexOf(".")) });
                }

            }
            return System.Text.Json.JsonSerializer.Serialize(lstFileUpload);
        }


        public async Task<string> UpdateFileDK(List<IFormFile> files, int TaiKhoanID, List<Attachment> LstAttachments, List<int> LstAttachDeletes)
        {
            #region Xóa -
            FileUploadCreateDto oFileUpload;
            if (LstAttachDeletes.Count > 0)
            {
                foreach (int item in LstAttachDeletes)
                    await DeleteAsync<FileUpload, int>(item);

                LstAttachments = LstAttachments.Where(x => !LstAttachDeletes.Contains(x.Id)).ToList();

            }
            #endregion
            #region  Thêm mới file đính kèm
            foreach (var file in files)
            {
                try
                {
                    if (file != null && file.Length > 0)
                    {
                        oFileUpload = new FileUploadCreateDto(file);
                        if (TaiKhoanID > 0)
                        {
                            oFileUpload.UserId = TaiKhoanID;
                        }
                        Create<FileUpload, FileUploadCreateDto>(oFileUpload);
                        LstAttachments.Add(new Attachment() { Id = oFileUpload.Id, Title = oFileUpload.Title, Url = "fileupload/download/" + oFileUpload.IdGuid, UrlView = "fileupload/view/" + oFileUpload.IdGuid + "/" + oFileUpload.Title, Type = oFileUpload.Type, Format = oFileUpload.Title.Substring(oFileUpload.Title.LastIndexOf(".")) });
                    }
                }
                catch (Exception e)
                {

                }

            }
            #endregion


            return System.Text.Json.JsonSerializer.Serialize(LstAttachments);
        }
        public async Task<string> ChangeDK(List<IFormFile> files, int TaiKhoanID, List<Attachment> LstAttachments, List<int> LstAttachDeletes)
        {
            List<Attachment> LstAttachmentReturn = new List<Attachment>();

            #region Xóa -
            if (LstAttachDeletes.Count > 0)
            {
                LstAttachments = LstAttachments.Where(x => !LstAttachDeletes.Contains(x.Id)).ToList();
            }
            #endregion            
            //Copy file
            if (LstAttachments.Count > 0)
            {
                foreach (var item in LstAttachments)
                {
                    var oFileUploadCreateDto = await FindAsync<FileUpload, FileUploadUpdateDto>(item.Id);
                    if (oFileUploadCreateDto != null)
                    {
                        oFileUploadCreateDto.Id = 0;
                        if (TaiKhoanID > 0)
                        {
                            oFileUploadCreateDto.UserId = TaiKhoanID;
                        }
                        oFileUploadCreateDto.IdGuid = Guid.NewGuid().ToString();
                        Create<FileUpload, FileUploadUpdateDto>(oFileUploadCreateDto);
                        LstAttachmentReturn.Add(new Attachment() { Id = oFileUploadCreateDto.Id, Title = oFileUploadCreateDto.Title, Url = "fileupload/download/" + oFileUploadCreateDto.IdGuid, UrlView = "fileupload/view/" + oFileUploadCreateDto.IdGuid + "/" + oFileUploadCreateDto.Title, Type = oFileUploadCreateDto.Type, Format = oFileUploadCreateDto.Title.Substring(oFileUploadCreateDto.Title.LastIndexOf(".")) });
                    }
                }
            }

            #region  Thêm mới file đính kèm
            FileUploadCreateDto oFileUpload;
            foreach (var file in files)
            {
                try
                {
                    if (file != null && file.Length > 0)
                    {
                        oFileUpload = new FileUploadCreateDto(file);
                        if (TaiKhoanID > 0)
                        {
                            oFileUpload.UserId = TaiKhoanID;
                        }
                        Create<FileUpload, FileUploadCreateDto>(oFileUpload);
                        LstAttachmentReturn.Add(new Attachment() { Id = oFileUpload.Id, Title = oFileUpload.Title, Url = "fileupload/download/" + oFileUpload.IdGuid, UrlView = "fileupload/view/" + oFileUpload.IdGuid + "/" + oFileUpload.Title, Type = oFileUpload.Type, Format = oFileUpload.Title.Substring(oFileUpload.Title.LastIndexOf(".")) });
                    }
                }
                catch (Exception e)
                {

                }

            }
            #endregion


            return System.Text.Json.JsonSerializer.Serialize(LstAttachmentReturn);
        }
        public async Task UpdateFilePermiss(string attachment, int permisson)
        {
            if (!string.IsNullOrEmpty(attachment))
            {
                List<Attachment> lstAttachment = JsonConvert.DeserializeObject<List<Attachment>>(attachment);
                FileUploadPermissonDto oFileUploadPermissonDto;
                foreach (var item in lstAttachment)
                {
                    oFileUploadPermissonDto = new FileUploadPermissonDto
                    {
                        Id = item.Id,
                        Permission = permisson
                    };
                    await UpdateAsync<FileUpload, FileUploadPermissonDto>(item.Id, oFileUploadPermissonDto);
                }
            }
        }
        //Cập nhật lại trường isdraft cho file
        public async Task UpdateIsDraft(int id)
        {
            FileUploadDraftDto oUpdate = await FindAsync<FileUpload, FileUploadDraftDto>(id);
            if (oUpdate != null)
            {
                oUpdate.IsDraft = false;
                await UpdateAsync<FileUpload, FileUploadDraftDto>(oUpdate.Id, oUpdate);
            }
        }
        //Add thông báo tự động
        public async Task AddNotificationAuto(string tieuDe, List<int> DonViIds, string noiDung, List<IFormFile> files = null, int TaiKhoanID = 0)
        {
            if (DonViIds.Count > 0)
            {
                NotificationCreateDto item = new NotificationCreateDto();
                item.CreatedDate = DateTime.Now;
                item.LoaiThongBao = (int)LoaiThongBao.TuDong;
                item.Nam = DateTime.Now.Year;
                item.DateFrom = DateTime.Now;
                item.DateTo = DateTime.Now.AddDays(7);
                item.TieuDe = tieuDe;
                item.NoiDung = noiDung;
                item.DonViIds = "," + string.Join(",", DonViIds.Select(x => x)) + ",";
                if (files != null && files.Count > 0)
                {
                    item.FileDinhKem = await CreateFile(files, TaiKhoanID);
                }
                await CreateAsync<Notification, NotificationCreateDto>(item);
                foreach (var dVId in DonViIds)
                {
                    if (dVId > 0)
                    {
                        await CreateAsync<ThongBaoDonVi, ThongBaoDonViCreateDto>(new ThongBaoDonViCreateDto() { DonViId = dVId, NotificationId = item.Id });
                    }
                }
                //Thêm code Add Log thành công
            }
            else
            {
                //Ghi Log code Add Log thất bại
            }
        }


        //Log phương án sắp xếp tài sản
        public async Task AddLogPASX(int DonViId, int ThaoTac, int TrangThai, int PhuongAnSapXepId)
        {
            if (DonViId > 0 && ThaoTac > 0 && TrangThai > 0 && PhuongAnSapXepId > 0)
            {
                LichSuSapXepTaiSanCreateDto item = new LichSuSapXepTaiSanCreateDto();
                item.NgayTao = DateTime.Now;
                item.ThaoTacThucHien = ThaoTac;
                item.TrangThai = TrangThai;
                item.PhuongAnSapXepId = PhuongAnSapXepId;
                item.DonViId = DonViId;
                await CreateAsync<LichSuSapXepTaiSan, LichSuSapXepTaiSanCreateDto>(item);
            }
        }
    }
}
