using BI.Jobs.Shared.Enum;
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
    public class SalesDAC: DAC
    {


                public void DeleteSalesDataForStoreDate(string storeId, DateTime transDate)
        {
            const string SQL_QUERY = @"
                DELETE d
FROM FactTransactionDetails d
INNER JOIN FactTransactionHeaders h
  ON d.TransactionheaderId = h.TransactionHeaderId
WHERE  TransactionDate = @transDate


DELETE FactTransactionHeaders WHERE TransactionDate = @transDate

            ";

            using (var sqlConnection = new SqlConnection(SQLConnectionString))
            {
                using (var cmd = new SqlCommand(SQL_QUERY, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@StoreId", storeId);
                    cmd.Parameters.AddWithValue("@transDate", transDate);

                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

   
        public void CreateHeader(string requestId, string headerId, string storeId, DateTime transDt, Tlog data)
        {
            const string SQL_QUERY =
               @" insert into FactTransactionHeaders(
[TransactionHeaderId]
,[TransactionNo]
,[TransactionDate]
,[TransactionYear]
,[TransactionMonth]
,[TransactionDay]
,[TransactionHour]
,[PayCode]
,[CashierCode]
,[BasketSize]
,[CustomerId]
,[TotalAmount]
,[DateKey]
,[TotalAmountIncludeTax]
,[receipt_ref_no]
,[customId]
,[destination]

)
Values(
@TransactionHeaderId
,@TransactionNo
,@TransactionDate
,@TransactionYear
,@TransactionMonth
,@TransactionDay
,@TransactionHour
,@PayCode
,@CashierCode
,@BasketSize
,@CustomerId
,@TotalAmount
,@DateKey
,@TotalAmountIncludeTax
,@receipt_ref_no
,@customId
,@destination
) ";

            using (var sqlConnection = new SqlConnection(SQLConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQL_QUERY, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@TransactionHeaderId", headerId);
                    cmd.Parameters.AddWithValue("@TransactionNo", data.sale_id);
                    cmd.Parameters.AddWithValue("@TransactionDate", transDt);
                    cmd.Parameters.AddWithValue("@TransactionYear", transDt.Year);
                    cmd.Parameters.AddWithValue("@TransactionMonth", transDt.Month);
                    cmd.Parameters.AddWithValue("@TransactionDay", transDt.Day);
                    cmd.Parameters.AddWithValue("@TransactionHour", DBNull.Value);
                    cmd.Parameters.AddWithValue("@PayCode", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CashierCode", DBNull.Value);
                    cmd.Parameters.AddWithValue("@StoreCode", storeId);
                    cmd.Parameters.AddWithValue("@BasketSize", DBNull.Value);
                    //cmd.Parameters.AddWithValue("@CustomerId", String.IsNullOrWhiteSpace(data.customer_id) ? DBNull.Value : data.customer_id);
                    cmd.Parameters.AddWithValue("@CustomerId", DBNull.Value);
                    //cmd.Parameters.AddWithValue("@TotalAmount", data.order_sub_total);
                   cmd.Parameters.AddWithValue("@TotalAmount", data.order_grand_total);
                    cmd.Parameters.AddWithValue("@destination", data.destination);



                    string dtKey = transDt.ToString("yyyyMMdd");
                    cmd.Parameters.AddWithValue("@DateKey", dtKey);
                    cmd.Parameters.AddWithValue("@TotalAmountIncludeTax", data.order_grand_total);

                    cmd.Parameters.AddWithValue("@receipt_ref_no", DBNull.Value);
                    cmd.Parameters.AddWithValue("@customId", requestId);

                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void CreateDetail(string requestId, SalesDetailModel data)
        {
            const string SQL_QUERY =
               @" 


INSERT INTO FactTransactionDetails
(
[TransactionDetailId]
,[TransactionHeaderId]
,[Quantity]
,[UnitPrice]
,[TotalAmountIncludeTax]
,[transaction_start_datetime]
,[SKU]
,[savings]


)
VALUES
(
NEWID()
,@TransactionHeaderId
,@Quantity
,@UnitPrice
,@TotalAmountIncludeTax
,@transaction_start_datetime
,@SKU
,@savings

) ";

            using (var sqlConnection = new SqlConnection(SQLConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQL_QUERY, sqlConnection))
                {
                    //cmd.Parameters.AddWithValue("@TransactionDetailId", headerId);
                    cmd.Parameters.AddWithValue("@TransactionHeaderId", data.HeaderId);
                    cmd.Parameters.AddWithValue("@ProductCode", data.SkuId);
                    cmd.Parameters.AddWithValue("@Quantity", data.Qty);
                    cmd.Parameters.AddWithValue("@UnitPrice", data.UnitPrice);
                    cmd.Parameters.AddWithValue("@TotalAmountIncludeTax", data.TotalAmountWithTax);
                    cmd.Parameters.AddWithValue("@transaction_start_datetime", data.transaction_start_datetime);
                    cmd.Parameters.AddWithValue("@SKU", data.SkuId);
                    cmd.Parameters.AddWithValue("@savings", data.savings);



                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SummarizedStore(string requestId, string DateKey)
        {
            const string SQL_QUERY =
               @" 
EXEC spSummarized_Sum_Stores_WithoutHour @DateKey";

            using (var sqlConnection = new SqlConnection(SQLConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQL_QUERY, sqlConnection))
                {
                    //cmd.Parameters.AddWithValue("@TransactionDetailId", headerId);
                    cmd.Parameters.AddWithValue("@DateKey", DateKey);
                  //  cmd.Parameters.AddWithValue("@LocationCode", LocationCode);
                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SummarizedProduct(string requestId, string DateKey)
        {
            const string SQL_QUERY =
               @" 
EXEC spSummarized_Sum_Products_WithoutHour @DateKey"; // change the stored procedure 

            using (var sqlConnection = new SqlConnection(SQLConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQL_QUERY, sqlConnection))
                {
                    //cmd.Parameters.AddWithValue("@TransactionDetailId", headerId);
                    cmd.Parameters.AddWithValue("@DateKey", DateKey);
                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<SummarizedStore> GetSummarizedStore()
        {
            List<SummarizedStore> result = new List<SummarizedStore>();
            try
            {
               const string SQL_QUERY =
                   @"SELECT [Year]
                  ,[FiscalYear]
                  ,[Month]
                  ,[FiscalMonth]
                  ,[DayOfMonth]
                  ,[Quarter]
                  ,[FiscalQuarter]
                  ,[DayOfYear]
                  ,[WeekOfMonth]
                  ,[FiscalWeekOfYear]
                  ,[DateKey]
                  ,[RegionName]
                  ,[CountryName]
                  ,[AreaName]
               
                  ,[LocationName]
                  ,[TotalTransaction]
                  ,[TotalAmount]
                  ,[BasketSize]
              FROM [Sum_Stores_WithoutHour] ";

                using (var sqlConnection = new SqlConnection(SQLConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(SQL_QUERY, sqlConnection))
                    {
                        sqlConnection.Open();

                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var t = new SummarizedStore();
                                t.Year = GetDataValue<string>(dr, "Year");
                                t.FiscalYear = GetDataValue<string>(dr, "FiscalYear");
                                t.Month = GetDataValue<string>(dr, "Month");
                                t.FiscalMonth = GetDataValue<string>(dr, "FiscalMonth");
                                t.DayOfMonth = GetDataValue<string>(dr, "DayOfMonth");
                                t.Quarter = GetDataValue<string>(dr, "Quarter");
                                t.FiscalQuarter = GetDataValue<string>(dr, "FiscalQuarter");
                                t.DayOfYear = GetDataValue<string>(dr, "DayOfYear");
                                t.WeekOfMonth = GetDataValue<string>(dr, "WeekOfMonth");
                                t.FiscalWeekOfYear = GetDataValue<string>(dr, "FiscalWeekOfYear");
                                t.DateKey = GetDataValue<int>(dr, "DateKey");
                                t.RegionName = GetDataValue<string>(dr, "RegionName");
                                t.CountryName = GetDataValue<string>(dr, "CountryName");
                                t.AreaName = GetDataValue<string>(dr, "AreaName");
                               // t.LocationCode = GetDataValue<string>(dr, "LocationCode");
                                t.LocationName = GetDataValue<string>(dr, "LocationName");
                                t.TotalTransaction = GetDataValue<int>(dr, "TotalTransaction");
                                t.TotalAmount = GetDataValue<decimal>(dr, "TotalAmount");
                                t.BasketSize = (GetDataValue<decimal>(dr, "BasketSize")).ToString();
                                //t.x = GetDataValue<string>(dr, "x");
                                result.Add(t);
                            }

                            return result;
                        }

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<SummarizedProduct> GetSummarizedProduct()
        {
            List<SummarizedProduct> result = new List<SummarizedProduct>();
            try
            {
                const string SQL_QUERY =
               @"SELECT[Year]
      ,[FiscalYear]
      ,[Month]
      ,[FiscalMonth]
      ,[DayOfMonth]
      ,[Quarter]
      ,[FiscalQuarter]
      ,[DayOfYear]
      ,[WeekOfMonth]
      ,[FiscalWeekOfYear]
      ,[DateKey]
      ,[ProductName]
      ,''
	  ,''
	  ,''
	  ,''
      ,[TotalTransaction]
      ,[TotalAmount]
      ,[BasketSize]

                FROM[Sum_Stores_WithoutHour]; ";

                using (var sqlConnection = new SqlConnection(SQLConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(SQL_QUERY, sqlConnection))
                    {
                        sqlConnection.Open();

                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var t = new SummarizedProduct();
                                t.Year = GetDataValue<string>(dr, "Year");
                                t.FiscalYear = GetDataValue<string>(dr, "FiscalYear");
                                t.Month = GetDataValue<string>(dr, "Month");
                                t.FiscalMonth = GetDataValue<string>(dr, "FiscalMonth");
                                t.DayOfMonth = GetDataValue<string>(dr, "DayOfMonth");
                                t.Quarter = GetDataValue<string>(dr, "Quarter");
                                t.FiscalQuarter = GetDataValue<string>(dr, "FiscalQuarter");
                                t.DayOfYear = GetDataValue<string>(dr, "DayOfYear");
                                t.WeekOfMonth = GetDataValue<string>(dr, "WeekOfMonth");
                                t.FiscalWeekOfYear = GetDataValue<string>(dr, "FiscalWeekOfYear");
                                t.DateKey = GetDataValue<int>(dr, "DateKey");
                                t.ProductName = GetDataValue<string>(dr, "ProductName");
                                t.CategoryName = "";
                                t.DepartmentName = "";
                                t.GroupName = "";
                                t.DivisionName = "";
                                t.TotalTransaction = 0;
                                t.TotalAmount = GetDataValue<decimal>(dr, "TotalAmount");
                                t.BasketSize = GetDataValue<decimal>(dr, "BasketSize");

                                result.Add(t);
                            }

                            return result;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
