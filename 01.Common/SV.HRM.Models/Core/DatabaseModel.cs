using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Models.Core
{
    public class DatabaseModel
    {
        public Guid Id { get; set; }
        public Guid? DatabaseManagementId { get; set; }
        public string FileName { get; set; }
        public DateTime? BackupOnDate { get; set; }
        public DateTime? RestoreOnDate { get; set; }
        public string Action { get; set; }
        public Guid? ActionByUserId { get; set; }
        public string ActionByUserName { get; set; }
        public string ActionResult { get; set; }

        public string DatabaseName { get; set; }

    }
    public class CreateBackupDatabaseModel
    {
        public Guid? DatabaseManagementId { get; set; }
        public string Action { get; set; }
        public Guid? ActionByUserId { get; set; }
        public string ActionByUserName { get; set; }
        public string DatabaseName { get; set; }
        public string BackupPath { get; set; }

    }
    public class ListRecordModel
    {
        public List<Guid> ListRecordId { get; set; }
    }
    public enum DatabaseQueryOrder
    {
        NGAY_THUC_THI_DESC,
    }
    public class DatabaseModelQueryFilter
    {
        public string TextSearch { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public Guid? Id { get; set; }
        public string Action { get; set; }
        public string ActionResult { get; set; }
        public DatabaseQueryOrder Order { get; set; }
        public DatabaseModelQueryFilter()
        {
            PageNumber = 1;
            PageSize = 10;
            TextSearch = string.Empty;
            Order = DatabaseQueryOrder.NGAY_THUC_THI_DESC;
        }
    }
    public class TranferFileModel
    {
        public Guid TranferFileId { get; set; }
        public string TranferFilePath { get; set; }
        public string TranferFileName { get; set; }
        public string TranferFileType { get; set; }
        public DateTime? CreateOnDate { get; set; }
        public DateTime? LastModifiOnDate { get; set; }
        public Guid? CreateOnUserId { get; set; }
        public int? TranferFileStatus { get; set; }
    }

    public class DatabaseHistoryLogModel
    {
        public Guid Id { get; set; }
        public string Action { get; set; }
        public DateTime? BackupRestoreOnDate { get; set; }
        public string FileName { get; set; }
        public string BackupRestoreDbName { get; set; }
        public Guid? StorageMediaId { get; set; }
        public string ActionByFullName { get; set; }

    }

    public class DatabaseLogQueryFilter
    {
        public string TextSearch { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public string Action { get; set; }
        public string FileName { get; set; }
        public string FullName { get; set; }
        public DatabaseQueryOrder Order { get; set; }
        public DatabaseLogQueryFilter()
        {
            PageNumber = 1;
            PageSize = 10;
            TextSearch = string.Empty;
            Order = DatabaseQueryOrder.NGAY_THUC_THI_DESC;
        }
    }

    public class DatabaseDeleteModel
    {
        public Guid? Id { get; set; }
        public string DatabaseName { get; set; }
    }
}
