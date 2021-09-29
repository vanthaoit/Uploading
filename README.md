
 -- Generate Excel Data
        DECLARE @FileName    varchar(100),
		@DirectoryPath    varchar(100),
		@SelectSQL VARCHAR(MAX),
		@Columns varchar(8000), 
		@sql varchar(8000), 
		@DataFile varchar(100)
           
        --SET @FileName='\\bedsqllpbk01\SQL_Full_Backups\Reports\VoucherReport\PROD\VoucherReport.xls'
        SET @FileName='\\Bedpsqlsi01\d$\DBA\VoucherReport\PROD\VoucherReport.xls'


        SET @DirectoryPath=substring(@FileName,1,len(@FileName)-charindex('\',reverse(@FileName)))
        --Delete old xls files
        SET @sql= 'EXEC master.dbo.xp_cmdshell ''DEL '+@DirectoryPath+'\*.xls'''
        EXEC(@sql)
        
        --Generate column names as a recordset        
        SET @Columns='''''ClientShortName'''' as ClientShortName,''''ChartID'''' as ChartID,''''AccountNumber'''' as AccountNumber,''''MRN'''' as MRN, ''''VoucherNumber'''' as VoucherNumber'
        --PRINT @Columns
        --Create a dummy file to have actual data
        SELECT @DataFile=@DirectoryPath+'\data_file.xls'


        --Generate column names in the passed EXCEL file
        SET @sql='exec master.dbo.xp_cmdshell ''bcp " select * from (select '+@Columns+') as t" queryout "'+@FileName+'" -c -t\t -T -S '+@@SERVERNAME+''''
        EXEC(@sql)


        --Generate data in the dummy file
        SET @SelectSQL='SELECT * FROM tempdb..##TempChartIDs Order By ClientShortName'
        
        SET @sql='exec master.dbo.xp_cmdshell ''bcp "'+@SelectSQL+'" queryout "'+@DataFile+'" -c -t\t -T -S '+@@SERVERNAME+''''
        EXEC(@sql)


        --Copy dummy file to passed EXCEL file
        SET @sql= 'exec master.dbo.xp_cmdshell ''type '+@DataFile+' >> "'+@FileName+'"'''
        EXEC(@sql)


        --Delete dummy file 
        SET @sql= 'exec master.dbo.xp_cmdshell ''del '+@DataFile+''''
        EXEC(@sql)
 
             
         --Send mail with Xls attachment
       DECLARE @msubject VARCHAR(500),@mbody VARCHAR(500)
       SET @msubject = ' Codify Voucher Report - as on '+CONVERT(VARCHAR,GETDATE(),101)+' '+SUBSTRING(CAST(CAST(GETDATE() AS TIME) AS VARCHAR),1,5)
       SET @mbody = 'Hi, <br><br>Please find attached Codify Voucher Report  <BR><BR> Regards, <br> Codify Support Team.'


        --   PRINT @msubject
        --   PRINT @mbody
        EXEC msdb.dbo.sp_send_dbmail
        @profile_name = 'BEDSQLSIAG',--'BEDSQLSIAG',
        @recipients = 'thao@gmail.com',
        @copy_recipients = 'thao@gmail.com',--'thao@gmail.com',
        @body = @mbody,
        @subject = @msubject,
        @body_format = 'HTML',
        @file_attachments = @FileName;





    

\\BedSQLLC02\d$\DBA\DMReport\PROD\DMReport.xls

