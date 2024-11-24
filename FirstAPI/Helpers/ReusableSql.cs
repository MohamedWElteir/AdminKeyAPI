using System.Data;
using Dapper;
using FirstAPI.Data;
using FirstAPI.DTOs;

namespace FirstAPI.Helpers;

public class ReusableSql
{
    private readonly DataContextDapper _datacontextDapper;
    public ReusableSql()
    {
        _datacontextDapper = DataContextDapper.GetInstance();
    }

    public bool UpsertUser(UserDto user)
    {
        const string storedProcedure = "TutorialAppSchema.spUser_Upsert";
        var parameters = new DynamicParameters();
        foreach (var property in user.GetType().GetProperties())
        {
            parameters.Add($"@{property.Name}", property.GetValue(user));
        }
        return _datacontextDapper.ExecuteSql(storedProcedure, parameters, CommandType.StoredProcedure);
    }
}