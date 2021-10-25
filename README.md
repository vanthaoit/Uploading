DECLARE @URL NVARCHAR(MAX) = 'http://localhost:3500/api/menu/getall';
Declare @Object as Int;
Declare @ResponseText as Varchar(8000);

Exec sp_OACreate 'MSXML2.XMLHTTP', @Object OUT;
Exec sp_OAMethod @Object, 'open', NULL, 'get',
       @URL,
       'False'
Exec sp_OAMethod @Object, 'send'
Exec sp_OAMethod @Object, 'responseText', @ResponseText OUTPUT
IF((Select @ResponseText) <> '')
BEGIN
     DECLARE @SuccMsg NVARCHAR(30) = 'OK API';
     Print @SuccMsg;
END
ELSE
BEGIN
     DECLARE @ErroMsg NVARCHAR(30) = 'No data found.';
     Print @ErroMsg;
END
Exec sp_OADestroy @Object



ALTER PROCEDURE [eligibility].[sp_GetDeductibleVouchers]
@payerConfigId INT,
@tenantId INT,
@startDate DATETIME,
@endDate DATETIME,
@dateOffsets nvarchar(50) = '' -- Not used 

AS
BEGIN


	/*-----------------------------------------
	The type of request will alter the results.

	The user will select insurance, self pay or dectible monitoring file types.
	For DM - we will need to check the voucher flags table for two active entries.
	- High Acutity = 1
	- Held for DM = 1

	The below holds variables pertaining to those settings
	*/-----------------------------------------
	DECLARE @isHighAcuityFlag INT = (SELECT TOP 1 id FROM Billing.Logix.MdFlags WHERE Name = 'High Acuity')

	
  --Get all the payers that match the search criterion of the payer config id
	Create Table #billingDbCarriers (Carrier_ID_FK INT, Tenant_ID INT )
	INSERT INTO #billingDbCarriers (Carrier_ID_FK, Tenant_ID )
	  SELECT  Carrier_ID_FK, TenantId
	  FROM
	  (
	  SELECT  mdPay.Carrier_ID_FK, mdPay.TenantId
	  FROM [LogixEligibility].[eligibility].[MdPayor] mdPay WITH(NOLOCK)
	  INNER JOIN eligibility.XPayer_PayerConfig xwalk WITH(NOLOCK) ON xwalk.MdPayerId = mdPay.Id	
	  WHERE xwalk.MdPayerConfigId = @payerConfigId
	  AND mdPay.TenantId = @tenantId

	  UNION ALL
	   
	  SELECT  mdPay.Carrier_ID_FK, mdPay.TenantId
	  FROM [LogixEligibility].[eligibility].[MdPayor] mdPay WITH(NOLOCK)
	  INNER JOIN eligibility.MdPayorConfig config WITH(NOLOCK) ON config.PayerId = mdPay.PayerId
	  WHERE config.Id = @payerConfigId AND mdPay.TenantId = @tenantId
	  )a
	  Group by Carrier_ID_FK, TenantId

	  CREATE CLUSTERED INDEX Ix_billingDbCarriers_indx1 on #billingDbCarriers (Carrier_ID_FK ASC, Tenant_ID ASC )
	  
  
  -- Get the voucher information from based on the carriers you pulled above and the service date provided
 ;With vouchers (BillingDbVoucherId, Voucher_ID_FK, VoucherNumber, Patient_ID_FK, ServiceDate, Tenant_ID, Carrier_ID_FK, 
			  Location_ID_FK, Place_Of_Service_ID_FK, Actual_Prov_Practitioner_ID, Patient_Policy_ID, 
			  Date_Voided, Original_Billing_Date, Date_Updated, HasPayment, VoucherBalance) AS (

  	 SELECT distinct
	 v.Id AS BillingDbVoucherId, 
	 v.Voucher_ID_FK,
	 v.VoucherNumber, 
	 v.Patient_ID_FK, 
	 v.ServiceDate, 
	 v.Tenant_ID, 
	 c.Carrier_ID_FK, 
	 v.Location_ID_FK, 
	 v.Place_Of_Service_ID_FK, 
	 v.Actual_Prov_Practitioner_ID,
	 v.[Patient_Policy_ID],
	 v.Date_Voided,
	 v.Original_Billing_Date,
	 v.Date_Updated,
	 CASE WHEN ISNULL(v.PostedPayments, 0) = 0 THEN 0 ELSE 1 END AS HasPayment,
	 Fees - ISNULL (v.PostedPayments, 0) - ISNULL(v.PostedAdjustments, 0) + ISNULL(v.Posted_Refunds, 0) + ISNULL (v.Posted_Misc_Debits, 0) AS VoucherBalance
	 FROM Billing.PM.Vouchers v WITH(NOLOCK)
	 INNER JOIN #billingDbCarriers c WITH(NOLOCK) ON c.Tenant_ID = v.Tenant_ID ANd c.Carrier_ID_FK = v.Carrier_ID_FK  
	 WHERE 
	 (
		(v.Date_Updated >= @startDate and v.Date_Updated <= @endDate)
	 )
	AND v.Tenant_ID = @tenantId

), patients (Tenant_Id, Patient_ID_FK, MRN, FirstName, LastName, MiddleInitial, Date_Of_Birth, Sex, SSN) AS (

	SELECT v.Tenant_ID, p.Patient_ID_FK, p.MRN, c.FirstName, c.LastName, c.MiddleInitial, c.Date_Of_Birth, c.Sex,c.SSN
	FROM vouchers v
	INNER JOIN Billing.PM.Patients p WITH(NOLOCK) ON p.Patient_ID_FK = v.Patient_ID_FK AND p.Tenant_ID = v.Tenant_ID
	LEFT JOIN Billing.PM.Contacts c WITH(NOLOCK) ON c.Contact_ID_FK = p.Contact_ID_FK AND c.Tenant_ID = v.Tenant_ID


), policyInfo (BillingDbVoucherId, Certificate_No) AS (

	SELECT v.BillingDbVoucherId, p.Certificate_No
	FROM vouchers v
	INNER JOIN Billing.PM.Patient_Policies ptPolicy WITH(NOLOCK) ON v.Tenant_ID = ptPolicy.Tenant_ID AND v.Patient_Policy_ID = ptPolicy.Patient_Policy_ID  
	INNER JOIN Billing.PM.Policies p WITH(NOLOCK) ON p.Policy_ID = ptPolicy.Policy_ID AND p.Tenant_ID = v.Tenant_ID

), VoucherFlags (Voucher_ID_FK, Tenant_Id, FlagId) AS (
	
	SELECT flags.Voucher_Id_Fk, flags.Tenant_Id, flags.FlagId
	FROM Billing.Logix.VoucherFlags flags
	INNER JOIN vouchers v ON v.Tenant_ID = flags.Tenant_Id AND v.Voucher_ID_FK = flags.Voucher_Id_Fk
	WHERE (flags.FlagId = @isHighAcuityFlag) AND flags.IsActive = 1

) 	SELECT 
	v.BillingDbVoucherId,
	v.Tenant_ID AS TenantId, 
	v.VoucherNumber,
	v.ServiceDate AS DateOfService,
	p.MRN,
	p.Patient_ID_FK AS PatientId,
	p.FirstName AS PatientFirstName,
	p.LastName AS PatientLastName,
	p.MiddleInitial AS PatientMiddleInitial,
	p.Date_Of_Birth AS PatientDob,
	p.Sex AS PatientGender,
	p.SSN AS PatientSsn,
	pol.Certificate_No AS PatientMemberId,
	places.PlaceOfServiceName,
	places.City AS PlaceOfServiceCity,
	places.State AS PlaceOfServiceState,
	places.Zip AS PlaceOfServiceZip,
	pr.FirstName AS BillingProviderFirstName,
	pr.LastName AS BillingProviderLastName,
	pr.NPI AS BillingProverNpi,
	CASE WHEN 
		highAcutityFlags.FlagId IS NOT NULL AND 
		v.HasPayment = 0 AND 
		v.VoucherBalance != 0 AND 
		v.Original_Billing_Date IS NULL AND 
		v.Date_Updated IS NOT NULL AND 
		v.Date_Voided IS NULL
	THEN 1 ELSE 0 END AS QualifiesForDM,
	CASE WHEN highAcutityFlags.FlagId IS NOT NULL THEN 1 ELSE 0 END AS PassDmAcuityValidation,
	CASE WHEN v.HasPayment = 0 THEN 1 ELSE 0 END AS PassDmPaymentsValidation,
	CASE WHEN v.VoucherBalance != 0  THEN 1 ELSE 0 END AS PassDmBalanceValidation,
	CASE WHEN v.Original_Billing_Date IS NULL THEN 1 ELSE 0 END AS PassDmOrigBillDateValidation,
	CASE WHEN v.Date_Updated IS NOT NULL  THEN 1 ELSE 0 END AS PassDmDateUpdatedValidation,
	CASE WHEN v.Date_Voided IS NULL THEN 1 ELSE 0 END AS PassDmDateVoidedValidation
	FROM vouchers v
	LEFT JOIN patients p WITH(NOLOCK) ON p.Patient_ID_FK = v.Patient_ID_FK AND p.Tenant_Id = v.Tenant_ID
	LEFT JOIN Billing.PM.PlaceOfServices places WITH(NOLOCK) ON places.Place_Of_Service_ID_FK = v.Place_Of_Service_ID_FK AND places.Tenant_ID = v.Tenant_ID
	LEFT JOIN Billing.PM.Practitioners pr WITH(NOLOCK) ON pr.Practitioner_ID_FK = v.Actual_Prov_Practitioner_ID AND pr.Tenant_ID = v.Tenant_ID
	LEFT JOIN policyInfo pol WITH(NOLOCK) ON pol.BillingDbVoucherId = v.BillingDbVoucherId
	LEFT JOIN VoucherFlags highAcutityFlags ON highAcutityFlags.Tenant_Id = p.Tenant_Id AND highAcutityFlags.Voucher_ID_FK = v.Voucher_ID_FK AND highAcutityFlags.FlagId = @isHighAcuityFlag
    Group by 	
	v.BillingDbVoucherId,
	v.Tenant_ID , 
	v.VoucherNumber,
	v.ServiceDate ,
	p.MRN,
	p.Patient_ID_FK ,
	p.FirstName ,
	p.LastName ,
	p.MiddleInitial ,
	p.Date_Of_Birth ,
	p.Sex ,
	p.SSN ,
	pol.Certificate_No ,
	places.PlaceOfServiceName,
	places.City ,
	places.[State] ,
	places.Zip ,
	pr.FirstName ,
	pr.LastName ,
	pr.NPI ,
	CASE WHEN 
		highAcutityFlags.FlagId IS NOT NULL AND 
		v.HasPayment = 0 AND 
		v.VoucherBalance != 0 AND 
		v.Original_Billing_Date IS NULL AND 
		v.Date_Updated IS NOT NULL AND 
		v.Date_Voided IS NULL
	THEN 1 ELSE 0 END,	
	CASE WHEN highAcutityFlags.FlagId IS NOT NULL THEN 1 ELSE 0 END,
	CASE WHEN v.HasPayment = 0 THEN 1 ELSE 0 END ,
	CASE WHEN v.VoucherBalance != 0  THEN 1 ELSE 0 END ,
	CASE WHEN v.Original_Billing_Date IS NULL THEN 1 ELSE 0 END ,
	CASE WHEN v.Date_Updated IS NOT NULL  THEN 1 ELSE 0 END ,
	CASE WHEN v.Date_Voided IS NULL THEN 1 ELSE 0 END 


END