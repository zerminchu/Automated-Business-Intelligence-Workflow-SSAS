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
    public class ProductDAC : DAC
    {
        public int GetProductWithId(string SkuId)
        {
            int result = 0;

            const string SQL_QUERY =
               @"SELECT [SKUId]
      ,[ProductCode]
      ,[CategoryCode]
      ,[DepartmentCode]
      ,[GroupCode]
      ,[DivisionCode]
      ,[UnitCost]
      ,[SupplierCode]
      ,[OriSKUId]
      ,[SKUDescription] FROM SKU WHERE ProductCode2 = @ProductId ";

            using (var sqlConnection = new SqlConnection(SQLConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQL_QUERY, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@ProductId", SkuId);

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

        public void UpsertProduct(SKUModel model)
        {
            const string SQL_QUERY =
               @" 
IF((SELECT COUNT(1) FROM DimDivisions WHERE DivisionCOde = @DivisionCode) > 0)
BEGIN
    UPDATE DimDivisions SET DivisionName = @DivisionName WHERE DivisionCode = @DivisionCode
END
ELSE
BEGIN
    INSERT INTO DimDivisions(DivisionCode, DivisionName) VALUES(@DivisionCode, @DivisionCode)
END

IF((SELECT COUNT(1) FROM DimGroups WHERE GroupCode = @GroupCode) > 0)
BEGIN
    UPDATE DimGroups SET GroupName = @GroupName WHERE GroupCode = @GroupCode 
END
ELSE
BEGIN
    INSERT INTO DimGroups(GroupCode, GroupName) VALUES(@GroupCode, @GroupName)
END

IF((SELECT COUNT(1) FROM DimDepartments WHERE DepartmentCode = @DepartmentCode) > 0)
BEGIN
    UPDATE DimDepartments SET DepartmentName = @DepartmentName WHERE DepartmentCode = @DepartmentCode
END
ELSE
BEGIN
    INSERT INTO DimDepartments(DepartmentCode, DepartmentName) VALUES(@DepartmentCode, @DepartmentName)
END

IF((SELECT COUNT(1) FROM DimCategories WHERE CategoryCode = @CategoryCode) > 0)
BEGIN
    UPDATE DimCategories SET CategoryName = @CategoryName WHERE CategoryCode = @CategoryCode
END
ELSE
BEGIN
    INSERT INTO DimCategories(CategoryName, CategoryCode) VALUES(@CategoryName, @CategoryCode)
END


IF((SELECT COUNT(1) FROM SKU WHERE ProductCode2 = @ProductCode2)>0)
BEGIN
    UPDATE SKU SET ProductCode2 = @ProductCode2, CategoryCode = @CategoryCode, 
DepartmentCode = @DepartmentCode, GroupCode = @GroupCode, DivisionCode = @DivisionCode, 
UnitCost = @UnitCost, SupplierCode = @SupplierCode, OriSKUId =@OriSKUId, SKUDescription = @SKUDescription
END
ELSE
BEGIN
    INSERT INTO SKU(
      [ProductCode]
      ,[ProductCode2]
      ,[CategoryCode]
      ,[DepartmentCode]
      ,[GroupCode]
      ,[DivisionCode]
      ,[UnitCost]
      ,[SupplierCode]
      ,[OriSKUId]
      ,[SKUDescription])
VALUES(@ProductCode
      ,@ProductCode2
      ,@CategoryCode
      ,@DepartmentCode
      ,@GroupCode
      ,@DivisionCode
      ,@UnitCost
      ,@SupplierCode
      ,@OriSKUId
      ,@SKUDescription)
END
";

            var result = new List<ProcessingJob>();
            using (var sqlConnection = new SqlConnection(SQLConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQL_QUERY, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@ProductCode", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ProductCode2", model.sku_item_code);
                    cmd.Parameters.AddWithValue("@CategoryCode", String.IsNullOrWhiteSpace(model.category_code)? "Default": model.category_code);
                    cmd.Parameters.AddWithValue("@CategoryName", String.IsNullOrWhiteSpace(model.category_description) ? "Default": model.category_description);
                    cmd.Parameters.AddWithValue("@DepartmentCode", String.IsNullOrWhiteSpace(model.department_code) ? "Default" : model.department_code);
                    cmd.Parameters.AddWithValue("@DepartmentName", String.IsNullOrWhiteSpace(model.department_description) ? "Default" : model.department_description);
                    cmd.Parameters.AddWithValue("@GroupCode", String.IsNullOrWhiteSpace(model.group_code) ? "Default" : model.group_code);
                    cmd.Parameters.AddWithValue("@GroupName", String.IsNullOrWhiteSpace(model.group_description) ? "Default" : model.group_description);
                    cmd.Parameters.AddWithValue("@DivisionCode", String.IsNullOrWhiteSpace(model.division_code) ? "Default" : model.division_code);
                    cmd.Parameters.AddWithValue("@DivisionName", String.IsNullOrWhiteSpace(model.division_description) ? "Default" : model.division_description);
                    cmd.Parameters.AddWithValue("@UnitCost", model.unit_price);
                    cmd.Parameters.AddWithValue("@SupplierCode", DBNull.Value);
                    cmd.Parameters.AddWithValue("@OriSKUId", DBNull.Value);
                    cmd.Parameters.AddWithValue("@SKUDescription", model.product_description);

                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
