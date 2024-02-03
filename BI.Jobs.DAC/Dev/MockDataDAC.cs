using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.DAC.Dev
{
    public class MockDataDAC : DAC
    {
        public void CreateStore(string storeId, string storeName)
        {
            storeId = storeId.Replace("'", "");
            storeName = storeName.Replace("'", "");


            string SQL_QUERY = @$"
            BEGIN TRAN tx;

DECLARE @RegionCode NVARCHAR(50) = 'Default'
DECLARE @CountryCode NVARCHAR(50) = 'Default'
DECLARE @AreaCode NVARCHAR(50) = 'Default'
DECLARE @LocationCode NVARCHAR(50) = '{storeId}'


IF((SELECT COUNT(1) FROM DimRegions WHERE RegionCode = @RegionCode) <= 0)
BEGIN
	INSERT INTO DimRegions(RegionCode, RegionName)
	SELECT @RegionCode, @RegionCode
END


IF((SELECT COUNT(1) FROM DimCountries WHERE CountryCode = @CountryCode) <= 0)
BEGIN
	INSERT INTO DimCountries(CountryCode, CountryName)
	SELECT @CountryCode, @CountryCode
END

IF((SELECT COUNT(1) FROM DimAreas WHERE AreaCode = @AreaCode) <= 0)
BEGIN
	INSERT INTO DimAreas(AreaCode, AreaName)
	SELECT @AreaCode, @AreaCode
END


IF((SELECT COUNT(1) FROM DimLocations WHERE LocationCode = @LocationCode) <= 0)
BEGIN
	INSERT INTO DimLocations(LocationCode, LocationName, AreaCode, CountryCode, RegionCode)
	SELECT @LocationCode, '{storeName}', @AreaCode, @CountryCode, @RegionCode
END


COMMIT;";

            using (var sqlConnection = new SqlConnection(SQLConnectionString))
            {
                using (var cmd = new SqlCommand(SQL_QUERY, sqlConnection))
                {
                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void CreateProduct(string productId, string productName)
        {
            productId = productId.Replace("'", "");
            productName = productName.Replace("'", "");


            string SQL_QUERY = $@"

if((SELECT COUNT(1) FROM SKU WHERE ProductCode2 = '{productId}') <=0)
BEGIN
    insert into SKU(
      [ProductCode2]
      ,[CategoryCode]
      ,[DepartmentCode]
      ,[GroupCode]
      ,[DivisionCode]
      ,[UnitCost]
      ,[SupplierCode]
      ,[OriSKUId]
      ,[SKUDescription])
  select '{productId}','Default','Default','Default','Default',0,'Default',NULL,'{productName}'


END ";

            using (var sqlConnection = new SqlConnection(SQLConnectionString))
            {
                using (var cmd = new SqlCommand(SQL_QUERY, sqlConnection))
                {
                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
