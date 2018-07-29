using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Moss.Hospital.Data.Entities;
using Moss.Hospital;
using System.Threading.Tasks;

namespace Moss.Hospital.Data.Cache
{
    public class GlobalCacheService
    {
        static bool loaded = false;
        public static bool IsLoadCache
        {
            get
            {
                string useCache = System.Configuration.ConfigurationManager.AppSettings["UseCache"];
                return useCache == null ? false : useCache.Equals("1");
            }
        }
        public void LoadCache(CacheType _cacheType)
        {
            if (IsLoadCache)
            {
                if (_cacheType == CacheType.All)
                {
                    if (!loaded)
                    {
                        MossHospitalEntities dbContext = new MossHospitalEntities();
                        List<CacheType> caches = new List<CacheType>();
                        caches.Add(CacheType.Department);
                        caches.Add(CacheType.DMBenhVien);
                        caches.Add(CacheType.DMDichVu);
                        caches.Add(CacheType.DMDichVuCL);
                        caches.Add(CacheType.DMDuoc);
                        caches.Add(CacheType.DMICD10);
                        caches.Add(CacheType.DMNhomDichVu);
                        caches.Add(CacheType.Employee);
                        caches.Add(CacheType.Permission);
                        caches.Add(CacheType.DMTinhThanh);
                        caches.Add(CacheType.DMQuanHuyen);
                        caches.Add(CacheType.DMXaPhuong);
                        caches.Add(CacheType.DMDanhMuc);
                        caches.Add(CacheType.PatientsObject);
                        caches.Add(CacheType.Feature);
                        caches.Add(CacheType.AssetsType);
                        caches.Add(CacheType.AssetsCate);
                        caches.Add(CacheType.Asset);
                        caches.Add(CacheType.DMMucHuong);
                        //this.LoadCache(CacheType.All, dbContext);
                        foreach (CacheType item in caches)
                        {
                            var task = Task.Run(() => {
                                LoadCache(item);
                            });
                            task.Wait();
                        }
                    }
                }
                else
                {
                    MossHospitalEntities dbContext = new MossHospitalEntities();
                    this.LoadCache(_cacheType, dbContext);
                }
                loaded = true;
            }
            
        }
        private void LoadCache(CacheType _cacheType, MossHospitalEntities _dbContext)
        {
            switch (_cacheType)
            {
                case CacheType.Permission:
                    GlobalCache.Permissions = _dbContext.Permissions.ToList();
                    break;
                case CacheType.Department:
                    GlobalCache.departments = _dbContext.Departments.ToList();
                    break;
                case CacheType.Employee:
                    GlobalCache.Employees = _dbContext.Employees.ToList();
                    break;
                case CacheType.DMDichVuCL:
                    GlobalCache.dMDichVuCLs = _dbContext.DMDichVuCLSangs.ToList();
                    break;
                case CacheType.DMDichVu:
                    GlobalCache.dMDichVus = _dbContext.DMDichVus.ToList();
                    break;
                case CacheType.DMDuoc:
                    GlobalCache.DMDuocs = _dbContext.DMDuocs.ToList();
                    break;
                case CacheType.DMNhomDichVu:
                    GlobalCache.dMNhomDichVus = _dbContext.DMNhomDichVus.ToList();
                    break;
                case CacheType.DMICD10:
                    GlobalCache.dMICD10s = _dbContext.DMICD10.ToList();
                    break;
                case CacheType.DMBenhVien:
                    GlobalCache.dMBenhViens = _dbContext.DMBenhViens.ToList();
                    break;
                case CacheType.All:
                    break;
                case CacheType.DMTinhThanh:
                    GlobalCache.DMTinhThanhs = _dbContext.DMTinhThanhs.ToList();
                    break;
                case CacheType.DMQuanHuyen:
                    GlobalCache.DMQuanHuyens = _dbContext.DMQuanHuyens.ToList();
                    break;
                case CacheType.DMXaPhuong:
                    GlobalCache.DMXaPhuongs = _dbContext.DMXaPhuongs.ToList();
                    break;
                case CacheType.DMDanhMuc:
                    GlobalCache.DMDanhmucs = _dbContext.DMDanhmucs.ToList();
                    break;
                case CacheType.PatientsObject:
                    GlobalCache.PatientsObjects = _dbContext.patientsObjects.ToList();
                    break;
                case CacheType.Feature:
                    GlobalCache.Features = _dbContext.Features.ToList();
                    break;
                case CacheType.SystemConfig:
                    GlobalCache.SystemConfigs = _dbContext.SystemConfigs.ToList();
                    break;
                case CacheType.AssetsType:
                    GlobalCache.AssetsTypes = _dbContext.AssetsTypes.ToList();
                    break;
                case CacheType.AssetsCate:
                    GlobalCache.AssetsCates = _dbContext.AssetsCates.ToList();
                    break;
                case CacheType.Asset:
                    GlobalCache.Assets = _dbContext.Assets.ToList();
                    break;
                case CacheType.DMMucHuong:
                    GlobalCache.DMMucHuongs = _dbContext.DMMucHuongs.ToList();
                    break;
                default:
                    break;
            }
        }

    }
}