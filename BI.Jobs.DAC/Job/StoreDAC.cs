using BI.Jobs.Shared.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.DAC.Job
{
    public class StoreDAC: DAC
    {
        public int GetStoreWithId(string storeId)
        {
            int result = 0;

            //const string SQL_QUERY =
            //   @"SELECT * FROM DimLocations WHERE LocationCode = @storeId ";

            const string SQL_QUERY =
               @"SELECT * FROM DimLocations WHERE LocationCode = @storeId ";

            using (var sqlConnection = new SqlConnection(SQLConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQL_QUERY, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@storeId", storeId);

                    sqlConnection.Open();

                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            result++;
                        }

                        return result;
                    }

                }
            }
        }

        
        public void UpsertStore(StoreModel model)
        {
            const string SQL_QUERY =
               @" 
IF((SELECT COUNT(1) FROM DimAreas WHERE AreaCode = @AreaCode) > 0)
BEGIN
    UPDATE DimAreas SET AreaName = @AreaName WHERE AreaCode = @AreaCode
END
ELSE
BEGIN
    INSERT INTO DimAreas(AreaCode, AreaName) VALUES(@AreaCode, @AreaName)
END

IF((SELECT COUNT(1) FROM DimCountries WHERE CountryCode = @CountryCode) > 0)
BEGIN
    UPDATE DimCountries SET CountryName = @CountryName WHERE CountryCode = @CountryCode
END
ELSE
BEGIN
    INSERT INTO DimCountries(CountryCode, CountryName) VALUES(@CountryCode, @CountryName)
END

IF((SELECT COUNT(1) FROM DimRegions WHERE RegionCode = @RegionCode) > 0)
BEGIN
    UPDATE DimRegions SET RegionName = @RegionName WHERE RegionCode = @RegionCode
END
ELSE
BEGIN
    INSERT INTO DimRegions(RegionCode, RegionName) VALUES(@RegionCode, @RegionName)
END


IF((SELECT COUNT(1) FROM DimLocations WHERE LocationCode = @LocationCode)>0)
BEGIN
    UPDATE DimLocations SET LocationName = @LocationName, 
PostalCode = @PostalCode, AreaCode = @AreaCode, CountryCode = @CountryCode, 
RegionCode = @RegionCode, OpeningDate = @OpeningDate 
 WHERE LocationCode = @LocationCode
END
ELSE
BEGIN
    INSERT INTO DimLocations(LocatioNCode, LocationName, PostalCode, AreaCode,
CountryCode, RegionCode, OpeningDate)
VALUES(@LocationCode, @LocationName, @PostalCode, @AreaCode,
@CountryCode, @RegionCode, @OpeningDate)
END
";

            var result = new List<ProcessingJob>();
            using (var sqlConnection = new SqlConnection(SQLConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQL_QUERY, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@LocationCode", model.code);
                    cmd.Parameters.AddWithValue("@LocationName", model.description);
                    cmd.Parameters.AddWithValue("@PostalCode", "");
                    cmd.Parameters.AddWithValue("@AreaCode", String.IsNullOrWhiteSpace(model.CustomField1) ? "Default": model.CustomField1);
                    cmd.Parameters.AddWithValue("@AreaName", String.IsNullOrWhiteSpace(model.CustomField1) ? "Default" : model.CustomField1);
                    cmd.Parameters.AddWithValue("@RegionCode", String.IsNullOrWhiteSpace(model.region) ? "Default" : model.region);
                    cmd.Parameters.AddWithValue("@RegionName", String.IsNullOrWhiteSpace(model.region) ? "Default" : model.region);
                    cmd.Parameters.AddWithValue("@CountryCode", String.IsNullOrWhiteSpace(model.country) ? "Default" : model.country);
                    cmd.Parameters.AddWithValue("@CountryName", String.IsNullOrWhiteSpace(model.country) ? "Default" : model.country);
                    cmd.Parameters.AddWithValue("@OpeningDate", DBNull.Value);
                   
                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
