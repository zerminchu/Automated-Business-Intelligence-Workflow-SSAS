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

   
        public void CreateHeader(string requestId, string headerId, string storeId, DateTime transDt, Tlog data, Params params_data)
        {
            const string SQL_QUERY =
               @" insert into FactTransactionHeaders(
[TransactionHeaderId]
,[SaleId]
,[TransactionDate]
,[SubTotal]
,[TotalAmountIncludeTax]
,[Destination]
,[TransactionStartDateTime]
,[TransactionEndDateTime]
,[TotalRounding]
,[IsOverring]
,[DeletedItems]


)
Values(
@TransactionHeaderId
,@SaleId
,@TransactionDate
,@SubTotal
,@TotalAmountIncludeTax
,@Destination
,@TransactionStartDateTime
,@TransactionEndDateTime
,@TotalRounding
,@IsOverring
,@DeletedItems

) ";

            using (var sqlConnection = new SqlConnection(SQLConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQL_QUERY, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@TransactionHeaderId", headerId);
                    cmd.Parameters.AddWithValue("@SaleId", data.sale_id);

                    cmd.Parameters.AddWithValue("@TransactionDate", transDt);
                    cmd.Parameters.AddWithValue("@SubTotal", data.order_sub_total);
                    cmd.Parameters.AddWithValue("@Destination", data.destination);
                    cmd.Parameters.AddWithValue("@TransactionStartDateTime", data.transaction_start_datetime);
                    cmd.Parameters.AddWithValue("@TransactionEndDateTime", data.transaction_end_datetime);
                    cmd.Parameters.AddWithValue("@TotalRounding", data.total_rounding);
                    cmd.Parameters.AddWithValue("@IsOverring", data.is_overring);
                    cmd.Parameters.AddWithValue("@DeletedItems", data.deleted_items);



                    //string dtKey = transDt.ToString("yyyyMMdd");
                    // cmd.Parameters.AddWithValue("@DateKey", dtKey);
                    cmd.Parameters.AddWithValue("@TotalAmountIncludeTax", data.order_grand_total);

                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void CreateDetail(string requestId, SalesDetailModel data)
        {
            const string SQL_QUERY =
               @" 


INSERT INTO FactTransactionDetailsVM
(
[TransactionDetailId]
    ,[TransactionHeaderId]
    ,[TransactionStartDateTime]
    ,[ValueMealId]
    ,[ValueMealThirdPartyId]
    ,[ValueMealName]
    ,[ValueMealCount]
    ,[ValueMealAmount]
    ,[ValueMealSavings]
    ,[ValueMealDiscount]
    ,[ValueMealSubTotal]
    ,[ValueMealGrandTotal]
    ,[ValueMealTaxTotal]
    ,[ValueMealMode]
)
VALUES
(
NEWID()
    ,@TransactionHeaderId
    ,@TransactionStartDateTime
    ,@ValueMealId
    ,@ValueMealThirdPartyId
    ,@ValueMealName
    ,@ValueMealCount
    ,@ValueMealAmount
    ,@ValueMealSavings
    ,@ValueMealDiscount
    ,@ValueMealSubTotal
    ,@ValueMealGrandTotal
    ,@ValueMealTaxTotal
    ,@ValueMealMode
) ;";



            using (var sqlConnection = new SqlConnection(SQLConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQL_QUERY, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@TransactionHeaderId", data.HeaderId);
                    cmd.Parameters.AddWithValue("@TransactionStartDateTime", data.transaction_start_datetime);
                    cmd.Parameters.AddWithValue("@ValueMealId", data.valuemeal_id);
                    cmd.Parameters.AddWithValue("@ValueMealThirdPartyId", data.valuemeal_third_party_id);
                    cmd.Parameters.AddWithValue("@ValueMealName", data.valuemeal_name);
                    cmd.Parameters.AddWithValue("@ValueMealCount", data.ValueMealCount);
                    cmd.Parameters.AddWithValue("@ValueMealAmount", data.ValueMealAmount);
                    cmd.Parameters.AddWithValue("@ValueMealSavings", data.valuemeal_savings);
                    cmd.Parameters.AddWithValue("@ValueMealDiscount", data.ValueMealDiscount);
                    cmd.Parameters.AddWithValue("@ValueMealSubTotal", data.ValueMealSubTotal);
                    cmd.Parameters.AddWithValue("@ValueMealGrandTotal", data.ValueMealGrandTotal);
                    cmd.Parameters.AddWithValue("@ValueMealTaxTotal", data.ValueMealTaxTotal);
                    cmd.Parameters.AddWithValue("@ValueMealMode", data.valuemeal_mode);

                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }




        public void CreateDetail2(string requestId, ProductDetailModel data)
        {
            const string SQL_QUERY =
               @" 

 INSERT INTO FactTransactionDetails
(
[TransactionDetailId]
    ,[TransactionHeaderId]
    ,[ProductsId]
    ,[ProductsThirdPartyId]
    ,[ProductsName]
    ,[ProductsCount]
    ,[ProductsAmount]
    ,[ProductsDiscount]
    ,[ProductsSubTotal]
    ,[ProductsGrandTotal]
    ,[ProductsTaxTotal]
    ,[ProductsPrice]
    ,[ProductsAlacarte]
    ,[ProductsMode]


)
VALUES
(
NEWID()
    ,@TransactionHeaderId
    ,@ProductsId
    ,@ProductsThirdPartyId
    ,@ProductsName
    ,@ProductsCount
    ,@ProductsAmount
    ,@ProductsDiscount
    ,@ProductsSubTotal
    ,@ProductsGrandTotal
    ,@ProductsTaxTotal
    ,@ProductsPrice
    ,@ProductsAlacarte
    ,@ProductsMode
);";



            using (var sqlConnection = new SqlConnection(SQLConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQL_QUERY, sqlConnection))
                {


                    cmd.Parameters.AddWithValue("@TransactionHeaderId", data.HeaderId);
                    cmd.Parameters.AddWithValue("@ProductsId", data.product_id);
                    cmd.Parameters.AddWithValue("@ProductsThirdPartyId", data.product_third_party_id);
                    cmd.Parameters.AddWithValue("@ProductsName", data.product_name);
                    cmd.Parameters.AddWithValue("@ProductsCount", data.ProductsCount);
                    cmd.Parameters.AddWithValue("@ProductsAmount", data.ProductsAmount);
                    cmd.Parameters.AddWithValue("@ProductsDiscount", data.ProductsDiscount);
                    cmd.Parameters.AddWithValue("@ProductsSubTotal", data.ProductsSubTotal);
                    cmd.Parameters.AddWithValue("@ProductsGrandTotal", data.ProductsGrandTotal);
                    cmd.Parameters.AddWithValue("@ProductsTaxTotal", data.ProductsTaxTotal);
                    cmd.Parameters.AddWithValue("@ProductsPrice", data.product_price);
                    cmd.Parameters.AddWithValue("@ProductsAlacarte", data.product_alacarte);
                    cmd.Parameters.AddWithValue("@ProductsMode", data.product_mode);

                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }




        public void CreateDetail3(string requestId, TenderDetailModel data)
        {
            const string SQL_QUERY =
               @" 

 INSERT INTO FactTxnDetailsTender
(
[TransactionDetailId]
    ,[TransactionHeaderId]
    ,[TenderId]
    ,[TenderThirdPartyId]
    ,[TenderName]
    ,[TenderCount]
    ,[TenderAmount]
    ,[TenderIsChange]
    ,[TenderMode]
 

)
VALUES
(
NEWID()
    ,@TransactionHeaderId
    ,@TenderId
    ,@TenderThirdPartyId
    ,@TenderName
    ,@TenderCount
    ,@TenderAmount
    ,@TenderIsChange
    ,@TenderMode
);";



            using (var sqlConnection = new SqlConnection(SQLConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQL_QUERY, sqlConnection))
                {


                    cmd.Parameters.AddWithValue("@TransactionHeaderId", data.HeaderId);
                    cmd.Parameters.AddWithValue("@TenderId", data.tender_id);
                    cmd.Parameters.AddWithValue("@TenderThirdPartyId", data.tender_third_party_id);
                    cmd.Parameters.AddWithValue("@TenderName", data.tender_name);
                    cmd.Parameters.AddWithValue("@TenderCount", data.TenderCount);
                    cmd.Parameters.AddWithValue("@TenderAmount", data.TenderAmount);
                    cmd.Parameters.AddWithValue("@TenderIsChange", data.tender_is_change);
                    cmd.Parameters.AddWithValue("@TenderMode", data.tender_mode);
                 
                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void SummarizedStore(string requestId)
        {
            const string SQL_QUERY =
                @"";

            using (var sqlConnection = new SqlConnection(SQLConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQL_QUERY, sqlConnection))
                {
                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void SummarizedProduct(string requestId)
        {
            const string SQL_QUERY =
               @""; 
            using (var sqlConnection = new SqlConnection(SQLConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQL_QUERY, sqlConnection))
                {
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
                   @"EXEC dbo.sp_BI_Main_Data";
               
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
                                t.TenderName = GetDataValue<string>(dr, "TenderName");
                                t.TenderCount = GetDataValue<decimal>(dr, "TenderCount");
                                t.TenderAmount = GetDataValue<decimal>(dr, "TenderAmount");
                                t.TenderIsChange = GetDataValue<bool>(dr, "TenderIsChange");
                                t.TenderMode = GetDataValue<string>(dr, "TenderMode");

                                t.ValueMealName = GetDataValue<string>(dr, "ValueMealName");
                                t.ValueMealCount = GetDataValue<decimal>(dr, "ValueMealCount");
                                t.ValueMealAmount = GetDataValue<decimal>(dr, "ValueMealAmount");
                                t.ValueMealDiscount = GetDataValue<decimal>(dr, "ValueMealDiscount");
                                t.ValueMealSubTotal = GetDataValue<decimal>(dr, "ValueMealSubTotal");
                                t.ValueMealGrandTotal = GetDataValue<decimal>(dr, "ValueMealGrandTotal");
                                t.ValueMealTaxTotal = GetDataValue<decimal>(dr, "ValueMealTaxTotal");
                                t.ValueMealMode = GetDataValue<string>(dr, "ValueMealMode");

                                t.ProductsName = GetDataValue<string>(dr, "ProductsName");
                                t.ProductsCount = GetDataValue<decimal>(dr, "ProductsCount");
                                t.ProductsAmount = GetDataValue<decimal>(dr, "ProductsAmount");
                                t.ProductsDiscount = GetDataValue<decimal>(dr, "ProductsDiscount");
                                t.ProductsSubTotal = GetDataValue<decimal>(dr, "ProductsSubTotal");
                                t.ProductsGrandTotal = GetDataValue<decimal>(dr, "ProductsGrandTotal");
                                t.ProductsTaxTotal = GetDataValue<decimal>(dr, "ProductsTaxTotal");
                                t.ProductsPrice = GetDataValue<string>(dr, "ProductsPrice");
                                t.ProductsAlacarte = GetDataValue<string>(dr, "ProductsAlacarte");
                                t.ProductsMode = GetDataValue<string>(dr, "ProductsMode");

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
               @"";

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
            
                                t.ValueMealName = GetDataValue<string>(dr, "ValueMealName");

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
