using Moss.Hospital.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Moss.Hospital.Data.Cache
{
    /// <summary>
    ///Rule:
    ///Thêm 1 item: Thêm vào DB sau đó thêm vào cache
    ///Xóa 1 item: Xóa khỏi DB sau đó xóa khỏi cache
    ///Cập nhật 1 item: Cập nhật vào DB sau đó cập nhật vào cache
    ///Tìm 1 item: Tìm trên Cache
    /// </summary>
    public static class GlobalCache
    {
        public static List<Permission> Permissions = new List<Permission>();
        public static List<Feature> Features = new List<Feature>();
        public static List<Department> departments = new List<Department>();
        public static List<Employee> Employees = new List<Employee>();
        public static List<DMDichVuCLSang> dMDichVuCLs = new List<DMDichVuCLSang>();
        public static List<DMDichVu> dMDichVus = new List<DMDichVu>();
        public static List<DMDuoc> DMDuocs = new List<DMDuoc>();
        public static List<DMNhomDichVu> dMNhomDichVus = new List<DMNhomDichVu>();
        public static List<DMICD10> dMICD10s = new List<DMICD10>();
        public static List<DMBenhVien> dMBenhViens = new List<DMBenhVien>();
        public static List<DMTinhThanh> DMTinhThanhs = new List<DMTinhThanh>();
        public static List<DMQuanHuyen> DMQuanHuyens = new List<DMQuanHuyen>();
        public static List<DMXaPhuong> DMXaPhuongs = new List<DMXaPhuong>();
        public static List<DMDanhmuc> DMDanhmucs = new List<DMDanhmuc>();
        public static List<patientsObject> PatientsObjects = new List<patientsObject>();
        public static List<SystemConfig> SystemConfigs = new List<SystemConfig>();
        public static List<AssetsType> AssetsTypes = new List<AssetsType>();
        public static List<AssetsCate> AssetsCates = new List<AssetsCate>();
        public static List<Asset> Assets = new List<Asset>();
        public static List<DMMucHuong> DMMucHuongs = new List<DMMucHuong>();
        
    }
    public enum CacheType
    {
        None=-1,
        All = 0,
        Permission = 1,
        Department = 2,
        Employee = 3,
        DMDichVuCL = 4,
        DMDichVu = 5,
        DMDuoc = 6,
        DMNhomDichVu = 7,
        DMICD10 = 8,
        DMBenhVien = 9,
        DMTinhThanh = 10,
        DMQuanHuyen = 11,
        DMXaPhuong = 12,
        DMDanhMuc = 13,
        PatientsObject = 14,
        Feature = 15,
        SystemConfig = 16,
        AssetsType=17,
        AssetsCate=18,
        Asset=19,
        DMMucHuong=20
    }
}