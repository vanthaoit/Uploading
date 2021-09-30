sp_configure 'show advanced options', 1 
GO 
RECONFIGURE; 
GO 
sp_configure 'Ole Automation Procedures', 1 
GO 
RECONFIGURE; 
GO 
sp_configure 'show advanced options', 1 
GO 
RECONFIGURE;

DECLARE @URL NVARCHAR(MAX) = 'http://localhost:3500/api/menu/getall';
Declare @Object as Int;
Declare @ResponseText as Varchar(8000);

Exec sp_OACreate 'MSXML2.XMLHTTP', @Object OUT;
Exec sp_OAMethod @Object, 'open', NULL, 'get',
       @URL,
       'False'
Exec sp_OAMethod @Object, 'send'
Exec sp_OAMethod @Object, 'responseText', @ResponseText OUTPUT
Select @ResponseText
--IF((Select @ResponseText) <> '')
--BEGIN
--     DECLARE @SuccMsg NVARCHAR(30) = 'OK API';
--     Print @SuccMsg;
--END
--ELSE
--BEGIN
--     DECLARE @ErroMsg NVARCHAR(30) = 'No data found.';
--     Print @ErroMsg;
--END
Exec sp_OADestroy @Object


--Delete dummy file 
      DECLARE @FileName    varchar(100),
		@DirectoryPath    varchar(100),
		@SelectSQL VARCHAR(MAX),
		@Columns nvarchar(MAX) = '', 
		@sql varchar(8000), 
		@DataFile varchar(100),
		@db_name varchar(8000) = N'LogixEligibility',
		@table_name varchar(8000) = N'eligibility.Artifacts',
		@file nvarchar(400) = 'DM Reporting 092920210913.xlsx'
SET @FileName='G:\DMReporting\'+@file
SET @sql= 'exec master.dbo.xp_cmdshell ''del '+@FileName+''''
EXEC(@sql)
